using Cubase.Macro.Common.Models;
using Nutstone.AI.Assistant.LocalModels.PluginOutputDefinitions;
using Nutstone.AI.Assistant.Models;

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

        bool RemovePlugin(CubasePluginSlot cubasePlugin);

        bool UpdateCubasePlugin(RootPluginOutput rootPlugin, Action<string> onError, Action<string> onWarning);

        [Obsolete("Just use the TrackCollection from the midi service")]
        TrackCollection? GetTracks();

        ChatRequest? GetChatRequest(Action<string> onError);

        void ProcessPluginRequest(ChatResult pluginRequest);


    }
}
