namespace Cubase.Macro.Mobile.Lyrics
{
    public class BottomMenuHandler
    {
        private readonly Grid Container;
        private readonly LyricViewer Viewer;

        public BottomMenuHandler(Grid container, LyricViewer viewer)
        {
            this.Container = container;
            this.Viewer = viewer;
        }
    }
}
