using Cubase.Hub.Services.Models;

namespace Cubase.Hub.Services.Config
{
    public interface IConfigurationService
    {
        bool IsLoaded { get; set; }

        bool LoadConfiguration(Action? OnLoadError);

        CubaseHubConfiguration? Configuration { get; }

        bool SaveConfiguration(Action<string>? OnSaveError);

        string? GetFinalMixLocationFromAlbumName(string albumName);

    }
}
