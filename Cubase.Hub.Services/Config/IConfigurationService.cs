using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.Config
{
    public interface IConfigurationService
    {
        bool IsLoaded { get; set; }

        CubaseHubConfiguration? LoadConfiguration(Action? OnLoadError);

        CubaseHubConfiguration? Configuration { get; }     

        bool SaveConfiguration(CubaseHubConfiguration configuration, Action<string>? OnSaveError);

    }
}
