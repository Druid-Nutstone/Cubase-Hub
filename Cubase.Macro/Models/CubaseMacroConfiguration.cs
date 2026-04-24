using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Cubase.Macro.Models
{
    public class CubaseMacroConfiguration
    {


        public int MacroPanelHeight { get; set; } = -1;
        
        public string ResetVisibilityKey { get; set; }

        public int MenuHeight { get; set; } = 80;

        public int KeyHeight { get; set; } = 120;

        public string CubaseExecutable { get; set; } = "Cubase15";

        public string CubaseProjectWindowName { get; set; } = "Cubase Pro Project";

        public bool ReloadWindowsMidiService { get; set; } = false;

        public void Save()
        {
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions() { WriteIndented = true });
            File.WriteAllText(CubaseMacroConstants.ConfigurationFileName, json);
        }
        
        public static CubaseMacroConfiguration Load()
        {
            if (File.Exists(CubaseMacroConstants.ConfigurationFileName))
            {
                return JsonSerializer.Deserialize<CubaseMacroConfiguration>(File.ReadAllText(CubaseMacroConstants.ConfigurationFileName));
            }
            else
            {
                return new CubaseMacroConfiguration();
            }
        }
    }
}
