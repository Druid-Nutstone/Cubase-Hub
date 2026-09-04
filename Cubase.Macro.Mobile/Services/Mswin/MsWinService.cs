namespace Cubase.Macro.Mobile.Services.Mswin
{
    public class MsWinService : IMsWinService
    {
        private string? lyricFile = null;

        public string LyricFile => this.lyricFile;

        public bool HaveLyricFile()
        {
            return this.lyricFile != null;
        }

        public bool HaveParameters()
        {
            Microsoft.Maui.Devices.DevicePlatform currentPlatform = DeviceInfo.Current.Platform;
            if (currentPlatform == DevicePlatform.WinUI)
            {
                var args = Environment.GetCommandLineArgs();
                if (args.Length > 1)
                {
                    this.lyricFile = args[1];
                    return true;
                }
            }
            return false;
        }
    }
}
