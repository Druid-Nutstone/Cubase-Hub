using Cubase.Hub.Services.Album;
using Cubase.Hub.Services.Config;
using Cubase.Hub.Services.Distributers.SoundCloud;
using Cubase.Hub.Services.Models;
using Cubase.Hub.Services.Track;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TagLib.Ape;


namespace Cubase.Hub.Services.Background
{

    public enum BackgroundProcessState
    {
        Run = 0,
        Stop = 1,
        Pause = 2,
        Resume = 3,

    }

    public class BackgroundService : IBackgroundService, IDisposable
    {


        private BackgroundProcessState _state = BackgroundProcessState.Run;

        private readonly IConfigurationService configurationService;

        private readonly IAlbumService albumService;

        private readonly ITrackService trackService;

        private readonly IServiceProvider serviceProvider;

        private readonly ConcurrentQueue<AlbumWatcher> albumQueue = new();
        private readonly HashSet<string> filePending = new(StringComparer.OrdinalIgnoreCase);

        private List<FileSystemWatcher> fileWatchers = new();

        private Dictionary<DistributionProvider, Action<AlbumWatcher>> DistributerMethods;

        private List<AlbumLocation> albumLocations = new();

        public BackgroundService(IAlbumService albumService,
                                 IServiceProvider serviceProvider,
                                 IConfigurationService configurationService,
                                 ITrackService trackService)
        {
            this.configurationService = configurationService;
            this.albumService = albumService;
            this.serviceProvider = serviceProvider;
            this.trackService = trackService;
            this.DistributerMethods = new Dictionary<DistributionProvider, Action<AlbumWatcher>>()
            {
                { DistributionProvider.SoundCloud, this.HandleSoundCloudDistributionUpdate }
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
            this.SetupWatchers();

            Task.Run(() =>
            {
                while (this._state != BackgroundProcessState.Stop)
                {
                    switch (_state)
                    {
                        case BackgroundProcessState.Run:
                            this.RemoveOldLogs();
                            // this.Monitor();
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

        private void SetupWatchers()
        {
            this.albumLocations = this.albumService.GetAlbumList(this.OnError);
            foreach (var albumLoc in this.albumLocations)
            {
                // get all subdirecories 
                var subdirs = Directory.GetDirectories(albumLoc.AlbumPath);
                foreach (var subdir in subdirs)
                {
                    var subdirPath = Path.Combine(subdir, CubaseHubConstants.MixdownDirectory);
                    if (Directory.Exists(subdirPath))
                    {
                        this.Log($"Started monitoring {subdirPath} for changes");
                        var fsw = new FileSystemWatcher(subdirPath)
                        {
                            IncludeSubdirectories = false,
                            NotifyFilter =
                                  NotifyFilters.FileName |
                                  NotifyFilters.LastWrite,
                            InternalBufferSize = 64 * 1024
                        };

                        fsw.Changed += MixChanged;
                        fsw.Created += MixChanged;

                        fsw.EnableRaisingEvents = true;

                        this.fileWatchers.Add(fsw);
                    }
                    else
                    {
                        this.Log($"Cannot find mixdown directory {subdirPath}. This mixdown directory is excluded");
                    }
                }
            }

            Task.Run(ProcessAlbumQueue);
        }

        private void MixChanged(object sender, FileSystemEventArgs e)
        {
            this.Log($"Mix changed {e.FullPath}");
            EnqueueFile(e.FullPath);
        }

        private void EnqueueFile(string fileName)
        {
            lock (filePending)
            {
                if (filePending.Add(fileName))
                {
                    var album = this.FindAlbum(Path.GetDirectoryName(fileName));
                    if (album != null)
                    {
                        albumQueue.Enqueue(new AlbumWatcher() { Album = album, FileName = fileName });
                    }
                }
                else
                {
                    this.Log($"Ignoring file update request for {fileName} because it is already enqueued for processing");
                }

            }
        }

        private void ProcessAlbumQueue()
        {
            while (_state != BackgroundProcessState.Stop)
            {
                if (_state != BackgroundProcessState.Pause)
                {
                    while (albumQueue.TryDequeue(out var albumUpdate))
                    {
                        HandleAlbumChange(albumUpdate);
                        lock (filePending)
                            filePending.Remove(albumUpdate.FileName);

                    }
                    Task.Delay(200).Wait();
                }
                else
                {
                    this.Log($"Background service is paused");
                    Task.Delay(2000).Wait();
                }
            }
        }

        private void HandleAlbumChange(AlbumWatcher albumWatcher)
        {
            this.Log($"Processing change for album {albumWatcher.Album} and track file {Path.GetFileNameWithoutExtension(albumWatcher.FileName)}");
            this.HandleFileMixUpdate(albumWatcher);
            var distroKiddy = this.configurationService?.Configuration?.DistributionConfiguration?.DistributionProvider;
            if (distroKiddy != null && distroKiddy != DistributionProvider.None)
            {
                if (this.DistributerMethods.ContainsKey(distroKiddy.Value))
                {
                    this.DistributerMethods[distroKiddy.Value].Invoke(albumWatcher);
                }
            }
        }

        private void HandleSoundCloudDistributionUpdate(AlbumWatcher albumWatcher)
        {
            if (!SoundCloudTokenResponse.TokenExists())
            {
                this.Log("There is no soundcloud token - wait for ui to logon");
                return;
            }

            var soundCloud = this.serviceProvider.GetService<SoundCloudDistributionProvider>();
            var soundCloudCache = SoundCloudCache.Create();
            if (!soundCloudCache.CacheExists())
            {
                if (soundCloud != null)
                {
                    if (!soundCloud.Connected)
                    {
                        var connected = soundCloud.Connect(this.OnError);
                        if (!connected)
                        {
                            this.Log($"Cannot connect to soundcloud");
                            return;
                        }
                    }
                    if (!soundCloudCache.CreateCache(soundCloud, OnError))
                    {
                        this.Log("Cannot create soundcloud cache");
                        return;
                    }
                }
                else
                {
                    this.Log($"Could not get soundcloud from the service provider");
                    return;
                }

                this.Log($"Upload {albumWatcher.MixDown.Title} to soundcloud..");
                soundCloud?.UploadTrack(albumWatcher.MixDown, this.OnError);
                soundCloud?.OrderAlbumTracks(albumWatcher.Album.Title, this.OnError, (progress) => { });
                var album = soundCloudCache.Albums.GetAlbum(albumWatcher.Album.Title);
                if (album == null)
                {
                    album = soundCloud.CreateAlbum(albumWatcher.Album, GetAlbumComments(), OnError);
                    if (album != null)
                    {
                        soundCloudCache.CreateCache(soundCloud, OnError);
                    }
                    else
                    {
                        this.Log($"Could not create album {albumWatcher.Album.Title}");
                        return;
                    }
                }
                var soundCloudAlbumUpdated = soundCloud.UpdateAlbum(album, albumWatcher.Album, GetAlbumComments(), OnError);
                if (!soundCloudAlbumUpdated)
                {
                    this.Log($"Could not update album metadata. see previous error");
                }
            }
            string GetAlbumComments()
            {
                return soundCloud.CreateAlbumComments(albumWatcher.Album, albumWatcher.Album.DistributionMixes);
            }
        }

        private void HandleFileMixUpdate(AlbumWatcher albumWatcher)
        {
            var track = albumWatcher.FileName;
            var album = albumWatcher.Album;

            var distributionMixdown = album.DistributionMixes.FirstOrDefault(x => x.FileName == track);
            if (distributionMixdown == null)
            {
                this.Log($"Cannot find {Path.GetFileNameWithoutExtension(track)} in the album distribution list");
                return;
            }

            var exportLocation = this.albumService.GetAlbumExportLocationForAlbum(album.Title);

            if (exportLocation == null)
            {
                this.Log($"Cannot get album export location {album.Title}");
                return;
            }

            // get the tags from the new file
            var mixdown = this.trackService.PopulateTagsFromFile(track);
            // and then update it with the detail from the saved album definition 
            mixdown.UpdateFromAnotherMix(distributionMixdown);
            // then save tags back to disk 
            this.trackService.SetTagsFromMixDowm(mixdown);

            // then re-save to album config -- should save to disk as well 
            album.AddForDistribution(mixdown);
            // copy it to export location 
            System.IO.File.Copy(mixdown.FileName, Path.Combine(exportLocation, Path.GetFileName(mixdown.FileName)), true);
            albumWatcher.SetMixDown(mixdown);
        }

        private AlbumConfiguration? FindAlbum(string startDirectory)
        {
            var dir = new DirectoryInfo(startDirectory);

            while (dir != null)
            {
                var file = dir.GetFiles($"*{CubaseHubConstants.CubaseAlbumFileExtension}").FirstOrDefault();
                if (file != null)
                {
                    return AlbumConfiguration.LoadFromFile(file.FullName);
                }
                dir = dir.Parent;
            }

            return null;
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

        private void OnError(string errorMsg)
        {
            this.Log($"Error: {errorMsg}");
        }

        private void Log(string msg)
        {
            System.IO.File.AppendAllLines(this.GetLogFileName(), new string[] { $"{DateTime.Now} {msg}" });
        }

        private string GetLogFileName()
        {
            var logFileName = Path.Combine(CubaseHubConstants.LogPath, $"{CubaseHubConstants.DistributionLogFilePrefix}-{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}.txt");
            return logFileName;
        }

        public void Dispose()
        {
            foreach (var watcher in this.fileWatchers)
            {
                watcher.Dispose();
            }
        }
    }
}
