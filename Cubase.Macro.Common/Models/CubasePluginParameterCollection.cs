using System.Text.Json;

namespace Cubase.Macro.Common.Models
{
    public class CubasePluginParameterCollection : List<CubasePluginParameter>
    {
        public void AddFromOtherCollection(CubasePluginParameterCollection cubasePluginParameters)
        {
            this.AddRange(cubasePluginParameters);
        }

        public CubasePluginParameter GetParameterByName(string name)
        {
            return this.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
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

    public class UpdateCubasePluginParameter : CubasePluginParameter
    {
        public int PluginID { get; set; }


        public static UpdateCubasePluginParameter CreateFromCubasePluginParameter(int pluginID, CubasePluginParameter cubasePluginParameter)
        {
            return new UpdateCubasePluginParameter()
            {
                PluginID = pluginID,
                DisplayValue = cubasePluginParameter.DisplayValue,
                Name = cubasePluginParameter.Name,
                ProcessValue = cubasePluginParameter.ProcessValue,
                Tag = cubasePluginParameter.Tag,
                Units = cubasePluginParameter.Units
            };

        }

    }
}
