using Cubase.Macro.Common.Models;
using Nutstone.AI.Assistant.LocalModels.PluginOutputDefinitions;

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
        public void Test_Plugin_Mapping_Parameters()
        {
            var pluginDescription = "RoomWorks Test";
            var pluginList = this.pluginService.GetPluginPresetsByDescription(pluginDescription);
            var plugin = pluginList.First().Plugin as RootPluginOutput;

            var result = this.pluginManagerService.UpdateCubasePlugin(plugin, (err) => { }, (warning) => { });
        }

        [TestMethod]
        public void Remove_Plugins_From_Selected_Track()
        {

            var pluginSlots = this.pluginManagerService.GetCubasePluginSlots();
            foreach (var plugin in pluginSlots.GetAllPlugins())

            {
                this.pluginManagerService.RemovePlugin(plugin);
            }
        }

        [TestMethod]
        public void Find_Mismatches_In_Plugin_Names()
        {
            var plugins = this.pluginService.GetKnownPluginNames();

            var cubasePlugins = this.pluginManagerService
                .GetAvailablePluginDetail()
                .Select(x => x.PluginName)
                .ToList();

            var mismatches = plugins
                .Where(plugin => !cubasePlugins.Contains(
                    plugin, StringComparer.OrdinalIgnoreCase))
                .Select(plugin => new
                {
                    CurrentName = plugin,
                    ShouldBe = cubasePlugins.FirstOrDefault(cubaseName =>
                        cubaseName.Contains(plugin, StringComparison.OrdinalIgnoreCase)
                        || plugin.Contains(cubaseName, StringComparison.OrdinalIgnoreCase))
                })
                .ToList();
        }

        [TestMethod]
        public void Create_Test_Plugins_In_Cubase()
        {
            // remove existing plugins;
            this.Remove_Plugins_From_Selected_Track();
            // get available plugins that are supported 
            var plugins = this.pluginService.GetKnownPluginNames();
            // get all cubase plugins 
            var knownPlugins = this.pluginManagerService.GetAvailablePluginDetail();
            // get a list of plugins (up to 16 slots)
            var pluginsToInsert = knownPlugins
                .Where(x => plugins.Any(p =>
                    string.Equals(p, x.PluginName, StringComparison.OrdinalIgnoreCase)))
                .Take(16)
                .ToList();
            foreach (var plugin in pluginsToInsert)
            {
                // get slots
                var slots = this.pluginManagerService.GetCubasePluginSlots();
                var firstFreeSlot = slots.GetFirstFreeSlot();
                this.pluginManagerService.InsertPluginIntoCurrentTrack(CubaseInsertPlugin.CreateFromPluginDetail(firstFreeSlot.Value, plugin));
            }
        }

        [TestMethod]
        /// more of a system test NOTE the fix for frequency2
        public void Can_Map_Ai_Cubase_Plugin_To_Cubase_Plugin()
        {

            var index = this.pluginService.GetPluginIndex();

            var pluginIndex = index.FirstOrDefault(x => x.Question.Contains("Kiss", StringComparison.OrdinalIgnoreCase));
            if (pluginIndex != null)
            {
                var aiPluginObjects = this.pluginService.GetPluginObjectsFromIndex(pluginIndex);
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
            var pluginName = "RoomWorks";

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
        public void Can_Generate_Saved_Parameters()
        {
            var pluginSlots = this.pluginManagerService.GetCubasePluginSlots();
            foreach (var plugin in pluginSlots)
            {
                var parameters = this.pluginManagerService.GetPluginParameters(pluginSlots.GetPluginByName(plugin.PluginName));
                var targetDir = "C:\\Deleteme\\Cubase_Plugin_Parameters";
                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }

                parameters.SaveToJson(Path.Combine(targetDir, $"{plugin.PluginName}Parameters.json"));
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
