using Cubase.Macro.Common.Models;
using Cubase.Macro.Services.Midi;
using Microsoft.Extensions.Logging;
using Nutstone.AI.Assistant.LocalModels.PluginOutputDefinitions;
using Nutstone.AI.Assistant.Models;
using Nutstone.AI.Assistant.Services.Plugins;

namespace Cubase.Macro.Services.Plugins
{
    public class PluginManagerService : IPLuginManagerService
    {
        private readonly IMidiService midiService;

        private readonly IPluginService pluginService;

        private readonly ILogger<PluginManagerService> logger;

        public PluginManagerService(IMidiService midiService,
                                    IPluginService pluginService,
                                    ILogger<PluginManagerService> logger)
        {
            this.midiService = midiService;
            this.pluginService = pluginService;
            this.logger = logger;
        }

        public CubasePluginDetailCollection GetAvailablePluginDetail()
        {
            using var responseReceived = new ManualResetEventSlim(false);


            var details = new CubasePluginDetailCollection();

            void OnCubasePluginDetailsReturned(CubasePluginDetailCollection pluginDetails)
            {
                details.AddFromOtherCollection(pluginDetails);
            }

            void OnCubasePluginDetailsCompleteReturned()
            {
                responseReceived.Set();
            }

            this.midiService.RegisterForGetPluginDetail(OnCubasePluginDetailsReturned);
            this.midiService.RegisterForGetPluginDetailsComplete(OnCubasePluginDetailsCompleteReturned);

            try
            {
                this.midiService.SendSysExMessage(Models.MidiCommand.GetAvailablePlugins, "{}");
                if (!responseReceived.Wait(TimeSpan.FromSeconds(60)))
                {
                    throw new TimeoutException(
                        "Timed out waiting for Cubase plugin parameters to complete.");
                }
                return details;
            }
            finally
            {
                this.midiService.DeRegisterForGetPluginDetail(OnCubasePluginDetailsReturned);
                this.midiService.DeRegisterForGetPluginDetailsComplete(OnCubasePluginDetailsCompleteReturned);
            }
        }

        public Nutstone.AI.Assistant.Models.ChatRequest? GetChatRequest(Action<string> onError)
        {

            var selectedTracks = this.midiService.TrackCollection.GetActiveTracks();

            if (selectedTracks.Count > 1)
            {
                onError.Invoke($"There are multiple tracks selected {string.Join(",", selectedTracks.Select(x => x.Name))}");
                return null;
            }

            if (selectedTracks.Count == 0)
            {
                onError.Invoke("Cannot find any selected tracks!");
                return null;
            }

            // get existing plugins 
            var plugins = this.GetCubasePluginSlots();
            return new Nutstone.AI.Assistant.Models.ChatRequest()
            {
                SelectedTrackName = selectedTracks.First().Name,
                SelectedTrackExistingPlugins =
                      this.pluginService.GetPluginNamesFromCubasePluginNames(plugins.GetActivePluginList())
            };
        }

        public TrackCollection? GetTracks()
        {
            using var responseReceived = new ManualResetEventSlim(false);

            TrackCollection tracks = null;

            void OnTracksCompleteReturned(TrackCollection tracksReceived)
            {
                tracks = tracksReceived;
                responseReceived.Set();
            }

            this.midiService.RegisterForTracksComplete(OnTracksCompleteReturned);
            try
            {
                this.midiService.GetTracks();
                if (!responseReceived.Wait(TimeSpan.FromSeconds(60)))
                {
                    throw new TimeoutException(
                        "Timed out waiting for Cubase plugin parameters to complete.");
                }
                return tracks;
            }
            finally
            {
                this.midiService.DeRegisterForTracksComplete(OnTracksCompleteReturned);
            }
        }

        public CubasePluginSlotCollection GetCubasePluginSlots()
        {
            using var responseReceived = new ManualResetEventSlim(false);

            var slots = new CubasePluginSlotCollection();

            void OnCubaseSlotsReturned(
                CubasePluginSlotCollection pluginParameters)
            {
                slots.AddFromOtherCollection(pluginParameters);
            }

            void OnCubaseSlotsCompleteReturned()
            {
                responseReceived.Set();
            }

            this.midiService.RegisterForGetPluginSlots(OnCubaseSlotsReturned);

            this.midiService.RegisterForGetPluginSlotsComplete(OnCubaseSlotsCompleteReturned);

            try
            {
                this.midiService.SendSysExMessage(Models.MidiCommand.GetPluginSlots, "{}");

                if (!responseReceived.Wait(TimeSpan.FromSeconds(60)))
                {
                    throw new TimeoutException(
                        "Timed out waiting for Cubase plugin parameters to complete.");
                }

                return slots;
            }
            finally
            {
                this.midiService.DeRegisterForGetPluginSlots(OnCubaseSlotsReturned);
                this.midiService.DeRegisterForGetPluginSlotsComplete(OnCubaseSlotsCompleteReturned);
            }
        }


