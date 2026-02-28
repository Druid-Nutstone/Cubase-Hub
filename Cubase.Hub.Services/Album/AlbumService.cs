using Cubase.Hub.Services.Audio;
using Cubase.Hub.Services.Config;
using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.Models;
using Cubase.Hub.Services.Track;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.Album
{
    public class AlbumService : IAlbumService
    {
        public AlbumConfiguration Configuration { get; private set; } = new AlbumConfiguration();

        private readonly IConfigurationService configurationService;

        private readonly IDirectoryService directoryService;

        private readonly ITrackService trackService;

        public AlbumService(IConfigurationService configurationService, 
                            ITrackService trackService,
                            IDirectoryService directoryService)
        {
            this.configurationService = configurationService;
            this.trackService = trackService;
            this.directoryService = directoryService;
        }
        
        public List<AlbumLocation> GetAlbumList(Action<string> onError)
        {
            var result = this.directoryService.GetCubaseAlbums(this.configurationService.Configuration.SourceCubaseFolders);
            if (result == null || result.Count == 0)
            {
                onError.Invoke($"There are no albums defined in the cubase directories");
            }
            return result;
        }

        public void NewAlbum()
        {
            this.Configuration = new AlbumConfiguration();
        }

        public bool SaveAlbum(string targetDirectory, Action<string> onError)
        {
            try
            {
                this.Configuration.SaveToDirectory(targetDirectory);
                return true;
            }
            catch (Exception ex)
            {
                onError(ex.Message);
            }
            return false;
        }

        public bool VerifyAlbum(Action<string> onError)
        {
            var isOK = this.Configuration.Verify(out string propertyInError);

            if (!isOK)
            {
                onError.Invoke($"The album property {propertyInError} is Invalid");
                return false;
            }

            return true;
        }

        public AlbumConfiguration GetAlbumConfigurationFromAlbumLocation(AlbumLocation albumLocation)
        {
            this.Configuration = AlbumConfiguration.LoadFromFile(Path.Combine(albumLocation.AlbumPath, CubaseHubConstants.CubaseAlbumConfigurationFileName));
            return this.Configuration;
        }

        public MixDownCollection GetMixesForAlbum(AlbumLocation albumLocation)
        {
            return this.trackService.GetMixesForAlbum(albumLocation);
        }

        public string InitialiseAlbumExportLocation(AlbumLocation albumLocation, Action<string> onError)
        {
            if (this.configurationService?.Configuration?.AlbumExportLocation != null)
            {
                var albumExportLocation = Path.Combine(this.configurationService?.Configuration?.AlbumExportLocation, albumLocation.AlbumName);
                var albumExportConfig = new AlbumExport { Name = albumLocation.AlbumName, Location = albumExportLocation };
                this.configurationService.Configuration.AlbumExports.Add(albumExportConfig);
                var saveState = this.configurationService.SaveConfiguration((err) =>
                {
                    onError.Invoke($"Could not save configuration for album {albumLocation.AlbumName}. You will have to do it manually");
                });
                this.InitialiseAlbumArt(albumExportLocation);
                return albumExportLocation;
            }
            else
            {
                onError.Invoke($"You have not configured the root album export directory. Use the options menu");
                return null;
            }
        }

        public string AlbumExportLocation(AlbumLocation albumLocation)
        {
            return this.configurationService?.Configuration?.AlbumExports?.FirstOrDefault(x => x.Name.Equals(albumLocation.AlbumName))?.Location;
         }

        public void InitialiseAlbumArt(string albumExportLocation)
        {
            this.directoryService.MakeSureDirectoryExists(Path.Combine(albumExportLocation, CubaseHubConstants.AlbumArt));
            this.directoryService.MakeSureDirectoryExists(Path.Combine(albumExportLocation, CubaseHubConstants.TrackArt));
        }

        public string? GetAlbumArt(AlbumLocation albumLocation)
        {
            var albumFinalMixLocation = this.configurationService.GetFinalMixLocationFromAlbumName(albumLocation.AlbumName);
            if (albumFinalMixLocation != null) 
            { 
                var albumArtLocation = Path.Combine(albumFinalMixLocation, CubaseHubConstants.AlbumArt);
                return Directory.GetFiles(albumArtLocation, "*.*")?.FirstOrDefault();
            }
            return null;
        }

        public bool CopyAlbumArt(AlbumLocation albumLocation, string targetart)
        {
            var albumFinalMixLocation = this.configurationService.GetFinalMixLocationFromAlbumName(albumLocation.AlbumName);
            if (albumFinalMixLocation != null)
            {
                var albumArtLocation = Path.Combine(albumFinalMixLocation, CubaseHubConstants.AlbumArt);

                var existingArt = Directory.GetFiles(albumArtLocation, $"{albumLocation.AlbumName}.*");
                foreach (var delFile in existingArt)
                {
                    File.Delete(delFile);
                }

                File.Copy(targetart, Path.Combine(albumArtLocation, $"{albumLocation.AlbumName}{Path.GetExtension(targetart)}"), true);
                return true;
            }
            return false;
        }
    }
}
