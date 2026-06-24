using Cubase.Macro.Common.Models;
using Cubase.Macro.Models;

namespace Cubase.Macro.Services.Midi
{
    public interface IMidiService
    {
        public void Initialise();

        public bool VolumeChanging { get;  set; }

        public CueLevelCollection CueCollection { get; }

        public TrackCollection TrackCollection { get; }

        public TransportLocationCollection TransportLocation { get; }

        public bool SendMidiMessage(CubaseKeyCommand cubaseMidiCommand);

        public void SendSysExMessage<T>(MidiCommand command, T request);

        public void RegisterForUpdateCueLevelsEndCallbacks(Action onUpdateCueLevelsEnd);

        public void DeRegisterForUpdateCueLevelsEndCallbacks(Action onUpdateCueLevelsEnd);

        public void RegisterForGetCueLevelsEndCallbacks(Action onGetCueLevelsEnd);

        public void DeRegisterForGetCueLevelsEndCallbacks(Action onGetCueLevelsEnd);

        public void VerifyDriver();

        void SuspendCueChecking();

        void ResumeCueChecking();

        void RestartMidi();

        void Dispose();

        public void GetCueCollection();

        public void ReloadScripts();

        public Action? OnReadyReceived { get; set; }

        public Action<CubaseMidiResponse> OnMidiResponse {  get; set; }
        
        bool Initialised { get; }

    }
}
