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

        private DateTime LastModified = DateTime.MinValue;

        public bool IsLoaded { get => this.configuration != null; set { } } 
        
        public CubaseHubConfiguration? Configuration => this.configuration;

        public ConfigurationService(IDirectoryService directoryService)
        {
            this.directoryService = directoryService;
        }



        public bool LoadConfiguration(Action? OnLoadError)
        {
            this.directoryService.MakeSureDirectoryExists(CubaseHubConstants.ConfigurationFileName);
            if (File.Exists(CubaseHubConstants.ConfigurationFileName))
            {
                var fileContent = File.ReadAllText(CubaseHubConstants.ConfigurationFileName);
                this.configuration = JsonSerializer.Deserialize<CubaseHubConfiguration>(fileContent);
                this.SetLastModified();
                return true;
            }
            if (OnLoadError != null)
            {
                OnLoadError();

            }
            return false;

        }

        private void SetLastModified()
        {
            this.LastModified = this.GetLastModifiedFromFile();
        }


        private DateTime GetLastModifiedFromFile()
        {
            var lm = new FileInfo(CubaseHubConstants.ConfigurationFileName);
            return lm.LastWriteTime;
        }

        public bool SaveConfiguration(Action<string>? OnSaveError)
        {
            try
            {
                // check lastmodified in case config has been changed via another instance of config 
                // as will happen if albums are loaded from the jumplist or an external source 
                if (!(this.GetLastModifiedFromFile().CompareTo(this.LastModified) > 0))
                {
                    var newConfig = JsonSerializer.Serialize<CubaseHubConfiguration>(this.configuration, new JsonSerializerOptions() { WriteIndented = true });
                    File.WriteAllText(CubaseHubConstants.ConfigurationFileName, newConfig);
                    this.LastModified = this.GetLastModifiedFromFile();
                }
                return true;
            }
            catch (Exception ex)
            {
                if (OnSaveError != null)
                {
                    OnSaveError(ex.Message);
                }
                return false;
            }
        }
    }
}
