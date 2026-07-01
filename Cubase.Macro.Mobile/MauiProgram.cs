using Cubase.Macro.Common.Lyrics.Scrolling;
using Cubase.Macro.Common.Lyrics.Services;
using Cubase.Macro.Common.Socket;
using Cubase.Macro.Mobile.Configuration;
using Cubase.Macro.Mobile.Lyrics;
using Cubase.Macro.Mobile.Nav;
using Cubase.Macro.Mobile.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Cubase.Macro.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<CubaseMacroWebSocketClient>();
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddTransient<LyricViewer>();
            builder.Services.TryAddTransient<ConfigurationPage>();
            builder.Services.AddSingleton<NavPage>();
            builder.Services.AddSingleton<FileHandler>();
            builder.Services.AddSingleton<ILyricService, LyricService>();
            builder.Services.AddSingleton<IMobileConfigurationService, MobileConfigurationService>(); 
            builder.Services.AddSingleton<IColourService, ColourService>();
            builder.Services.AddSingleton<IlyricMidiService, MobileLyricService>();
            builder.Services.AddTransient<MainPage>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
