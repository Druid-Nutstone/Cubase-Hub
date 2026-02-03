using Cubase.Hub.Services.Config;
using Cubase.Hub.Services.FileAndDirectory;
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

        public BaseTest()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection
                .AddSingleton<IConfigurationService, ConfigurationService>()
                .AddSingleton<IDirectoryService, DirectoryService>()
                .AddSingleton<IProjectService, ProjectService>();
                
            var provider = serviceCollection.BuildServiceProvider();    
            this.configurationService = provider.GetRequiredService<IConfigurationService>();   
            this.projectService = provider.GetRequiredService<IProjectService>();
        }
    }
}
