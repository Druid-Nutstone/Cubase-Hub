using Cubase.Macro.Common.Models;
using Cubase.Macro.Services.Midi;
using Microsoft.Extensions.Logging;
using Nutstone.AI.Assistant.LocalModels.PluginOutputDefinitions;

namespace Cubase.Macro.Services.Plugins
{
    public class PluginManagerService : IPLuginManagerService
    {
        private readonly IMidiService midiService;

        private readonly ILogger<PluginManagerService> logger;

        public PluginManagerService(IMidiService midiService, ILogger<PluginManagerService> logger)
        {
            this.midiService = midiService;
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

        public CubasePluginSlotCollection GetCubasePluginSlots()
        {
            using var responseReceived = new ManualResetEventSlim(false);

            CubasePluginSlotCollection slots = null;

            void OnCubaseSlotsReturned(
                CubasePluginSlotCollection cubasePluginSlots)
            {
                slots = cubasePluginSlots;

                this.midiService.DeRegisterForGetPluginSlots(
                    OnCubaseSlotsReturned);

                responseReceived.Set();
            }

            this.midiService.RegisterForGetPluginSlots(OnCubaseSlotsReturned);



            try
            {
                this.midiService.SendSysExMessage(
                    Models.MidiCommand.GetPluginSlots, "{}");

                if (!responseReceived.Wait(TimeSpan.FromSeconds(30)))
                {
                    this.midiService.DeRegisterForGetPluginSlots(
                        OnCubaseSlotsReturned);

                    throw new TimeoutException(
                        "Timed out waiting for Cubase plugin slots.");
                }

                return slots;
            }
            catch
            {
                this.midiService.DeRegisterForGetPluginSlots(
                    OnCubaseSlotsReturned);

                throw;
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
            this.midiService.SendSysExMessage<CubaseInsertPlugin>(Models.MidiCommand.InsertPlugin, cubaseInsertPlugin);
            return true;
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

        public bool UpdatePluginParameter(UpdateCubasePluginParameter parameter)
        {
            this.midiService.SendSysExMessage<UpdateCubasePluginParameter>(Models.MidiCommand.UpdatePluginParameter, parameter);
            return true;
        }
    }
}
