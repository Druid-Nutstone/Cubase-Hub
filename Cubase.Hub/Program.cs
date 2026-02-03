using Cubase.Hub.Controls.MainFormControls.ProjectsForm;
using Cubase.Hub.Forms.Config;
using Cubase.Hub.Forms.Main;
using Cubase.Hub.Services.Config;
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
                .AddSingleton<IMessageService, MessageService>()
                .AddSingleton<IConfigurationService, ConfigurationService>()
                .AddSingleton<IDirectoryService, DirectoryService>()
                .AddSingleton<IProjectService, ProjectService>();   
            var provider = serviceCollection.BuildServiceProvider();
            return provider;
        }
    }
}