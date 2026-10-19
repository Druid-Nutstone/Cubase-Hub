namespace Cubase.Macro.Common.Models
{
    public class CubasePluginDetailCollection : List<CubasePluginDetail>
    {
        public void AddFromOtherCollection(CubasePluginDetailCollection cubasePluginDetails)
        {
            this.AddRange(cubasePluginDetails);
        }

        public CubasePluginDetail? GetPluginByName(string pluginName)
        {
            if (this.HavePlugin(pluginName))
            {
                return this.First(x => x.PluginName.Equals(pluginName, StringComparison.OrdinalIgnoreCase));
            }
            return null;
        }

        public bool HavePlugin(string pluginName)
        {
            return this.Any(x => x.PluginName.Equals(pluginName, StringComparison.OrdinalIgnoreCase));
        }
    }

    public class CubasePluginDetail
    {
        public string PluginName { get; set; }

        public string PluginID { get; set; }

        public int SlotID { get; set; }
    }
}
