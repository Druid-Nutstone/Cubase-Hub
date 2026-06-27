using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Cubase.Macro.Common.Models
{
    public class LyricContent
    {
        public string FileName { get; set; }

        public IEnumerable<string> Content { get; set; } = Enumerable.Empty<string>();

        public string Serialise()
        {
            var json = JsonSerializer.Serialize(this);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        }

        public static LyricContent Deserialise(string message)
        {
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(message));
            return JsonSerializer.Deserialize<LyricContent>(json);
        }
    }
}
