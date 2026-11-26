namespace Cubase.Macro.Tests.MacroLanguageTests
{
    [TestClass]
    public class lexTests : BaseMidiTest
    {

        private static bool UseFakes = false;

        public lexTests() : base(UseFakes)
        {
        }

        [TestMethod]
        public void ExecuteMacro()
        {
            this.StartMidiService();

            string script = """
                var result = RecordEnableByname("Groove Agent","Yamaha")
                if result == true then
                   RecordDisableByName("Groove Agent")
                   ExecuteMidi("Start")
                end if  
                """;
            this.cLService.ProcessMacro(script, (error) =>
            {
                Console.WriteLine(error);
            });
        }

        [TestMethod]
        public void Test_SetSelected()
        {
            this.StartMidiService();
            string script = """
                var result = SetSelected("Record",true)
                if (result == true) then 
                    var enablePlay = SetTransport("Play", true)
                end if
                """;
            var tskj = Task.Run(() =>
            {
                var tlist = this.midiService.TrackCollection.GetSelectedTracks();
                while (this.midiService.TrackCollection.GetSelectedTracks().Count() == 0)
                {
                    Thread.Sleep(1000);
                }
            });
            tskj.Wait();
            this.cLService.ProcessMacro(script, (error) =>
            {
                Console.WriteLine(error);
            });
        }

        [TestCleanup]
        public void TearDown()
        {
            this.midiService.Dispose();

        }

        private void StartMidiService()
        {
            var task = Task.Run(() =>
            {
                this.midiService.Initialise();
                while (!this.midiService.Initialised)
                {
                    Thread.Sleep(500);
                }
                while (this.midiService.TrackCollection.Count == 0)
                {
                    Thread.Sleep(1000);
                }
            });
            task.Wait();
        }
    }
}
