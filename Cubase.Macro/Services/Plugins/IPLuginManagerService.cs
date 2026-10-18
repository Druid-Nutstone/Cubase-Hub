using Cubase.Macro.Common.Models;

namespace Cubase.Macro.Services.Plugins
{
    public interface IPLuginManagerService
    {
        CubasePluginSlotCollection GetCubasePluginSlots();

        CubasePluginParameterCollection GetPluginParameters(CubasePluginSlot cubasePluginSlot);
    }
}
