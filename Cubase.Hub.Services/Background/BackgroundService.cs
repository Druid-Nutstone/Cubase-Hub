using Cubase.Hub.Services.Album;
using Cubase.Hub.Services.Config;
using Cubase.Hub.Services.Distributers.SoundCloud;
using Cubase.Hub.Services.Models;
using Cubase.Hub.Services.Track;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;


namespace Cubase.Hub.Services.Background
{

    public enum BackgroundProcessState
    {
        Run = 0,
        Stop = 1,
        Pause = 2,
        Resume = 3,

    }

    public class BackgroundService : IBackgroundService
    {


        private BackgroundProcessState _state = BackgroundProcessState.Run;

        private readonly IConfigurationService configurationService;

        private readonly IAlbumService albumService;

        private readonly ITrackService trackService;

        private readonly IServiceProvider serviceProvider;

        private Dictionary<DistributionProvider, Action> DistributerMethods;

        public BackgroundService(IAlbumService albumService,
                                 IServiceProvider serviceProvider,
                                 IConfigurationService configurationService,
                                 ITrackService trackService)
        {
            this.configurationService = configurationService;
            this.albumService = albumService;
            this.serviceProvider = serviceProvider;
            this.trackService = trackService;
            this.DistributerMethods = new Dictionary<DistributionProvider, Action>()
            {
                { DistributionProvider.SoundCloud, this.SoundCloudDistributer }
            };
            if (!Directory.Exists(CubaseHubConstants.LogPath))
            {
                Directory.CreateDirectory(CubaseHubConstants.LogPath);
            }
            if (!Directory.Exists(CubaseHubConstants.CachePath))
            {
                Directory.CreateDirectory(CubaseHubConstants.CachePath);
            }
            SoundCloudCache.RemoveCache();
        }

        public void Pause()
        {
            this._state = BackgroundProcessState.Pause;
        }

        public void Resume()
        {
            this._state = BackgroundProcessState.Run;
        }

        public void Start()
        {
            Task.Run(() =>
            {
                while (this._state != BackgroundProcessState.Stop)
                {
                    switch (_state)
                    {
                        case BackgroundProcessState.Run:
                            this.RemoveOldLogs();
                            this.Monitor();
                            break;
                    }
                    Task.Delay(TimeSpan.FromSeconds(60)).Wait();
                }

            });
        }

        public void Stop()
        {
            this._state = BackgroundProcessState.Stop;
        }

        private void RemoveOldLogs()
        {
            var logPath = CubaseHubConstants.UserAppDataFolderPath;
            var logFiles = Directory
                .GetFiles(logPath, $"{CubaseHubConstants.DistributionLogFilePrefix}*.*")
                .Select(f => new FileInfo(f))
                .OrderByDescending(f => f.LastWriteTimeUtc)
                .Skip(5);

            foreach (var file in logFiles)
            {
                Log($"Deleting log {file}");
                file.Delete();
            }
        }

        private void Monitor()
        {
            this.CheckForUpdatedAlbums();
            this.CheckForUpdatedMixes();
        }


        private void CheckForUpdatedAlbums()
        {
            var albumLocations = this.albumService.GetAlbumList(OnError);
            foreach (var albumLoc in albumLocations)
            {
                // export location for this album 
                var albumLocation = this.albumService.AlbumExportLocation(albumLoc);
                var albumConfiguration = this.albumService.GetAlbumConfigurationFromAlbumLocation(albumLoc);
                // get the mixes for this album 
                var albumMixes = this.albumService.GetMixesForAlbum(albumLoc);
                // ensure all the mixes have the correct export location  
                albumMixes.SetMixdownExportLocation(albumLocation);
                // and the same for distribution mixes 
                albumConfiguration.DistributionMixes.SetMixdownExportLocation(albumLocation);
                foreach (var item in albumMixes.ThatHaveMixDownAsTitle())
                {
                    var haveSavedMixdown = albumConfiguration.FindMixDown(item);
                    if (haveSavedMixdown != null)
                    {
                        // reset a selected distribution mix to the saved info 
                        // it has been refreshed by cubase as a new mixdown 
                        item.SetAlbumInformation(albumConfiguration);
                        item.UpdateFromAnotherMix(haveSavedMixdown);

                    }
                    else // not saved but set some defaults !
                    {
                        item.Title = item.Title = Path.GetFileNameWithoutExtension(item.FileName);
                        item.SetAlbumInformation(albumConfiguration);
                    }
                    this.trackService.SetTagsFromMixDowm(item);
                }
                // locate any updated mixes that need to be copied for distribution 
                var mixesThatHaveChanged = albumConfiguration.CheckForUpdatedDistributionMixes(albumMixes);
                // copy any changed mixes 
                foreach (var updatedMix in mixesThatHaveChanged)
                {
                    this.Log($"Copying mix {Path.GetFileName(updatedMix.FileName)} to distribution mix {updatedMix.ExportLocation}");
                    File.Copy(updatedMix.FileName, Path.Combine(updatedMix.ExportLocation, Path.GetFileName(updatedMix.FileName)), true);
                }
            }
        }