        public CubasePluginParameterCollection GetPluginParameters(
            CubasePluginSlot cubasePluginSlot)
        {
            using var responseReceived = new ManualResetEventSlim(false);

            var parameters = new CubasePluginParameterCollection();

            void OnCubaseParametersReturned(
                CubasePluginParameterCollection pluginParameters)
            {
                parameters.AddFromOtherCollection(pluginParameters);
            }

            void OnCubaseParametersCompleteReturned()
            {
                responseReceived.Set();
            }

            this.midiService.RegisterForGetPluginParameters(
                OnCubaseParametersReturned);

            this.midiService.RegisterForGetPluginParametersComplete(
                OnCubaseParametersCompleteReturned);

            try
            {
                this.midiService.SendSysExMessage<CubasePluginSlot>(
                    Models.MidiCommand.GetPluginParameters,
                    cubasePluginSlot);

                if (!responseReceived.Wait(TimeSpan.FromSeconds(60)))
                {
                    throw new TimeoutException(
                        "Timed out waiting for Cubase plugin parameters to complete.");
                }

                return parameters;
            }
            finally
            {
                this.midiService.DeRegisterForGetPluginParameters(
                    OnCubaseParametersReturned);

                this.midiService.DeRegisterForGetPluginParametersComplete(
                    OnCubaseParametersCompleteReturned);
            }
        }

        public bool InsertPluginIntoCurrentTrack(CubaseInsertPlugin cubaseInsertPlugin)
        {
            using var completion = new ManualResetEventSlim(false);

            bool result = false;

            this.midiService.OnCommandComplete = (success) =>
            {
                result = success;
                completion.Set();
            };

            try
            {
                this.midiService.SendSysExMessage<CubaseInsertPlugin>(
                    Models.MidiCommand.InsertPlugin,
                    cubaseInsertPlugin);

                if (!completion.Wait(TimeSpan.FromSeconds(30)))
                {
                    throw new TimeoutException(
                        "Timed out waiting for Cubase to complete InsertPlugin.");
                }

                return result;
            }
            finally
            {
                this.midiService.OnCommandComplete = null;
            }
        }

        public List<CubasePluginParameter> MapAIPluginToCubasePlugin(CubasePluginParameterCollection availableParameters, RootPluginOutput aiPlugin)
        {
            var parameters = new List<CubasePluginParameter>();
            foreach (var cubasePluginParameter in availableParameters)
            {
                var populatedPlugin = aiPlugin.PopulateCubasePlugin(cubasePluginParameter);
                if (populatedPlugin != null)
                {
                    parameters.Add(populatedPlugin);
                }
            }
            return parameters;
        }

        public bool RemovePlugin(CubasePluginSlot cubasePlugin)
        {
            using var completion = new ManualResetEventSlim(false);

            bool result = false;

            this.midiService.OnCommandComplete = (success) =>
            {
                result = success;
                completion.Set();
            };

            try
            {
                this.midiService.SendSysExMessage<CubasePluginSlot>(
                    Models.MidiCommand.RemovePlugin,
                    cubasePlugin);

                if (!completion.Wait(TimeSpan.FromSeconds(30)))
                {
                    throw new TimeoutException(
                        "Timed out waiting for Cubase to complete RemovePlugin.");
                }

                return result;
            }
            finally
            {
                this.midiService.OnCommandComplete = null;
            }
        }

