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

        public void OpenCubaseProject(string projectPath, Action<string>? OnError)
        {
            if (File.Exists(projectPath))
            {
                var cubaseLocation = this.configurationService.Configuration?.CubaseExeLocation;
                if (!string.IsNullOrEmpty(cubaseLocation))
                {
                    var cubase = new System.Diagnostics.Process();
                    cubase.StartInfo.FileName = this.configurationService.Configuration?.CubaseExeLocation;
                    cubase.StartInfo.Arguments = $"\"{projectPath}\"";
                    cubase.Start();
                }
                else
                {
                    if (OnError != null)
                    {
                        OnError("Cubase executable location is not configured. Please set it in the configuration.");
                    }
                }
            }
            else
            {
                if (OnError != null)
                {
                    OnError($"Project file not found: {projectPath}");
                }
            }
        }
    }
}
