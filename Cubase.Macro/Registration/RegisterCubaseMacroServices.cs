using Cubase.Macro.Common.Lyrics.Services.Scrolling;
using Cubase.Macro.Common.Socket;
using Cubase.Macro.Services.Config;
using Cubase.Macro.Services.Keyboard;
using Cubase.Macro.Services.Midi;
using Cubase.Macro.Services.Plugins;
using Cubase.Macro.Services.Window;
using Microsoft.Extensions.DependencyInjection;
using Nutstone.AI.Assistant.Services.Plugins;

namespace Cubase.Macro.Registration
{
    public static class RegisterCubaseMacroServices
    {

        public static IServiceCollection RegisterCubaseMacro(this IServiceCollection services)
        {
            return services
                   .AddSingleton<IKeyboardService, KeyboardService>()
                   .AddSingleton<IWindowService, WindowService>()
                   .AddSingleton<IConfigurationService, ConfigurationService>()
                   .AddSingleton<IMidiService, MidiService>()
                   .AddSingleton<IScrollerService, ScrollerService>()
                   .AddSingleton<IPLuginManagerService, PluginManagerService>()
                   .AddSingleton<IPluginService, PluginService>()
                   .AddSingleton<CubaseMacroWebSocketClient>();
        }
    }
}