        private void CheckForUpdatedMixes()
        {
            var distroKiddy = this.configurationService?.Configuration?.DistributionConfiguration?.DistributionProvider;
            if (distroKiddy != null && distroKiddy != DistributionProvider.None)
            {
                if (this.DistributerMethods.ContainsKey(distroKiddy.Value))
                {
                    this.DistributerMethods[distroKiddy.Value].Invoke();
                }
            }
        }

        private void SoundCloudDistributer()
        {
            // don't log on here !
            if (!SoundCloudTokenResponse.TokenExists()) return;
            
            var soundCloud = this.serviceProvider.GetService<SoundCloudDistributionProvider>();
            var soundCloudCache = SoundCloudCache.Create();
            List<AlbumLocation>? albums = null;

            if (!soundCloudCache.CacheExists())
            {
                if (soundCloud != null)
                {
                    if (Connect())
                    {
                        if (!soundCloudCache.CreateCache(soundCloud, OnError))
                        {
                            return; 
                        }
                    }
                    else
                    {
                        this.Log($"Could not connect to soundcloud");
                        return;
                    }
                }
                else
                {
                    this.Log($"Could not get soundcloud from the service provider");
                    return;
                }
            }
            // get all mixes for all albums that have been selected for distribution
            var allAlbumaMixes = new MixDownCollection();
            albums = this.albumService.GetAlbumList(this.OnError);
            foreach (var album in albums)
            {
                var albumMixes = this.albumService.GetAlbumConfigurationFromAlbumLocation(album);
                allAlbumaMixes.AddRange(albumMixes.DistributionMixes);
            }
            // compare last_modified
            foreach (var localMixDown in allAlbumaMixes)
            {
                if (Connect())
                {
                    var remoteMixDown = soundCloudCache.Tracks.FirstOrDefault(x => x.Title == localMixDown.Title);
                    if (remoteMixDown == null || remoteMixDown.isUploadDateOlderThan(localMixDown.LastModified))
                    {
                        this.Log($"Upload {localMixDown.Title} to soundcloud..");
                        soundCloud?.UploadTrack(localMixDown, this.OnError);
                        soundCloud?.OrderAlbumTracks(localMixDown.Album, this.OnError, (progress) => { });
                        soundCloudCache.AddUpdatedAlbum(albums.FirstOrDefault(x => x.AlbumName == localMixDown.Album));
                    }
                }
                else
                {
                    this.Log("Cannot connect to sound cloud");
                    return;
                }
            }
            
            // update any albums 
            if (soundCloudCache.UpdatedAlbums.Count > 0)
            {
                foreach (var updatedAlbum in soundCloudCache.UpdatedAlbums)
                {
                    var albumConfig = this.albumService.GetAlbumConfigurationFromAlbumLocation(updatedAlbum);
                    var album = soundCloudCache.Albums.GetAlbum(albumConfig.Title);
                    if (album != null)
                    {
                        var soundCloudAlbumUpdated = soundCloud.UpdateAlbum(album, albumConfig, soundCloud.CreateAlbumComments(albumConfig, albumConfig.DistributionMixes), OnError);
                        if (!soundCloudAlbumUpdated)
                        {
                            this.Log($"Could not updated album metadata. see previous error");
                        }
                    }
                }
                // and now update the cache - otherwise we will be updating multiple times 
                soundCloudCache.CreateCache(soundCloud, OnError);
            }

            bool Connect()
            {
                if (!soundCloud.Connected)
                {
                    var connected = soundCloud.Connect(this.OnError);
                    if (!connected)
                    {
                        this.Log($"Cannot connect to soundcloud");
                        return false;
                    }
                }
                return true;
            }
        }

        private void OnError(string errorMsg)
        {
            this.Log($"Error: {errorMsg}");
        }

        private void Log(string msg)
        {
            File.AppendAllLines(this.GetLogFileName(), new string[] { $"{DateTime.Now} {msg}" });
        }

        private string GetLogFileName()
        {
            var logFileName = Path.Combine(CubaseHubConstants.LogPath, $"{CubaseHubConstants.DistributionLogFilePrefix}-{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}.txt");
            return logFileName;
        }

    }
}
