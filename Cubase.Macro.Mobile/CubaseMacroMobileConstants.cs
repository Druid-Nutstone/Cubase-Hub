using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Mobile
{
    public static class CubaseMacroMobileConstants
    {
        public static string BaseFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CubaseMacro");

        public static string LyricSourceFolder = Path.Combine(BaseFolder, "Lyrics");

        public static string ConfigurationFile = Path.Combine(BaseFolder, "MobileConfiguration.json");

        public static string LyricCollection = Path.Combine(LyricSourceFolder, "LyricIndexCollection.json");

        public static Color DefaultBackgroundColour = Color.FromRgba("#1E1E1E");
#if DEBUG
        public static string TargetIPAddress => "192.168.1.110";
        // Place sensitive logging or diagnostic tools here

#else
       public static string TargetIPAddress => "192.168.4.9"; 
#endif

    }
}
