using Cubase.Macro.Common.Models;
using Nutstone.AI.Assistant.LocalModels.PluginOutputDefinitions;

namespace Cubase.Macro.Services.Plugins
{
    public interface IPLuginManagerService
    {
        CubasePluginSlotCollection GetCubasePluginSlots();

        CubasePluginParameterCollection GetPluginParameters(CubasePluginSlot cubasePluginSlot);

        CubasePluginDetailCollection GetAvailablePluginDetail();

        bool InsertPluginIntoCurrentTrack(CubaseInsertPlugin cubaseInsertPlugin);

        List<CubasePluginParameter> MapAIPluginToCubasePlugin(CubasePluginParameterCollection availableParameters, RootPluginOutput aiPlugin);

        bool UpdatePluginParameter(UpdateCubasePluginParameter parameter);
    }
}
