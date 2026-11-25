namespace Cubase.Macro.Tests.MacroLanguageTests
{
    [TestClass]
    public class lexTests : BaseMidiTest
    {

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