        public bool UpdateCubasePlugin(RootPluginOutput rootPlugin, Action<string> onError, Action<string> onWarning)
        {
            try
            {
                if (!rootPlugin.CanAutomate)
                {
                    onWarning($"root Plugin {rootPlugin.PluginName} cannot (currently) be automated");
                    return false;
                }
                // get the real name of the plugin - incase it is different 
                var pluginName = this.pluginService.GetCubasePluginName(rootPlugin.PluginName);
                // get all the plugin slots that are in the current track 
                var pluginSlots = this.GetCubasePluginSlots();
                var pluginSlot = pluginSlots.GetPluginByName(pluginName);
                if (pluginSlot == null)
                {
                    onError.Invoke($"There is no plugin {pluginName} in the current tracks plugin inserts");
                    return false;
                }
                // get the current parameters for the plugin 
                var pluginParameters = this.GetPluginParameters(pluginSlots.GetPluginByName(pluginName));
                // call the rootplugin to map update parameters 
                var updatedPluginParameters = this.MapAIPluginToCubasePlugin(pluginParameters, rootPlugin);
                // now send each parameter update to the cubase plugin 
                foreach (var parameter in updatedPluginParameters)
                {
                    var updateableParameter = UpdateCubasePluginParameter.CreateFromCubasePluginParameter(pluginSlot.PluginID, parameter);
                    this.UpdatePluginParameter(updateableParameter);
                }
                return true;
            }
            catch (Exception ex)
            {
                onError.Invoke(ex.Message);
                return false;
            }
        }

        public bool UpdatePluginParameter(UpdateCubasePluginParameter parameter)
        {
            using var completion = new ManualResetEventSlim(false);

            bool result = false;

            this.midiService.OnCommandComplete = (success) =>
            {
                result = success;
                completion.Set();
            };

            try
            {
                this.midiService.SendSysExMessage<UpdateCubasePluginParameter>(
                    Models.MidiCommand.UpdatePluginParameter,
                    parameter);

                if (!completion.Wait(TimeSpan.FromSeconds(10)))
                {
                    throw new TimeoutException(
                        "Timed out waiting for Cubase to complete UpdatePluginParameter.");
                }

                return result;
            }
            finally
            {
                this.midiService.OnCommandComplete = null;
            }
        }

        public void ProcessPluginRequest(ChatResult pluginRequest)
        {
            SendProgressMessage("Running Cubase plugin intergeation commands");

            CubasePluginSlotCollection pluginSlots;

            string currentTrackName = null;

            if (!EnsureCubaseIsReady())
            {
                return;
            }

            if (pluginRequest.RemoveExistingPlugins)
            {
                SendProgressMessage($"Removing Existing plugins from track {currentTrackName}");
                pluginSlots = this.GetCubasePluginSlots();
                foreach (var plugin in pluginSlots.GetAllPlugins())
                {
                    SendProgressMessage($"Removing {plugin.PluginName}");
                    this.RemovePlugin(plugin);
                }
            }

            pluginSlots = this.GetCubasePluginSlots();
            var pluginDetail = this.GetAvailablePluginDetail();
            foreach (RootPluginOutput plugin in pluginRequest.Plugins)
            {
                // get the cubase plugin name 
                var cubaseName = this.pluginService.GetCubasePluginName(plugin.PluginName);
                var pluginExists = pluginSlots.GetPluginByName(cubaseName);
                SendProgressMessage($"updating or insertinfg {cubaseName}");
                if (pluginExists == null) // new plugin 
                {
                    this.InsertPluginIntoCurrentTrack(
                        CubaseInsertPlugin.CreateFromPluginDetail(pluginSlots.GetFirstFreeSlot().Value, pluginDetail.GetPluginByName(cubaseName)));
                    pluginSlots = this.GetCubasePluginSlots();
                    this.UpdateCubasePlugin(plugin,
                        SendErrorMessage,
                        SendProgressMessage);
                }
                else
                {
                    this.UpdateCubasePlugin(plugin,
                        SendErrorMessage,
                        SendProgressMessage);
                }
                pluginSlots = this.GetCubasePluginSlots();
            }

            pluginRequest.OnProgress.Invoke(ChatProgress.CreateComplete("Insert/updates are complete"));

            bool EnsureCubaseIsReady()
            {
                SendProgressMessage("Validating Request ..");
                var tracks = this.midiService.TrackCollection;
                if (tracks.GetActiveTracks().Count != 1)
                {
                    SendErrorMessage($"There can only be ONE track selected. the current selected tracks count is {tracks.GetActiveTracks().Count}");
                    return false;
                }
                currentTrackName = tracks.GetActiveTracks().First().Name;
                return true;
            }

            void SendProgressMessage(string message)
            {
                pluginRequest.OnProgress.Invoke(ChatProgress.CreateProgress(message));
            }

            void SendErrorMessage(string message)
            {
                pluginRequest.OnProgress.Invoke(ChatProgress.CreateError(message));
            }
        }
    }
}
