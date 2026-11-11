using VoxPresetTools;

namespace Cubase.Macro.Tests.AC50
{
    [TestClass]
    public class TestAC50
    {
        [TestMethod]
        public void CanReadExisting()
        {
            var voxHelper = new VoxPresetManager();
            var preset = voxHelper.ReadBankFile("C:\\Deleteme\\TestVox1.cam50prog");


            voxHelper.PrintPresetDetails(preset.First());

        }

        [TestMethod]
        public void DumpHex()
        {
            var voxHelper = new VoxPresetManager();
            voxHelper.InspectFileHex("C:\\Deleteme\\TestVox1.cam50prog");
        }

        [TestMethod]
        public void MakeExampleJson()
        {
            var voxHelper = new VoxPresetManager();
            voxHelper.ExampleJson("C:\\Deleteme\\voxpreset_example.json");
        }

    }
}
