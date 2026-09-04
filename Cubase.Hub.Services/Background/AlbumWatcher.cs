using Cubase.Hub.Services.Models;

namespace Cubase.Hub.Services.Background
{
    public class AlbumWatcher
    {
        public string FileName { get; set; }

        public AlbumConfiguration Album { get; set; }

        public MixDown MixDown { get; private set; }


        public void SetMixDown(MixDown mixDown)
        {
            this.MixDown = mixDown;
        }
    }
}
