using Cubase.Hub.Controls.Export;
using Cubase.Hub.Controls.MainFormControls.ProjectsControl;
using Cubase.Hub.Controls.MainFormControls.ProjectsControl.PlayControls;
using Cubase.Hub.Controls.MainFormControls.ProjectsForm;
using Cubase.Hub.Forms;
using Cubase.Hub.Forms.Albums;
using Cubase.Hub.Forms.CompletedMixes;
using Cubase.Hub.Forms.Config;
using Cubase.Hub.Forms.Edit;
using Cubase.Hub.Forms.Export;
using Cubase.Hub.Forms.Main;
using Cubase.Hub.Forms.Main.Menu;
using Cubase.Hub.Forms.Mixes;
using Cubase.Hub.Forms.Tracks;
using Cubase.Hub.Services.Album;
using Cubase.Hub.Services.Audio;
using Cubase.Hub.Services.Config;
using Cubase.Hub.Services.Cubase;
using Cubase.Hub.Services.Distributers;
using Cubase.Hub.Services.Distributers.RouteNoteDistro;
using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.JumpFolder;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Models;
using Cubase.Hub.Services.Projects;
using Cubase.Hub.Services.Track;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace Cubase.Hub
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
            SetCurrentProcessExplicitAppUserModelID("DavidNuttall.CubaseHub");
            ApplicationConfiguration.Initialize();
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var serviceProvider = InstallServices();
            var cubaseService = serviceProvider.GetRequiredService<ICubaseService>();   
            var configurationService = serviceProvider.GetRequiredService<IConfigurationService>(); 
            if (!ProcessAnyCommands(args, cubaseService, configurationService, serviceProvider))
            {
                var form = serviceProvider.GetRequiredService<MainForm>();
                var jumpListService = serviceProvider.GetRequiredService<IJumpListService>();
                jumpListService.Initialise();
                Application.Run(form);
            }
        }

        static bool ProcessAnyCommands(string[] args, ICubaseService cubaseService, IConfigurationService configurationService, IServiceProvider serviceProvider)
        {
            configurationService.LoadConfiguration(() => 
            { 
                MessageBox.Show("Error loading configuration. Please check your configuration file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            });

            if (args.Length > 0)
            {
                if (args[0] == "open")
                {
                    cubaseService.OpenCubaseProject(args[1], (err) => 
                    { 
                        MessageBox.Show($"Error opening project: {err}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    });
                    // open last project logic
                    return true;
                }

                if (args[0] == "albums")
                {
                    var form = serviceProvider.GetRequiredService<ManageAlbumsForm>();
                    form.Initialise();
                    Application.Run(form);
                    return true;
                }

                if (args[0] == "minimise")
                {
                    var form = serviceProvider.GetRequiredService<MainForm>();
                    form.StartMinimised();
                    Application.Run(form);
                    return true;
                }

                if (args[0] == "editaudio")
                {
                    var form = serviceProvider.GetRequiredService<EditTrackForm>();
                    form.Initialise(args[1]);
                    Application.Run(form);
                    return true;
                }
            }
            return false;
        }

        static IServiceProvider InstallServices()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection
                .AddScoped<MainForm>()
                .AddScoped<ConfigurationForm>()
                .AddScoped<ProjectsControl>()
                .AddScoped<CubaseProjectControl>()
                .AddScoped<MenuContent>()
                .AddScoped<NewAlbumForm>()
                .AddScoped<NewTrackForm>()
                .AddScoped<ManageAlbumsForm>()
                .AddScoped<ManageMixesForm>()
                .AddScoped<EditTrackForm>()
                .AddTransient<PlayControl>()
                .AddTransient<ExportProjectControl>()
                .AddTransient<ExportForm>()
                .AddTransient<CubaseProjectItemControl>()
                .AddTransient<CubaseProjectExtendedPropertiesControl>()
                .AddTransient<CubaseProjectItemMixesControl>()
                .AddSingleton<IMessageService, MessageService>()
                .AddSingleton<IConfigurationService, ConfigurationService>()
                .AddSingleton<IDirectoryService, DirectoryService>()
                .AddSingleton<ICubaseService, CubaseService>()
                .AddSingleton<IAudioService, AudioService>()
                .AddSingleton<CompletedMixesForm,  CompletedMixesForm>()
                .AddTransient<IAlbumService, AlbumService>()
                .AddTransient<ITrackService, TrackService>()
                .AddSingleton<IJumpListService, JumpListService>()
                .AddKeyedTransient<IDistributer, RouteNoteDistributer>(Distributers.RouteNote)
                .AddSingleton<IProjectService, ProjectService>();
                
            var provider = serviceCollection.BuildServiceProvider();
            return provider;
        }
    }
}