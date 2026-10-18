namespace Cubase.Macro.Common.Models
{
    public class CubasePluginSlotCollection : List<CubasePluginSlot>
    {
        public CubasePluginSlot GetPluginByName(string name)
        {
            if (this.PluginExists(name))
            {
                return this.First(x => x.PluginName.Equals(name, StringComparison.OrdinalIgnoreCase));
            }
            return null;
        }

        public bool PluginExists(string name)
        {
            return this.Any(x => x.PluginName.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }

    public class CubasePluginSlot
    {
        public string PluginName { get; set; }

        public int PluginID { get; set; }

        public int SlotID { get; set; }

        public int Index { get; set; }
    }
}
