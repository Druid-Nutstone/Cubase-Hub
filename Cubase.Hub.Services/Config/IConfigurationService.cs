using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.Config
{
    public interface IConfigurationService
    {
        bool IsLoaded { get; set; }

        bool LoadConfiguration(Action? OnLoadError);

        CubaseHubConfiguration? Configuration { get; }     

        bool SaveConfiguration(Action<string>? OnSaveError);

    }
}
