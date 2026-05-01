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
    
        public static string MidiConfigurationFileName = Path.Combine(UserAppDataFolderPath, "CubaseMidiMacroConfig.json");

        public static string MidiJavascriptFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Steinberg", "Cubase", "MIDI Remote", "Driver Scripts", "Local", "Nutstone", "VirtualDevice", "Nutstone_VirtualDevice.js");

        // C:\Users\david\OneDrive\Documents\Steinberg\Cubase\User Presets\Project Logical Editor

        public static string PleDirectoryMacroLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Steinberg", "Cubase", "User Presets", "Project Logical Editor");


    }
}
