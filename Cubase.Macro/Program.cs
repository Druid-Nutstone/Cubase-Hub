using Cubase.Macro.Forms.Configuration;
using Cubase.Macro.Forms.Main;
using Cubase.Macro.Services.Config;
using Cubase.Macro.Services.Keyboard;
using Cubase.Macro.Services.Midi;
using Cubase.Macro.Services.Monitor;
using Cubase.Macro.Services.Mouse;
using Cubase.Macro.Services.Window;
using Cubase.Macro.Services.WindowsServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Diagnostics;
using System.IO;
    
using System.Runtime.InteropServices;
using System.Windows.Shell;

namespace Cubase.Macro
{
    internal static class Program
    {
        [DllImport("shell32.dll")]
        private static extern int SetCurrentProcessExplicitAppUserModelID(string AppID);

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            ApplicationConfiguration.Initialize();
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            SetCurrentProcessExplicitAppUserModelID("DavidNuttall.CubaseMacro");

            var services = InstallServices();

            SetJumpListItems();

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
            
            LoadForm(services, args);

        }

        static void LoadForm(IServiceProvider services, string[] args) 
        {
            if (args.Count() < 1)
            {
                // get main form 
                var mainForm = services.GetService<MainForm>();
                mainForm.WindowState = FormWindowState.Minimized;
                StartMidiService(services);
                Application.Run(services.GetService<MainForm>());
            }
            else
            {
                var options = args[0]; 
                if (options == "settings")
                {
                    var configForm = services.GetService<SettingsForm>();
                    Application.Run(configForm);
                }

            }
        }

        static void StartMidiService(IServiceProvider services)
        {
            var configurationService = services.GetService<IConfigurationService>();
            configurationService?.ReloadConfiguration();

            var windowsService = services.GetService<IWindowsControllerService>();

            var shouldReloadWindowsMidiService = false;

            if (configurationService?.Configuration != null)
            {
                shouldReloadWindowsMidiService = configurationService.Configuration.ReloadWindowsMidiService;

                if (shouldReloadWindowsMidiService)
                {
                    windowsService.StopMidiWindowsService(); 
                }
            }

            var midiService = services.GetService<IMidiService>();
            midiService.Initialise();

            if (shouldReloadWindowsMidiService)
            {
                 windowsService.StartMidiWindowsService();
            }
        }

        static void SetJumpListItems()
        {
            var jumpList = new JumpList();
            jumpList.JumpItems.Add(new JumpTask
            {
                Title = "Open Settings",
                Arguments = "settings",
                ApplicationPath = Application.ExecutablePath,
                IconResourcePath = Application.ExecutablePath,
                IconResourceIndex = 0
            });
            jumpList.Apply();
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
                .AddSingleton<IConfigurationService, ConfigurationService>()
                .AddSingleton<IMidiService, MidiService>()
                .AddSingleton<IWindowsControllerService, WindowsControllerService>()
                .AddScoped<SettingsMainControl>()
                .AddScoped<SettingsForm>()
                .AddScoped<MainForm>();

            var provider = serviceCollection.BuildServiceProvider();
            return provider;
        }

    }
}