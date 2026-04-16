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

        public static string Midi = nameof(Midi);
        
        public static string UserAppDataFolderPath =
            System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                UserAppDataFolderName);

        public static string LogPath = Path.Combine(UserAppDataFolderPath, "Logs");

        public static string KeyCommandsFileLocation { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Steinberg", "Cubase 15_64", "Key Commands.xml");

        public static string MacroConfigurationFileName = Path.Combine(UserAppDataFolderPath, "CubaseMacro.json");
    
        public static string ConfigurationFileName = Path.Combine(UserAppDataFolderPath, "CubaseMacroConfig.json");
    }
}
