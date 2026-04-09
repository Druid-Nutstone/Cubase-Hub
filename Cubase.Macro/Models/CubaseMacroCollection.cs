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

        public int BackgroundColourARGB { get; set; } = 0;

        public int ForegroundColourARGB { get; set; } = 0;

        public CubaseMacroButtonType ButtonType { get; set; } = CubaseMacroButtonType.NotApplicable;

        public CubaseMacroToggleState ToggleState { get; set; } = CubaseMacroToggleState.NotApplicable;    

        public List<CubaseKeyCommand> ToggleOnKeys { get; set; } = new List<CubaseKeyCommand>();  

        public List<CubaseKeyCommand> ToggleOffKeys { get; set; } = new List<CubaseKeyCommand>();   

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

    public enum CubaseMacroButtonType
    {
        NotApplicable = 0,
        Single = 1,
        Toggle = 2
    }

    public enum CubaseMacroToggleState
    {
        NotApplicable = 0,
        On = 1,
        Off = 2
    }
}
