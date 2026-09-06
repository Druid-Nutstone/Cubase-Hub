using Cubase.Macro.Common.Lyrics.Services.Scrolling;
using Cubase.Macro.Common.Socket;
using Cubase.Macro.Mobile.Configuration;
using Cubase.Macro.Mobile.Lyrics;
using Cubase.Macro.Mobile.Nav;
using Cubase.Macro.Mobile.Services.Lyrics;
using Cubase.Macro.Mobile.Services.Mswin;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Serilog;

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
                    fonts.AddFont("JetBrainsMono-Regular.ttf", "CustomMono");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<CubaseMacroWebSocketClient>();
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddTransient<LyricViewer>();
            builder.Services.TryAddTransient<ConfigurationPage>();
            builder.Services.AddSingleton<NavPage>();
            builder.Services.AddSingleton<ILyricService, LyricService>();
            builder.Services.AddSingleton<IMobileConfigurationService, MobileConfigurationService>();
            builder.Services.AddSingleton<IMsWinService, MsWinService>();
            builder.Services.AddSingleton<IScrollerService, ScrollerService>();
            builder.Services.AddTransient<MainPage>();
            builder.Logging.ClearProviders();

            builder.Logging.AddSerilog();

            var logPath = CubaseMacroMobileConstants.LogFilePath;

            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            var logFile = Path.Combine(logPath, "app-.txt");


            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.File(
               logFile,
               rollingInterval: RollingInterval.Day,
               retainedFileCountLimit: 10)
               .CreateLogger();

            return builder.Build();
        }
    }
}
