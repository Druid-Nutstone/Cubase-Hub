using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Cubase.Macro.Common.Models
{
    public class CubaseRemoteMidiMacroCollection : List<CubaseMacro>
    {
        public CubaseRemoteMidiMacroCollection()
        {

        }

        public CubaseRemoteMidiMacroCollection(IEnumerable<CubaseMacro> items) : base(items) 
        {

        }

        public static CubaseRemoteMidiMacroCollection Deserialise(string message)
        {
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(message));
            return JsonSerializer.Deserialize<CubaseRemoteMidiMacroCollection>(json);
        }

        public string Serialise()
        {
            var json = JsonSerializer.Serialize(this);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        }
    }
}
