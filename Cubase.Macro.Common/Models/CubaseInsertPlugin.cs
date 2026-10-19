namespace Cubase.Macro.Common.Models
{
    public class CubaseInsertPlugin
    {
        public string PluginName { get; set; }

        public string PluginID { get; set; }

        public int SlotID { get; set; }

        public static CubaseInsertPlugin CreateFromPluginDetail(int slotID, CubasePluginDetail cubasePluginDetail)
        {
            return new CubaseInsertPlugin()
            {
                PluginID = cubasePluginDetail.PluginID,
                PluginName = cubasePluginDetail.PluginName,
                SlotID = slotID
            };
        }
    }
}
