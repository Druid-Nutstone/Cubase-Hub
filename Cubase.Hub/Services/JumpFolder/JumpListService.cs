using Cubase.Hub.Properties;
using Cubase.Hub.Services.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Shell;

namespace Cubase.Hub.Services.JumpFolder
{
    public class JumpListService : IJumpListService
    {
        private readonly IConfigurationService configurationService;

        public JumpListService(IConfigurationService configurationService)
        {
            this.configurationService = configurationService;
        }

        public void Initialise()
        {
            JumpList jumpList = new JumpList();

            this.configurationService.LoadConfiguration(() => { });
            var recentProjects = this.configurationService?.Configuration?.RecentProjects;

            foreach (var project in recentProjects)
            {
                JumpTask projectTask = new JumpTask
                {
                    Title = System.IO.Path.GetFileNameWithoutExtension(project),
                    Description = $"{System.IO.Path.GetFileNameWithoutExtension(project)}",
                    ApplicationPath = Application.ExecutablePath,
                    Arguments = $"open \"{project}\"",
                    IconResourcePath = Path.Combine(Application.StartupPath, "cubasefile.ico"),
                    IconResourceIndex = 0
                };
                jumpList.JumpItems.Add(projectTask);
            }

            JumpTask openTask = new JumpTask
            {
                Title = "Open Albums",
                Description = "Open Album Collection",
                ApplicationPath = Application.ExecutablePath,
                Arguments = "albums",
                IconResourcePath = Path.Combine(Application.StartupPath, "album.ico"),
                IconResourceIndex = 0
            };

            jumpList.JumpItems.Add(openTask);

            jumpList.Apply();
        }
    }
}
