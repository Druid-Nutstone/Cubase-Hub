using Cubase.Hub.Services.Config;
using Cubase.Hub.Services.Distributers;
using Cubase.Hub.Services.Distributers.SoundCloud;
using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.Models;
using Cubase.Hub.Services.Projects;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubse.Hub.Tests
{
    public class BaseTest
    {
        protected IConfigurationService configurationService;

        protected IProjectService projectService;

        protected IServiceProvider serviceProvider;

        public BaseTest()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection
                .AddSingleton<IConfigurationService, ConfigurationService>()
                .AddSingleton<IDirectoryService, DirectoryService>()
                .AddKeyedSingleton<IDistributionProvider, SoundCloudDistributionProvider>(DistributionProvider.SoundCloud)
                .AddSingleton<IProjectService, ProjectService>();
                
            this.serviceProvider = serviceCollection.BuildServiceProvider();    
            this.configurationService = serviceProvider.GetRequiredService<IConfigurationService>();   
            this.projectService = serviceProvider.GetRequiredService<IProjectService>();
        }
    }
}
