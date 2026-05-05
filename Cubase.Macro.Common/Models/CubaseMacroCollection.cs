using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Cubase.Macro.Common.Models
{
    public class CubaseMacroCollection
    {

        public static CubaseMacroCollection Deserialise(string message)
        {
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(message));
            return JsonSerializer.Deserialize<CubaseMacroCollection>(json);
        }

        public string Serialise()
        {
            var json = JsonSerializer.Serialize(this);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        }

        public List<CubaseMacro> Macros { get; set; } = new();
        public List<CubaseMacro> CommonMacros { get; set; } = new();



        public CubaseRemoteMidiMacroCollection GetMidiRemoteCollection()
        {
            var allMacros =
                this.Macros
                    .Concat(this.CommonMacros ?? new List<CubaseMacro>())
                    .SelectMany(m => m.GetAll())
                    .Where(m => m.IsAvailableForRemote)
                    .ToList();

            return new CubaseRemoteMidiMacroCollection(allMacros);
        }

        public void Save()
        {
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions() { WriteIndented = true });
            File.WriteAllText(CubaseMacroConstants.MacroConfigurationFileName, json);
        } 
        
        public static CubaseMacroCollection Load()
        {
            if (File.Exists(CubaseMacroConstants.MacroConfigurationFileName))
            {
                return JsonSerializer.Deserialize<CubaseMacroCollection>(File.ReadAllText(CubaseMacroConstants.MacroConfigurationFileName));  
            }
            else
            {
                var macroCollection = new CubaseMacroCollection();
                macroCollection.Macros.Add(CubaseMacro.CreatePrimaryMenu());
                return macroCollection;
            }
        }

        
        public CubaseMacro FindParentFromBase(Guid id)
        {
            return this.FindParentIdRecursive(this.Macros?.First(), id);
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

        public bool IsAvailableForRemote { get; set; } = false;

        public int BackgroundColourARGB { get; set; } = Color.FromArgb(32, 32, 32).ToArgb();

        public int ForegroundColourARGB { get; set; } = Color.FromArgb(220, 220, 220).ToArgb();

        public int ToggleBackgroundColourARGB { get; set; } = Color.FromArgb(32, 32, 32).ToArgb();

        public int ToggleForegroundColourARGB { get; set; } = Color.FromArgb(220, 220, 220).ToArgb();

        public CubaseMacroButtonType ButtonType { get; set; } = CubaseMacroButtonType.NotApplicable;

        public CubaseMacroToggleState ToggleState { get; set; } = CubaseMacroToggleState.NotApplicable;    

        public List<CubaseKeyCommand> ToggleOnKeys { get; set; } = new List<CubaseKeyCommand>();  

        public List<CubaseKeyCommand> ToggleOffKeys { get; set; } = new List<CubaseKeyCommand>();   

        public List<CubaseMacro> Macros{ get; set; } = new List<CubaseMacro>();

        public bool MenuChangesVisibility { get; set; } = false;

        public string Serialize()
        {
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions() { WriteIndented = false });
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        } 

        public static CubaseMacro Deserialize(string base64String)
        {
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(base64String));
            return JsonSerializer.Deserialize<CubaseMacro>(json);
        }

        public IEnumerable<CubaseMacro> GetAll()
        {
            yield return this;

            if (Macros == null)
                yield break;

            foreach (var child in Macros)
            {
                foreach (var descendant in child.GetAll())
                {
                    yield return descendant;
                }
            }
        }

        public static CubaseMacro CreateNewMenuMacro(Guid? parentId = null)
        {
            return new CubaseMacro()
            {
                MacroType = CubaseMacroType.Menu,
                ParentId = parentId,
            };
        }

        public static CubaseMacro CreatePrimaryMenu()
        {
            return new CubaseMacro()
            {
                MacroType = CubaseMacroType.Menu,
                Title = "Primary Menu",
                Id = Guid.NewGuid(),
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
