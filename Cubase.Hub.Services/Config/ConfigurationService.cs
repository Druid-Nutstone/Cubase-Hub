using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Cubase.Hub.Services.Config
{
    public class ConfigurationService : IConfigurationService
    {
        private CubaseHubConfiguration? configuration;

        private readonly IDirectoryService directoryService;

        public bool IsLoaded { get => this.configuration != null; set { } } 
        
        public CubaseHubConfiguration? Configuration => this.configuration;

        public ConfigurationService(IDirectoryService directoryService)
        {
            this.directoryService = directoryService;
        }



        public CubaseHubConfiguration? LoadConfiguration(Action? OnLoadError)
        {
            this.directoryService.MakeSureDirectoryExists(CubaseHubConstants.ConfigurationFileName);
            if (File.Exists(CubaseHubConstants.ConfigurationFileName))
            {
                var fileContent = File.ReadAllText(CubaseHubConstants.ConfigurationFileName);
                this.configuration = JsonSerializer.Deserialize<CubaseHubConfiguration>(fileContent);   
                return this.configuration;
            }
            if (OnLoadError != null)
            {
                OnLoadError();
            }
            return null;
        }

        public bool SaveConfiguration(CubaseHubConfiguration configuration, Action? OnSaveError)
        {
            try
            {
                var newConfig = JsonSerializer.Serialize<CubaseHubConfiguration>(configuration, new JsonSerializerOptions() { WriteIndented = true});
                File.WriteAllText(CubaseHubConstants.ConfigurationFileName, newConfig);
                this.configuration = configuration; 
                return true;
            }
            catch (Exception)
            {
                if (OnSaveError != null)
                {
                    OnSaveError();
                }
                return false;
            }
        }
    }
}
