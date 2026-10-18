using System.Text.Json;

namespace Cubase.Macro.Common.Models
{
    public class CubasePluginParameterCollection : List<CubasePluginParameter>
    {
        public void AddFromOtherCollection(CubasePluginParameterCollection cubasePluginParameters)
        {
            this.AddRange(cubasePluginParameters);
        }

        public void SaveToJson(string fileName)
        {
            var asText = JsonSerializer.Serialize(this, new JsonSerializerOptions() { WriteIndented = true });
            File.WriteAllText(fileName, asText);
        }
    }

    public class CubasePluginParameter
    {
        public string Name { get; set; }

        public int Tag { get; set; }

        public string DisplayValue { get; set; }

        public string Units { get; set; }

        public double ProcessValue { get; set; }
    }
}
