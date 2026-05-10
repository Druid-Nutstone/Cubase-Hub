using Cubase.Macro.Forms.Configuration;
using Cubase.Macro.Services.Config;
using Cubase.Macro.Services.Keyboard;
using Cubase.Macro.Services.Midi;
using Cubase.Macro.Services.Window;
using Cubase.Macro.Services.WindowsServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Runtime.InteropServices;
using System.Windows.Shell;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Cubase.Macro.Services.WebSockets;
using Cubase.Macro.Common.Models;
using Cubase.Macro.Forms.Lyrics;

namespace Cubase.Macro
{
    public static class Program
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

            var host = CreateHostApiAndServices();
            
            // var services = InstallServices();

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
            
            LoadForm(host, args);

        }

        static void LoadForm(IHost host, string[] args) 
        {
            if (args.Count() < 1)
            {
                // get main form 
                var mainForm = host.Services.GetService<MainForm>();
                mainForm.WindowState = FormWindowState.Minimized;
                StartMidiService(host);
                // start websocket api 
                _ = host.RunAsync();
                Application.Run(host.Services.GetService<MainForm>());
            }
            else
            {
                var options = args[0]; 
                if (options == "settings")
                {
                    var configForm = host.Services.GetService<SettingsForm>();
                    Application.Run(configForm);
                }
                if (options == "lyrics")
                {
                    var lyricForm = host.Services.GetService<LyricsForm>();
                    Application.Run(lyricForm);
                }

            }
        }

        static void StartMidiService(IHost host)
        {
            Log.Logger.Information($"Loading Config from {CubaseMacroConstants.ConfigurationFileName}");
            var configurationService = host.Services.GetService<IConfigurationService>();
            configurationService?.ReloadConfiguration();

            var windowsService = host.Services.GetService<IWindowsControllerService>();

            var shouldReloadWindowsMidiService = false;

            if (configurationService?.Configuration != null)
            {
                shouldReloadWindowsMidiService = configurationService.Configuration.ReloadWindowsMidiService;

                if (shouldReloadWindowsMidiService)
                {
                    windowsService.StopMidiWindowsService(); 
                }
            }

            var midiService = host.Services.GetService<IMidiService>();
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
            jumpList.JumpItems.Add(new JumpTask
            {
                Title = "Open Lyrics",
                Arguments = "lyrics",
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


        public static IHost CreateHostApiAndServices()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddLogging(builder =>
                    {
                        builder.ClearProviders();
                        builder.AddSerilog();
                    });

                    services
                        .AddSingleton<IKeyboardService, KeyboardService>()
                        .AddSingleton<IWindowService, WindowService>()
                        .AddSingleton<IConfigurationService, ConfigurationService>()
                        .AddSingleton<IMidiService, MidiService>()
                        .AddSingleton<IWindowsControllerService, WindowsControllerService>()
                        .AddScoped<SettingsMainControl>()
                        .AddScoped<SettingsForm>()
                        .AddScoped<LyricsForm>()
                        .AddScoped<MainForm>();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options => 
                    {
                        options.ListenAnyIP(8014);
                    });
                    webBuilder.UseKestrel()
                        .UseUrls("http://localhost:8014")
                        .Configure(app =>
                        {
                            app.UseWebSockets();
                            app.Use(async (context, next) =>
                            {
                                if (context.WebSockets.IsWebSocketRequest)
                                {
                                    var midi = context.RequestServices.GetRequiredService<IMidiService>();
                                    var config = context.RequestServices.GetRequiredService<IConfigurationService>();

                                    var ip = context.Connection.RemoteIpAddress?.ToString();
                                    var port = context.Connection.RemotePort;

                                    Log.Information($"WebSocket connection from {ip}:{port}");
                                    using var ws = await context.WebSockets.AcceptWebSocketAsync();
                                    await CubaseSockets.HandleWebSocket(ws, midi, Log.Logger, config);
                                }
                                else
                                {
                                    await next();
                                }
                            });
                        });
                })
                .Build();
        }
    }


}