using Cubase.Macro.Forms;
using Microsoft.Windows.Themes;
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

        public CubaseMacro FindParentIdRecursive(CubaseMacro macro, Guid id)
        {
            if (macro.Id == id)
                return macro;

            if (macro.Macros != null)
            {
                foreach (var child in macro.Macros)
                {
                    var found = FindParentIdRecursive(child, id);
                    if (found != null)
                        return found;
                }
            }

            return null;
        }
    }

    public class CubaseMacro
    {
        public string Title { get; set; }

        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid? ParentId { get; set; }

        public string TitleToggle { get; set; }
        
        public bool ReturnToParentMenuAfterExecution { get; set; } = false;

        public CubaseMacroType MacroType { get; set; }

        public int BackgroundColourARGB { get; set; } = DarkTheme.BackColor.ToArgb();

        public int ForegroundColourARGB { get; set; } = DarkTheme.TextColor.ToArgb();

        public int ToggleBackgroundColourARGB { get; set; } = DarkTheme.BackColor.ToArgb();

        public int ToggleForegroundColourARGB { get; set; } = DarkTheme.TextColor.ToArgb();

        public CubaseMacroButtonType ButtonType { get; set; } = CubaseMacroButtonType.NotApplicable;

        public CubaseMacroToggleState ToggleState { get; set; } = CubaseMacroToggleState.NotApplicable;    

        public List<CubaseKeyCommand> ToggleOnKeys { get; set; } = new List<CubaseKeyCommand>();  

        public List<CubaseKeyCommand> ToggleOffKeys { get; set; } = new List<CubaseKeyCommand>();   

        public List<CubaseMacro> Macros{ get; set; } = new List<CubaseMacro>();

        public static CubaseMacro CreateNewMenuMacro(Guid? parentId = null)
        {
            return new CubaseMacro()
            {
                MacroType = CubaseMacroType.Menu,
                ParentId = parentId,
            };
        }

        public static CubaseMacro CreateMenuMacro(string title, List<CubaseMacro> macros, Guid? parentId = null)
        {
            return new CubaseMacro()
            {
                Title = title,
                MacroType = CubaseMacroType.Menu,
                ParentId = parentId,
                Macros = macros
            };
        }

        public static CubaseMacro CreateKeyCommandMacro(string title, Guid? parentid = null)
        {
            return new CubaseMacro()
            {
                Title = title,
                MacroType = CubaseMacroType.KeyCommand,
                ParentId = parentid
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
