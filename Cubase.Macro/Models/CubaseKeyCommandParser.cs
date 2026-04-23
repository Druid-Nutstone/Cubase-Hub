using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Cubase.Macro.Models
{

    public class CubaseKeyCommandParser
    {

        public CubaseKeyCommandCollection Parse()
        {
            var filePath = CubaseMacroConstants.KeyCommandsFileLocation;
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"{filePath} Cubase 15 key commands XML file not found.");
            }
            var doc = XDocument.Load(filePath);
            var list = new CubaseKeyCommandCollection();

            foreach (var element in doc.Root.Elements())
            {
                if (element.Attribute("name")?.Value == "Categories")
                {
                    ProcessCatgeories(element, list);
                }
            }
            AddMidiCommands(list);
            return list;
        }

        private void AddMidiCommands(CubaseKeyCommandCollection commands)
        {
            commands.Add(CubaseKeyCommand.CreateMidi("Start", 2, 0));
            commands.Add(CubaseKeyCommand.CreateMidi("Show Guitars", 0, 2));
            commands.Add(CubaseKeyCommand.CreateMidi("Show Keyboards", 1, 2));
            commands.Add(CubaseKeyCommand.CreateMidi("Show Bass", 2, 2));
            commands.Add(CubaseKeyCommand.CreateMidi("Show Drums", 3, 2));
            commands.Add(CubaseKeyCommand.CreateMidi("Show Vocals", 4, 2));
            commands.Add(CubaseKeyCommand.CreateMidi("Start Recording", 5, 2));
            commands.Add(CubaseKeyCommand.CreateMidi("Stop Recording", 6, 2));
            commands.Add(CubaseKeyCommand.CreateMidi("Show All Tracks", 11, 0));
            commands.Add(CubaseKeyCommand.CreateMidi("Expand Folders", 23, 1));
            commands.Add(CubaseKeyCommand.CreateMidi("Collapse Folders", 16, 0));
            commands.Add(CubaseKeyCommand.CreateMidi("Zoom Selected Track", 13, 0));
            commands.Add(CubaseKeyCommand.CreateMidi("UnZoom Selected Track", 7, 2));
            commands.Add(CubaseKeyCommand.CreateMidi("Undo Record", 8, 1));
            commands.Add(CubaseKeyCommand.CreateMidi("Show Tracks With Data", 10, 0));
            commands.Add(CubaseKeyCommand.CreateMidi("New Track Version", 9, 1));
            commands.Add(CubaseKeyCommand.CreateMidi("Show Selected", 12, 0));
        }

        /*
            this.Add(CubaseMidiCommand.Create("Show Selected Tracks", 0, 12, "Channel & Track Visibility", "ShowSelected", 127));         
         */

        private void ProcessCatgeories(XElement catElement, CubaseKeyCommandCollection commands)
        {
            foreach (var element in catElement.Elements())
            {
                if (element.Name.LocalName.ToLower() == "item")
                {
                    ProcessCatItem(element, commands);
                }
            }
        }

        private void ProcessCatItem(XElement catItemElement, CubaseKeyCommandCollection commands)
        {
            var categoryName = "";
            foreach (var element in catItemElement.Elements())
            {
                if (element.Name.LocalName.ToLower() == "string")
                {
                    // category name
                    categoryName = element.Attribute("value")?.Value ?? "Unknown";
                }
                if (element.Name.LocalName.ToLower() == "list")
                {
                    // commands list
                    foreach (var cmdItem in element.Elements("item"))
                    {
                        ProcessCommandItem(cmdItem, categoryName, commands);
                    }
                }
            }
        }

        private void ProcessCommandItem(XElement cmdItemElement, string categoryName, CubaseKeyCommandCollection commands)
        {
            string name = "";
            string key = "";
            string action = "";

            foreach (var element in cmdItemElement.Elements("string"))
            {
                var elName = element.Attribute("name")?.Value?.ToLowerInvariant();
                switch (elName)
                {
                    case "name":
                        name = element.Attribute("value")?.Value ?? "";
                        break;
                    case "key":
                        key = element.Attribute("value")?.Value ?? "";
                        break;
                }
            }

            foreach (var list in cmdItemElement.Elements("list"))
            {
                var keys = list.Descendants("item")
                    .Select(i => i.Attribute("value")?.Value)
                    .Where(v => !string.IsNullOrEmpty(v))
                    .ToList();
                if (keys.Any())
                    key = string.Join(", ", keys);
            }
            // only add if key is assigned
            if (!string.IsNullOrEmpty(key))
            {
                commands.Add(new CubaseKeyCommand()
                {
                    Category = categoryName,
                    Name = name,
                    Key = key,
                });
            }
        }

    }
}

