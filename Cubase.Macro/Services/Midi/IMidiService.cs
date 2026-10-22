using Cubase.Macro.Common.Models;
using Cubase.Macro.Models;

namespace Cubase.Macro.Services.Midi
{
    public interface IMidiService
    {
        public void Initialise();

        public bool VolumeChanging { get; set; }

        bool MonitoringTransport { get; }

        Action<bool>? OnCommandComplete { get; set; }

        public CueLevelCollection CueCollection { get; }

        public TrackCollection TrackCollection { get; }

        public TransportLocationCollection TransportLocation { get; }

        public bool SendMidiMessage(CubaseKeyCommand cubaseMidiCommand);

        public void SendSysExMessage<T>(MidiCommand command, T request);

        public void RegisterForTracksComplete(Action<TrackCollection> onTracksComplete);

        public void DeRegisterForTracksComplete(Action<TrackCollection> onTracksComplete);

        public void RegisterForUpdateCueLevelsEndCallbacks(Action onUpdateCueLevelsEnd);

        public void DeRegisterForUpdateCueLevelsEndCallbacks(Action onUpdateCueLevelsEnd);

        public void RegisterForGetCueLevelsEndCallbacks(Action onGetCueLevelsEnd);

        public void RegisterForGetPluginSlots(Action<CubasePluginSlotCollection> onPluginSlot);

        public void RegisterForGetPluginSlotsComplete(Action onPluginSlotComplete);

        public void DeRegisterForGetPluginSlotsComplete(Action onPluginSlotComplete);

        public void DeRegisterForGetPluginSlots(Action<CubasePluginSlotCollection> onPluginSlot);

        public void RegisterForGetPluginParameters(Action<CubasePluginParameterCollection> onPluginParameter);

        public void RegisterForGetPluginDetail(Action<CubasePluginDetailCollection> onPluginDetail);

        public void DeRegisterForGetPluginDetail(Action<CubasePluginDetailCollection> onPluginDetail);

        public void DeRegisterForGetPluginParameters(Action<CubasePluginParameterCollection> onPluginParameter);

        public void RegisterForGetPluginParametersComplete(Action onPluginParameterComplete);

        public void DeRegisterForGetPluginParametersComplete(Action onPluginParameterComplete);

        public void RegisterForGetPluginDetailsComplete(Action onPluginDetailComplete);

        public void DeRegisterForGetPluginDetailsComplete(Action onPluginDetailComplete);

        public void DeRegisterForGetCueLevelsEndCallbacks(Action onGetCueLevelsEnd);

        public void RegisterForTransportMonitoring(Action<bool> onMonitoringChanged);

        public void VerifyDriver();

        void SuspendCueChecking();

        void ResumeCueChecking();

        void RestartMidi();

        void Dispose();

        void GetCueCollection();

        void StartTransportMonitoring();

        void StopTransportMonitoring();

        void ReloadScripts();

        Action? OnReadyReceived { get; set; }

        Action<CubaseMidiResponse> OnMidiResponse { get; set; }

        void GetTracks();

        bool Initialised { get; }

    }
}
