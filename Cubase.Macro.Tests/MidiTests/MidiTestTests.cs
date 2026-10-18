namespace Cubase.Macro.Tests.MidiTests
{
    [TestClass]
    public class MidiTestTests : BaseMidiTest
    {
        public MidiTestTests()
        {

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
            this.midiService.Initialise();
            Thread.Sleep(5000);
            var pluginSlots = this.pluginManagerService.GetCubasePluginSlots();
            this.midiService.Dispose();
        }

        [TestMethod]
        public void Can_Get_Plugin_Parameters()
        {
            this.midiService.Initialise();
            Thread.Sleep(5000);
            var pluginSlots = this.pluginManagerService.GetCubasePluginSlots();
            if (pluginSlots.PluginExists("frequency"))
            {
                var parameters = this.pluginManagerService.GetPluginParameters(pluginSlots.GetPluginByName("frequency"));
                parameters.SaveToJson("C:\\deleteme\\frequencyparameters.json");
            }
            this.midiService.Dispose();
        }
    }
}
