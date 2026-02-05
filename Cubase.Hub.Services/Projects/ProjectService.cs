using Cubase.Hub.Services.Config;
using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.Projects
{
    public class ProjectService : IProjectService
    {
        
        private readonly IConfigurationService configurationService;    

        private readonly IDirectoryService directoryService;

        public CubaseProjectCollection Projects { get; private set; }

        public ProjectService(IConfigurationService configurationService, 
                              IDirectoryService directoryService)
        {
            this.configurationService = configurationService;
            this.directoryService = directoryService;
        }
        
        public CubaseProjectCollection? LoadProjects(Action<string> OnError)
        {
            if (this.configurationService.Configuration == null)
            {
                // as a fall back - see if we can load the config here 
                var config = this.configurationService.LoadConfiguration(() => 
                {
                    OnError?.Invoke("No configuration loaded");
                });
                if (config == null)                 
                {
                    return null;
                }
            }
            this.Projects = new CubaseProjectCollection();
            foreach (var cubaseProjectFolder in this.configurationService.Configuration!.SourceCubaseFolders)
            {
                var projectsInFolder = this.directoryService.GetCubaseProjects(cubaseProjectFolder);
                foreach (var project in projectsInFolder)
                {
                    this.MapCubaseProject(project, this.Projects);
                }   
            }
            return this.Projects;
        }

        private void MapCubaseProject(string sourceProject, CubaseProjectCollection cubaseProjects)
        {
            cubaseProjects.AddProject(sourceProject);
        }
    }
}
