using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Cubase.Macro.Models
{
    public class CubaseMacroConfiguration
    {

        public string ResetVisibilityKey { get; set; }


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
