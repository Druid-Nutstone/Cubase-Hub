using Cubase.Macro.Common.Models;
using Cubase.Macro.Models;
using Cubase.Macro.Services.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Cubase.Macro.Services.Midi
{
    public class MidiService : IMidiService, IDisposable
    {
        private readonly ILogger<MidiService> logger;
        private readonly IServiceProvider serviceProvider;
        private IConfigurationService configurationService;

        private NutstoneDriver? midiDriver;

        private readonly BlockingCollection<byte[]> messageQueue = new();
        private CancellationTokenSource? cancellationTokenSource;
        private Task? workerTask;

        private CueLevelCollection cueLevels = new CueLevelCollection();

        private bool disposed;

        private bool inGettingCueLevels = false;

        private System.Timers.Timer cueTimer = new();

        public Action<bool>? OnCommandComplete { get; set; }
        public Action? OnReadyReceived { get; set; }

        public List<Action> GetCueLevelsEndCallbacks { get; } = new List<Action>(); 

        public List<Action> UpdatedCueLevelsEndCallbacks { get; } = new List<Action>();

        public Action<CubaseMidiResponse> OnMidiResponse {  get; set; }

        public bool ReadyReceived { get; private set; }

        public CueLevelCollection CueCollection => this.cueLevels ?? new CueLevelCollection();

        public MidiService(
            ILogger<MidiService> logger,
            IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            this.configurationService = this.serviceProvider.GetService<IConfigurationService>();
            cueTimer.Elapsed += CueTimer_Elapsed;
            cueTimer.Interval = 2000; // every 2 seconds
        }


        private void CueTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.cueLevels.HaveAtLeastOneChange && !inGettingCueLevels)
            {
                this.HandleCueLevelsUpdated();
            }
        }

        public void Initialise()
        {
            logger.LogInformation("Initialising Nutstone MIDI...");

            midiDriver = new NutstoneDriver("Nutstone");
            midiDriver.MidiMessageReceived += MidiDriver_MidiMessageReceived;

            cancellationTokenSource = new CancellationTokenSource();

            workerTask = Task.Run(() =>
                ProcessMessages(cancellationTokenSource.Token),
                cancellationTokenSource.Token);
        }

        public bool SendMidiMessage(CubaseKeyCommand cubaseMidiCommand)
        {
            try
            {
                logger.LogInformation($"Sending Midi {cubaseMidiCommand.Name} Ch:{cubaseMidiCommand.Channel} Note:{cubaseMidiCommand.Note}");
                midiDriver?.SendNoteOn(
                    cubaseMidiCommand.Channel,
                    cubaseMidiCommand.Note,
                    127);

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send MIDI note.");
                return false;
            }
        }

        public void SendSysExMessage<T>(MidiCommand command, T request)
        {
            try
            {
                midiDriver?.SendMessage(command, request);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send SysEx message.");
            }
        }

        public void VerifyDriver()
        {
            ReadyReceived = false;
            SendSysExMessage(MidiCommand.Ready, "{}");
        }

        public void RestartMidi()
        {
            midiDriver?.RestartPort();
        }

        private void MidiDriver_MidiMessageReceived(byte[] message)
        {
            if (!messageQueue.IsAddingCompleted)
            {
                messageQueue.Add(message);
            }
        }

        private void ProcessMessages(CancellationToken token)
        {
            try
            {
                foreach (var message in messageQueue.GetConsumingEnumerable(token))
                {
                    try
                    {
                        HandleMidiMessage(message);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error processing MIDI message.");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("MIDI worker stopped.");
            }
        }

        private void HandleMidiMessage(byte[] message)
        {
            if (message.Length < 3)
                return;

            if (message[0] != 0xF0 || message[1] != 0x7D)
                return;

            int endIndex = Array.IndexOf(message, (byte)0xF7);

            if (endIndex == -1)
                return;

            List<byte> content = new();

            for (int i = 2; i < endIndex; i++)
            {
                content.Add((byte)(message[i] & 0x7F));
            }

            int separatorIndex = content.IndexOf(0x00);

            if (separatorIndex == -1)
                return;

            string command =
                Encoding.ASCII.GetString(content.Take(separatorIndex).ToArray());

            string payload =
                Encoding.ASCII.GetString(content.Skip(separatorIndex + 1).ToArray());

            logger.LogInformation("Received MIDI Command: {Command}", command);

            ProcessCommand(command, payload);
        }

        private void ProcessCommand(string command, string payload)
        {
            switch (command.ToLowerInvariant())
            {
                case var _ when command.Equals(MidiCommand.Ready.ToString(), StringComparison.OrdinalIgnoreCase):
                    HandleReady();
                    break;
                case var _ when command.Equals(MidiCommand.ClearChannels.ToString(), StringComparison.OrdinalIgnoreCase):
                    this.cueLevels = new CueLevelCollection();
                    break;
                case var _ when command.Equals(MidiCommand.CommandComplete.ToString(), StringComparison.OrdinalIgnoreCase):
                    OnCommandComplete?.Invoke(true);
                    break;
                case var _ when command.Equals(MidiCommand.CueLevelChange.ToString(), StringComparison.OrdinalIgnoreCase):
                    var cueResponse = JsonSerializer.Deserialize<CueLevelChange>(payload);
                    this.HandleCueLevelChange(cueResponse);
                    break;
                case var _ when command.Equals(MidiCommand.GetCueLevelsComplete.ToString(), StringComparison.OrdinalIgnoreCase):
                    this.HandleCueLevelsUpdated();
                    break;
                case var _ when command.Equals(MidiCommand.Failed.ToString(), StringComparison.OrdinalIgnoreCase):
                    OnCommandComplete?.Invoke(false);
                    break;
                case var _ when command.Equals(MidiCommand.UpdateCueLevelComplete.ToString(), StringComparison.OrdinalIgnoreCase):
                    foreach (var callback in UpdatedCueLevelsEndCallbacks)
                    {
                        try
                        {
                            callback.Invoke();
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "Error invoking UpdateCueLevelsEnd callback.");
                        }
                    }
                    break;
                case var _ when command.Equals(MidiCommand.CommandValueChanged.ToString(), StringComparison.OrdinalIgnoreCase):
                    try
                    {
                        var midiResponse = JsonSerializer.Deserialize<CubaseMidiResponse>(payload);
                        OnMidiResponse?.Invoke(midiResponse);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"Error deserialising CommandValueChanged");
                    }
                    break;
                default:
                    logger.LogInformation(
                        "Unhandled MIDI command: {Command} Payload: {Payload}",
                        command,
                        payload);
                    break;
            }
        }

        private void HandleCueLevelsUpdated()
        {
            this.inGettingCueLevels = false;
            this.cueLevels.HaveAtLeastOneChange = false;
            foreach (var callback in GetCueLevelsEndCallbacks)
            {
                try
                {
                    callback.Invoke();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error invoking GetCueLevelsEnd callback.");
                }
            }
        }

        private void HandleCueLevelChange(CueLevelChange cueLevelChange)
        {
            if (cueLevelChange == null)
                return;

            var cueName =
                this.configurationService != null
                    ? this.configurationService.Configuration.CueNames[cueLevelChange.CueIndex]
                    : $"Cue{cueLevelChange.CueIndex}";

            this.cueLevels.UpdateCueLevel(cueLevelChange, cueName);
        }

        private void HandleReady()
        {
            ReadyReceived = true;

            logger.LogInformation("Cubase MIDI Sync is ready.");

            OnReadyReceived?.Invoke();
        }

        public void Dispose()
        {
            if (disposed)
                return;

            disposed = true;

            try
            {
                cancellationTokenSource?.Cancel();

                messageQueue.CompleteAdding();

                workerTask?.Wait(TimeSpan.FromSeconds(2));
            }
            catch
            {
                // ignore shutdown exceptions
            }

            if (midiDriver != null)
            {
                midiDriver.MidiMessageReceived -= MidiDriver_MidiMessageReceived;
                midiDriver.Dispose();
            }

            cancellationTokenSource?.Dispose();
            messageQueue.Dispose();
        }

        public void RegisterForGetCueLevelsEndCallbacks(Action onGetCueLevelsEnd)
        {
            this.GetCueLevelsEndCallbacks.Add(onGetCueLevelsEnd);
        }

        public void DeRegisterForGetCueLevelsEndCallbacks(Action onGetCueLevelsEnd)
        {
            this.GetCueLevelsEndCallbacks.Remove(onGetCueLevelsEnd); 
        }

        public void GetCueCollection()
        {
            this.inGettingCueLevels = true;
            this.SendSysExMessage(MidiCommand.GetCueLevels, "{}");
        }

        public void RegisterForUpdateCueLevelsEndCallbacks(Action onUpdateCueLevelsEnd)
        {
            this.UpdatedCueLevelsEndCallbacks.Add(onUpdateCueLevelsEnd);
        }

        public void DeRegisterForUpdateCueLevelsEndCallbacks(Action onUpdateCueLevelsEnd)
        {
            this.UpdatedCueLevelsEndCallbacks.Remove(onUpdateCueLevelsEnd);
        }

        public void SuspendCueChecking()
        {
            this.cueTimer.Stop();
        }

        public void ResumeCueChecking()
        {
            this.cueTimer.Start();
        }
    }
}