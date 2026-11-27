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
                var result = SelectEnableByName("Groove Agent")
                if result == true then
                   // RecordDisableByName("Groove Agent")
                   // ExecuteMidi("Start")
                end if  
                """;
            this.cLService.RunScript(script, (error) =>
            {
                Console.WriteLine(error);
            }, (p) => { });
        }

        [TestMethod]
        public void Test_SetSelected()
        {
            this.StartMidiService();
            string script = """
                // set left locator to cursor position 
                var setLocatorResult = ExecuteMidi("Set Left Locator")
                if (setLocatorResult == true) then 
                   // move back 4 bars 
                   var moveCursorBack = ExecuteMidi("Cursor Back 1 Bar", 4)
                   if (moveCursorBack == true) then
                      // set marker 3 to current position 
                      var setMarker3 = ExecuteMidi("Set Marker 3")
                      if (setMarker3 == true) then 
                        // set any selected tracks into record mode 
                        var result = SetSelected("Record",true)
                        if (result == true) then 
                           // enable punch in 
                           var enablePunchIn = SetTransport("PunchIn", true)
                           if (enablePunchIn == true) then
                              var enablePlay = SetTransport("Play", true) 
                           end if   
                        end if
                      end if
                   end if                      
                end if
                """;
            var tskj = Task.Run(() =>
            {
                var tlist = this.midiService.TrackCollection.GetSelectedTracks();
                while (tlist.Count == 0)
                {
                    Thread.Sleep(1000);
                    tlist = this.midiService.TrackCollection.GetSelectedTracks();
                }
            });
            tskj.Wait();
            this.cLService.RunScript(script, (error) =>
            {
                Console.WriteLine(error);
            }, (progress) => { });
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
