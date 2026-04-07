using Cubase.Macro.Services.Keyboard;
using Cubase.Macro.Services.Mouse;
using Cubase.Macro.Services.Window;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Windows.Shell;
using System.IO;
    
using System.Runtime.InteropServices;
using Cubase.Macro.Forms.Configuration;

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
                // initialise mouse watcher 
                var mouseWatcher = services.GetService<IMouseService>();
                mouseWatcher.Initialise();
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
                .AddSingleton<IMouseService, MouseService>()
                .AddScoped<SettingsMainControl>()
                .AddScoped<SettingsForm>()
                .AddScoped<MainForm>();

            var provider = serviceCollection.BuildServiceProvider();
            return provider;
        }
    }
}