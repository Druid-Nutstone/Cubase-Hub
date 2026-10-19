using Cubase.Macro.Common.Models;
using Nutstone.AI.Assistant.LocalModels.PluginOutputDefinitions;
using Nutstone.AI.Assistant.Services.Plugins;

namespace Cubase.Macro.Tests.MidiTests
{
    [TestClass]
    public class MidiTestTests : BaseMidiTest
    {
        public MidiTestTests() : base()
        {
            if (!this.midiService.Initialised)
            {
                var ready = new TaskCompletionSource(
                    TaskCreationOptions.RunContinuationsAsynchronously);

                this.midiService.OnReadyReceived = () =>
                {
                    ready.TrySetResult();
                };

                this.midiService.Initialise();

                ready.Task.Wait();
            }

        }

        [TestCleanup]
        public void TearDown()
        {
            this.midiService.Dispose();
        }
        [TestMethod]
        /// more of a system test NOTE the fix for frequency2
        public void Can_Map_Ai_Cubase_Plugin_To_Cubase_Plugin()
        {
            var aipluginService = new PluginService();

            var index = aipluginService.GetPluginIndex();

            var pluginIndex = index.FirstOrDefault(x => x.Question.Contains("Kiss", StringComparison.OrdinalIgnoreCase));
            if (pluginIndex != null)
            {
                var aiPluginObjects = aipluginService.GetPluginObjectsFromIndex(pluginIndex);
                foreach (var aiPlugin in aiPluginObjects)
                {
                    var asRootPlugin = (RootPluginOutput)aiPlugin;
                    var pluginName = asRootPlugin.PluginName.ToLower() == "frequency2" ? "frequency" : asRootPlugin.PluginName;
                    var pluginSlots = this.pluginManagerService.GetCubasePluginSlots();
                    if (pluginSlots.PluginExists(pluginName))
                    {
                        var pluginSlot = pluginSlots.GetPluginByName(pluginName);
                        var parameters = this.pluginManagerService.GetPluginParameters(pluginSlots.GetPluginByName(pluginName));
                        var updatedParameters = this.pluginManagerService.MapAIPluginToCubasePlugin(parameters, asRootPlugin);
                        foreach (var parameter in updatedParameters)
                        {
                            var updateableParameter = UpdateCubasePluginParameter.CreateFromCubasePluginParameter(pluginSlot.PluginID, parameter);
                            this.pluginManagerService.UpdatePluginParameter(updateableParameter);
                        }
                    }
                }
            }
        }



        [TestMethod]
        public void Can_Start_Midi()
        {
            this.midiService.Initialise();
            Assert.IsTrue(this.midiService.Initialised);
        }

        [TestMethod]
        public void Can_Get_Existing_Plugins_ForTrack()
        {
            var pluginSlots = this.pluginManagerService.GetCubasePluginSlots();
            this.midiService.Dispose();
        }

        [TestMethod]
        public void Can_Get_Plugin_Parameters()
        {
            var pluginName = "frequency";

            var pluginSlots = this.pluginManagerService.GetCubasePluginSlots();
            if (pluginSlots.PluginExists(pluginName))
            {
                var parameters = this.pluginManagerService.GetPluginParameters(pluginSlots.GetPluginByName(pluginName));
                var targetDir = "C:\\Deleteme\\Cubase_Plugin_Parameters";
                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }

                parameters.SaveToJson(Path.Combine(targetDir, $"{pluginName}Parameters.json"));
            }
        }

        [TestMethod]
        public void Can_Insert_A_Plugin()
        {
            var pluginName = "multibandcompressor";
            var pluginSlots = this.pluginManagerService.GetCubasePluginSlots();
            var pluginDetails = this.pluginManagerService.GetAvailablePluginDetail();
            if (pluginDetails != null)
            {
                // only add if it does NOT already exist ?
                if (pluginSlots.GetPluginByName(pluginName) == null)
                {
                    // plugin details
                    var pluginDetail = pluginDetails.GetPluginByName(pluginName);
                    // into the first free slot 
                    if (pluginSlots.GetFirstFreeSlot().HasValue)
                    {
                        this.pluginManagerService.InsertPluginIntoCurrentTrack(CubaseInsertPlugin.CreateFromPluginDetail(pluginSlots.GetFirstFreeSlot().Value, pluginDetail));
                    }
                }
            }
        }

        [TestMethod]
        public void Can_Set_A_Plugin_Parameter()
        {
            int pluginID = -1;
            var parameters = this.GetPluginParameters("frequency", out pluginID);
            if (parameters != null)
            {
                var currentParameter = parameters.GetParameterByName("EQ1Freq");
                if (currentParameter != null)
                {
                    currentParameter.DisplayValue = "80";
                    var updateableParameter = UpdateCubasePluginParameter.CreateFromCubasePluginParameter(pluginID, currentParameter);
                    this.midiService.SendSysExMessage<UpdateCubasePluginParameter>(Models.MidiCommand.UpdatePluginParameter, updateableParameter);
                }
            }
        }

        private CubasePluginParameterCollection? GetPluginParameters(string pluginName, out int pluginID)
        {
            var pluginSlots = this.pluginManagerService.GetCubasePluginSlots();
            pluginID = -1;
            if (pluginSlots.PluginExists(pluginName))
            {
                var pluginSlot = pluginSlots.GetPluginByName(pluginName);
                pluginID = pluginSlot.PluginID;
                return this.pluginManagerService.GetPluginParameters(pluginSlot);

            }
            return null;
        }
    }
}
