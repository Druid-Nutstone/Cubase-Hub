using Cubase.Macro.Common.Models;
using Cubase.Macro.Models;
using Cubase.Macro.Services.Midi;

namespace Cubase.Macro.Tests.Fakes
{
    public class FakeMidiService : IMidiService
    {

        public bool MonitoringTransport => false;

        public CueLevelCollection CueCollection => new CueLevelCollection();

        public TrackCollection TrackCollection => new FakeTrackCollection()
        {
            new Track() { Id = "FakeTrack1", Name = "Track 1"},
            new Track() { Id = "FakeTrack2Selected", Name = "Track 2", Selected = true }
        };

        public TransportLocationCollection TransportLocation => new TransportLocationCollection();

        public TransportModel TransportModel => new TransportModel();

        public Action? OnReadyReceived { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Action<CubaseMidiResponse> OnMidiResponse { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Initialised => true;

        private bool volumechanging = false;

        public bool VolumeChanging { get => true; set => volumechanging = value; }

        private Action<bool>? onCommandComplete;

        public Action<bool>? OnCommandComplete { get => onCommandComplete; set => onCommandComplete = value; }

        public void DeRegisterForGetCueLevelsEndCallbacks(Action onGetCueLevelsEnd)
        {

        }

        public void DeRegisterForGetPluginDetail(Action<CubasePluginDetailCollection> onPluginDetail)
        {
        }

        public void DeRegisterForGetPluginDetailsComplete(Action onPluginDetailComplete)
        {
        }

        public void DeRegisterForGetPluginParameters(Action<CubasePluginParameterCollection> onPluginParameter)
        {
        }

        public void DeRegisterForGetPluginParametersComplete(Action onPluginParameterComplete)
        {
        }

        public void DeRegisterForGetPluginSlots(Action<CubasePluginSlotCollection> onPluginSlot)
        {
        }

        public void DeRegisterForGetPluginSlotsComplete(Action onPluginSlotComplete)
        {
        }

        public void DeRegisterForTracksComplete(Action<TrackCollection> onTracksComplete)
        {
        }

        public void DeRegisterForUpdateCueLevelsEndCallbacks(Action onUpdateCueLevelsEnd)
        {
        }

        public void Dispose()
        {
        }

        public void GetCueCollection()
        {
        }

        public void GetTracks()
        {
        }

        public void Initialise()
        {
        }

        public void RegisterForGetCueLevelsEndCallbacks(Action onGetCueLevelsEnd)
        {
        }

        public void RegisterForGetPluginDetail(Action<CubasePluginDetailCollection> onPluginDetail)
        {
        }

        public void RegisterForGetPluginDetailsComplete(Action onPluginDetailComplete)
        {
        }

        public void RegisterForGetPluginParameters(Action<CubasePluginParameterCollection> onPluginParameter)
        {
        }

        public void RegisterForGetPluginParametersComplete(Action onPluginParameterComplete)
        {
        }

        public void RegisterForGetPluginSlots(Action<CubasePluginSlotCollection> onPluginSlot)
        {
        }

        public void RegisterForGetPluginSlotsComplete(Action onPluginSlotComplete)
        {
        }

        public void RegisterForTracksComplete(Action<TrackCollection> onTracksComplete)
        {
        }

        public void RegisterForTransportMonitoring(Action<bool> onMonitoringChanged)
        {
        }

        public void RegisterForUpdateCueLevelsEndCallbacks(Action onUpdateCueLevelsEnd)
        {
        }

        public void ReloadScripts()
        {
        }

        public void RestartMidi()
        {
        }

        public void ResumeCueChecking()
        {
        }

        public bool SendMidiMessage(CubaseKeyCommand cubaseMidiCommand, Action<string> onError)
        {
            return true;
        }

        public void SendSysExMessage<T>(MidiCommand command, T request)
        {
        }

        public void StartTransportMonitoring()
        {
        }

        public void StopTransportMonitoring()
        {
        }

        public void SuspendCueChecking()
        {
        }

        public void VerifyDriver()
        {
        }
    }
}
