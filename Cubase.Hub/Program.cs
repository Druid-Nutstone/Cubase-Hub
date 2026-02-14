using Cubase.Hub.Controls.MainFormControls.ProjectsControl;
using Cubase.Hub.Controls.MainFormControls.ProjectsControl.PlayControls;
using Cubase.Hub.Controls.MainFormControls.ProjectsForm;
using Cubase.Hub.Forms;
using Cubase.Hub.Forms.Albums;
using Cubase.Hub.Forms.Config;
using Cubase.Hub.Forms.Export;
using Cubase.Hub.Forms.Main;
using Cubase.Hub.Forms.Main.Menu;
using Cubase.Hub.Forms.Mixes;
using Cubase.Hub.Forms.Tracks;
using Cubase.Hub.Services.Audio;
using Cubase.Hub.Services.Config;
using Cubase.Hub.Services.Cubase;
using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Projects;
using Microsoft.Extensions.DependencyInjection;
namespace Cubase.Hub
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var serviceProvider = InstallServices();
            var form = serviceProvider.GetRequiredService<MainForm>();
            Application.Run(form);
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
                .AddTransient<PlayControl>()
                .AddTransient<ExportForm>()
                .AddTransient<CubaseProjectItemControl>()
                .AddTransient<CubaseProjectExtendedPropertiesControl>()
                .AddTransient<CubaseProjectItemMixesControl>()
                .AddSingleton<IMessageService, MessageService>()
                .AddSingleton<IConfigurationService, ConfigurationService>()
                .AddSingleton<IDirectoryService, DirectoryService>()
                .AddSingleton<ICubaseService, CubaseService>()
                .AddTransient<IAudioService, AudioService>()
                .AddSingleton<IProjectService, ProjectService>();   
            var provider = serviceCollection.BuildServiceProvider();
            return provider;
        }
    }
}