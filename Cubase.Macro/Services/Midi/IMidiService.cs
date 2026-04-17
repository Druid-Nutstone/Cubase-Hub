using Cubase.Macro.Models;

namespace Cubase.Macro.Services.Midi
{
    public interface IMidiService
    {
         public void Initialise();

        public bool SendMidiMessage(CubaseKeyCommand cubaseMidiCommand);

        public void SendSysExMessage<T>(MidiCommand command, T request);

        public void VerifyDriver();

        void RestartMidi();

        void Dispose();

        public Action? OnReadyReceived { get; set; }

    }
}
