using Cubase.Hub.Services.Models;

namespace Cubse.Hub.Tests.Audio.Tests
{
    [TestClass]
    public class AudioTests : BaseTest
    {
        [TestMethod]
        public void Can_Set_PropertyInfo()
        {
            var mixdown = MixDown.CreateFromFile("C:\\Deleteme\\Mixes\\Martin\\BrokenBones.wav");

            this.audioService.AudioPopulateMixdownFromTags(mixdown);

            mixdown.Producer = "David Nuttall";
            mixdown.Engineer = "Druid";
            mixdown.Studio = "Nutstone Studios";

            this.audioService.AudioSetTagsFromMixDowm(mixdown);
        }

    }
}
