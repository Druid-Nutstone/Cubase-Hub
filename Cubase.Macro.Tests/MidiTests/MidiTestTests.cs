using Cubase.Macro.Common.Models;

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
            var pluginSlots = this.pluginManagerService.GetCubasePluginSlots();
            if (pluginSlots.PluginExists("frequency"))
            {
                var parameters = this.pluginManagerService.GetPluginParameters(pluginSlots.GetPluginByName("frequency"));
                parameters.SaveToJson("C:\\deleteme\\frequencyparameters.json");
            }
        }

        [TestMethod]
        public void Can_Insert_A_Plugin()
        {

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
