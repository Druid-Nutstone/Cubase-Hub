using Cubase.Macro.Common.Socket;
using Cubase.Macro.Mobile.Lyrics;
using Cubase.Macro.Mobile.Nav;

namespace Cubase.Macro.Mobile
{
    public partial class AppShell : Shell
    {

        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("LyricViewer", typeof(LyricViewer));
            Routing.RegisterRoute("MainPage", typeof(MainPage));
            Routing.RegisterRoute("NavPage", typeof(NavPage));
        }
    }
}
