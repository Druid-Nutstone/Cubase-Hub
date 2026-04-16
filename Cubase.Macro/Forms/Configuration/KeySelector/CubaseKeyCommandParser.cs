using Cubase.Macro.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Cubase.Macro.Forms.Configuration.KeySelector
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
        }

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

