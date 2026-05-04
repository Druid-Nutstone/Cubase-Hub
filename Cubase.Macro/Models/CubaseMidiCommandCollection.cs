using Cubase.Macro.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Cubase.Macro.Models
{
    
    public class CubaseMidiCommandCollection : List<CubaseMidiCommand>
    {
        public static CubaseMidiCommandCollection Load()
        {
            if (File.Exists(CubaseMacroConstants.MidiConfigurationFileName))
            {
                return JsonSerializer.Deserialize<CubaseMidiCommandCollection>(File.ReadAllText(CubaseMacroConstants.MidiConfigurationFileName));
            }
            else
            {
                return new CubaseMidiCommandCollection();
            }
        }

        public static CubaseMidiCommandCollection LoadFromJavascript()
        {
            var midiCommands = new CubaseMidiCommandCollection();
            if (File.Exists(CubaseMacroConstants.MidiJavascriptFileName))
            {
                var jsLines = File.ReadAllLines(CubaseMacroConstants.MidiJavascriptFileName);
                foreach (var line in jsLines)
                {
                    var match = Regex.Match(
                        line,
                        @"name:\s*""(?<name>[^""]+)""\s*,\s*channel:\s*(?<channel>\d+)\s*,\s*note:\s*(?<note>\d+)",
                        RegexOptions.Singleline
                    );

                    if (match.Success)
                    {
                        string name = match.Groups["name"].Value;
                        int channel = int.Parse(match.Groups["channel"].Value);
                        int note = int.Parse(match.Groups["note"].Value);

                        midiCommands.Add(new CubaseMidiCommand
                        {
                            Command = name,
                            Channel = channel,
                            Note = note
                        });
                    }
                }
            }
            return midiCommands;

        }
    }

    public class CubaseMidiCommand
    {
        public string Command { get; set; } 

        public int Note { get; set; } = 0;

        public int Channel { get; set; } = 0;
    }
}
