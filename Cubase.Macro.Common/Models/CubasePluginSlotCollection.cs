namespace Cubase.Macro.Common.Models
{
    public class CubasePluginSlotCollection : List<CubasePluginSlot>
    {

        public void AddFromOtherCollection(CubasePluginSlotCollection cubasePluginSlots)
        {
            this.AddRange(cubasePluginSlots);
        }

        public CubasePluginSlot? GetPluginByName(string name)
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

        public List<string> GetActivePluginList()
        {
            return this.Where(x => !string.IsNullOrEmpty(x.PluginName))
                       .Select(x => x.PluginName)
                       .ToList();
        }

        public List<CubasePluginSlot> GetAllPlugins()
        {
            return this.Where(x => !string.IsNullOrEmpty(x.PluginName)).ToList();
        }

        public int? GetFirstFreeSlot()
        {
            var slot = this.FirstOrDefault(x => string.IsNullOrEmpty(x.PluginName));
            if (slot != null)
            {
                return slot.SlotID;
            }
            return null;
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
