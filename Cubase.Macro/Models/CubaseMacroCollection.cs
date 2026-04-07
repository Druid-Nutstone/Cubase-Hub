using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Cubase.Macro.Models
{
    public class CubaseMacroCollection : List<CubaseMacro>
    {
        
        public void Save()
        {
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions() { WriteIndented = true });
            File.WriteAllText(CubaseMacroConstants.ConfigurationFileName, json);
        } 
        
        public static CubaseMacroCollection Load()
        {
            if (File.Exists(CubaseMacroConstants.ConfigurationFileName))
            {
                return JsonSerializer.Deserialize<CubaseMacroCollection>(File.ReadAllText(CubaseMacroConstants.ConfigurationFileName));  
            }
            else
            {
                return new CubaseMacroCollection();
            }
        }
    }


    public class CubaseMacro
    {
        public string Title { get; set; }

        public CubaseMacroType MacroType { get; set; }

        public List<CubaseMacro> Macros{ get; set; } = new List<CubaseMacro>();

        public static CubaseMacro CreateNewMenuMacro()
        {
            return new CubaseMacro()
            {
                MacroType = CubaseMacroType.Menu,
            };
        }

        public static CubaseMacro CreateMenuMacro(string title, List<CubaseMacro> macros)
        {
            return new CubaseMacro()
            {
                Title = title,
                MacroType = CubaseMacroType.Menu,
                Macros = macros
            };
        }

        public static CubaseMacro CreateKeyCommandMacro(string title)
        {
            return new CubaseMacro()
            {
                Title = title,
                MacroType = CubaseMacroType.KeyCommand
            };
        }

    }

    public enum CubaseMacroType
    {
        Menu = 0,
        KeyCommand = 1,
    }
}
