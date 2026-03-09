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
        private static string SoundCloudCacheFileName => "SoundCloudCache.json";

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
            var soundCloud = Path.Combine(CubaseHubConstants.CachePath, SoundCloudCacheFileName);
            if (File.Exists(soundCloud))
            {
                File.Delete(soundCloud);
            }
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
                    Task.Delay(200).Wait();
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
            var cachedLocation = Path.Combine(CubaseHubConstants.CachePath, SoundCloudCacheFileName);
            var soundCloud = this.serviceProvider.GetService<SoundCloudDistributionProvider>();
            if (soundCloud != null)
            {
                if (!CacheExists())
                {
                    if (Connect())
                    {
                        var loadTracks = soundCloud.GetTracks(this.OnError);
                        if (loadTracks == null)
                        {
                            this.Log($"Cannot load any tracks from soundcloud");
                            return;
                        }
                        loadTracks.Save(cachedLocation);
                    }
                    else
                    {
                        return;
                    }
                }

                var allTracks = SoundCloudTrackCollection.LoadFrom(cachedLocation);

                // get all mixes for all albums that have been selected for distribution
                var allAlbumaMixes = new MixDownCollection();
                var albums = this.albumService.GetAlbumList(this.OnError);
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
                        var remoteMixDown = allTracks.FirstOrDefault(x => x.Title == localMixDown.Title);
                        if (remoteMixDown != null)
                        {
                            // local is newer than remote .... upload 
                            if (remoteMixDown.isUploadDateOlderThan(localMixDown.LastModified))
                            {
                                this.Log($"Upload {localMixDown.Title} to soundcloud..");
                                soundCloud.UploadTrack(localMixDown, this.OnError);
                            }
                        }
                        else
                        {
                            // not upload yet - so have to upload it 
                            this.Log($"Upload New track {localMixDown.Title} to soundcloud..");
                            soundCloud.UploadTrack(localMixDown, this.OnError);
                        }
                    }
                }
            }
            else
            {
                this.Log($"Cannot find soundCloud provider");
            }

            bool CacheExists()
            {
                return File.Exists(cachedLocation);
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
