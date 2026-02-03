using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services
{
    public static class CubaseHubConstants
    {
        public static string UserAppDataFolderName = "CubaseHub";

        public static string MixdownDirectory = "MixDown"; 

        public static string UserAppDataFolderPath = 
            System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                UserAppDataFolderName);

        public static string ConfigurationFileName = Path.Combine(UserAppDataFolderPath, "CubaseHub.json");

        public static string CubaseFileExtension = ".cpr";  
    }
}
