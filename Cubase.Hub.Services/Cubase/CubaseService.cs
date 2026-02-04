using Cubase.Hub.Services.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.Cubase
{
    public class CubaseService : ICubaseService
    {
        private IConfigurationService configurationService;

        public CubaseService(IConfigurationService configurationService) 
        { 
          this.configurationService = configurationService; 
        }

        public void OpenCubaseProject(string projectPath)
        {
            if (File.Exists(projectPath))
            {
                var cubase = new System.Diagnostics.Process();
                cubase.StartInfo.FileName = this.configurationService.Configuration?.CubaseExeLocation;
                cubase.StartInfo.Arguments = $"\"{projectPath}\"";
                cubase.Start();
            }
        }
    }
}
