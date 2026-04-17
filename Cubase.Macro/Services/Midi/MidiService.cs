using Cubase.Macro.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace Cubase.Macro.Services.Midi
{
    public class MidiService : IMidiService, IDisposable
    {
        private NutstoneDriver midiDriver;

        private readonly ILogger<MidiService> logger;

        private readonly IServiceProvider serviceProvider;

        private Action<bool>? OnCommandComplete { get; set; } = null;

        public Action? OnReadyReceived { get; set; } = null;

        public bool ReadyReceived { get; set; } = false;


        private bool disposed;


        private Dictionary<string, Action<string>> commandProcessors;

        public MidiService(ILogger<MidiService> logger, IServiceProvider serviceProvider) 
        { 
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        public void Initialise()
        {
            this.logger.LogInformation("Initialising Nutstone Midi ..");
            this.midiDriver = new NutstoneDriver("Nutstone");
            this.midiDriver.MidiMessageReceived += MidiDriver_MidiMessageReceived;
        }

        public bool SendMidiMessage(CubaseKeyCommand cubaseMidiCommand)
        {
            try
            {
                this.midiDriver.SendNoteOn(cubaseMidiCommand.Channel, cubaseMidiCommand.Note, 127);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public void SendSysExMessage<T>(MidiCommand command, T request)
        {
            this.midiDriver.SendMessage(command, request);  
        }


        private void MidiDriver_MidiMessageReceived(byte[] message)
        {
            if (message.Length >= 3 && message[0] == 0xF0 && message[1] == 0x7D)
            {
                // Extract the payload between 0xF0..0xF7
                int endIndex = Array.IndexOf(message, (byte)0xF7, 0);
                if (endIndex == -1) return; // incomplete

                // Convert SysEx bytes to string
                List<byte> content = new List<byte>();
                for (int i = 2; i < endIndex; i++) content.Add((byte)(message[i] & 0x7F));

                // Split command / message at separator 0x00
                int sep = content.IndexOf(0x00);
                if (sep == -1) return;

                string command = Encoding.ASCII.GetString(content.Take(sep).ToArray());
                string payload = Encoding.ASCII.GetString(content.Skip(sep + 1).ToArray());
                Debug.WriteLine($"Received Command {command}");
            }
        }

        private void MessageReceived(string message)
        {
            this.logger.LogInformation($"Message received: {message}");
        }

        private void Ready(string emptyString)
        {
            this.ReadyReceived = true;
            this.logger.LogInformation($"Cubase Midi Sync is ready..");
            if (OnReadyReceived != null)
            {
                OnReadyReceived();
            }
        }

       
        public void VerifyDriver()
        {
            this.ReadyReceived = false;
            this.SendSysExMessage(MidiCommand.Ready, "{}");
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            this.midiDriver.Dispose();
          
        }

        public void RestartMidi()
        {
            this.midiDriver.RestartPort();
        }
    }
}
