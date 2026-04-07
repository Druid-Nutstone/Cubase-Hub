using Cubase.Macro.Services.Keyboard;
using Cubase.Macro.Services.Mouse;
using Cubase.Macro.Services.Window;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Cubase.Macro
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            ApplicationConfiguration.Initialize();

            var services = InstallServices(); 

            // get main form 
            var mainForm = services.GetService<MainForm>();
            mainForm.WindowState = FormWindowState.Minimized;

            // initialise mouse watcher 
            var mouseWatcher = services.GetService<IMouseService>();
            mouseWatcher.Initialise();

            var logPath = Path.Combine(
                  CubaseMacroConstants.LogPath,
                  $"{CubaseMacroConstants.CubaseMacroLog}.log");

            Directory.CreateDirectory(Path.GetDirectoryName(logPath)!);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(
                  logPath,
                  rollingInterval: RollingInterval.Day,
                  retainedFileCountLimit: 10)
                .CreateLogger();

            Application.Run(services.GetService<MainForm>());
        }

        static IServiceProvider InstallServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddSerilog();
            });

            serviceCollection
                .AddSingleton<IKeyboardService, KeyboardService>()
                .AddSingleton<IWindowService, WindowService>()
                .AddSingleton<IMouseService, MouseService>()
                .AddScoped<MainForm>();

            var provider = serviceCollection.BuildServiceProvider();
            return provider;
        }
    }
}