using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Cubase.Macro
{
    public static class CubaseMacroConstants
    {
        public static string UserAppDataFolderName = "CubaseHub";

        public static string CubaseMacroLog = "CubaseMacroLog-";

        public static string UserAppDataFolderPath =
            System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                UserAppDataFolderName);

        public static string LogPath = Path.Combine(UserAppDataFolderPath, "Logs");

        public static string ConfigurationFileName = Path.Combine(UserAppDataFolderPath, "CubaseMacro.json");
    }
}
