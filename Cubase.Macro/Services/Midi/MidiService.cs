using Cubase.Macro.Common.Models;
using Cubase.Macro.Models;
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

        private NutstoneDriver? midiDriver;

        private readonly BlockingCollection<byte[]> messageQueue = new();
        private CancellationTokenSource? cancellationTokenSource;
        private Task? workerTask;

        private bool disposed;

        public Action<bool>? OnCommandComplete { get; set; }
        public Action? OnReadyReceived { get; set; }

        public Action<CubaseMidiResponse> OnMidiResponse {  get; set; }

        public bool ReadyReceived { get; private set; }

        public MidiService(
            ILogger<MidiService> logger,
            IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
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
            switch (command.ToUpperInvariant())
            {
                case "READY":
                    HandleReady();
                    break;

                case "COMPLETE":
                    OnCommandComplete?.Invoke(true);
                    break;

                case "FAILED":
                    OnCommandComplete?.Invoke(false);
                    break;
                case "COMMANDVALUECHANGED":
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
    }
}