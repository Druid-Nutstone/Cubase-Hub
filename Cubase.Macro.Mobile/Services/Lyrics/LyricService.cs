using Cubase.Macro.Common.Models;
using Cubase.Macro.Common.Models.Lyrics;
using Cubase.Macro.Mobile.Configuration;
using Nutstone.Server.Common.Client;
using Nutstone.Server.Common.Models;
using Plugin.Maui.Audio;
using System.Diagnostics;

namespace Cubase.Macro.Mobile.Services.Lyrics
{
    public class LyricService : ILyricService
    {
        public LyricContainer CurrentLyric { get; set; }

        private readonly IMobileConfigurationService configurationService;

        private readonly IAudioManager audioManager;

        private IAudioPlayer audioPlayer;

        private NutstoneFilesClient client;

        public LyricService(IMobileConfigurationService configurationService, IAudioManager audioManager)
        {
            configurationService.InitialiseConfiguration();
            this.configurationService = configurationService;
            this.audioManager = audioManager;
            this.client = new NutstoneFilesClient(configurationService.Configuration.NutstoneServer, configurationService.Configuration.SecurityKeys);
        }

        public async Task CheckForFileUpdates(Action<string> onError)
        {
            if (!Directory.Exists(CubaseMacroMobileConstants.LyricSourceFolder))
            {
                Directory.CreateDirectory(CubaseMacroMobileConstants.LyricSourceFolder);
            }

            if (!Directory.Exists(CubaseMacroMobileConstants.SetlistSourceFolder))
            {
                Directory.CreateDirectory(CubaseMacroMobileConstants.SetlistSourceFolder);
            }

            // using nutstone server to get the files 

            var availableLyrics = await this.client.GetFileIndex(this.configurationService.Configuration.LyricDirectory, (err) =>
            {
                onError?.Invoke(err.Message);
            });

            if (availableLyrics != null)
            {
                foreach (var lyric in availableLyrics.Files)
                {
                    var localFileVersion = Path.Combine(CubaseMacroMobileConstants.LyricSourceFolder, lyric.Name);

                    if (!File.Exists(localFileVersion))
                    {
                        await SaveLatestFileContent(lyric, CubaseMacroMobileConstants.LyricSourceFolder, onError);
                    }
                    else
                    {
                        var localFile = Path.Combine(CubaseMacroMobileConstants.LyricSourceFolder, lyric.Name);
                        if (File.GetLastWriteTimeUtc(localFile) < lyric.LastModifiedFileDate)
                        {
                            var saveCustomOptions = LyricContainer.Load(localFile, (rr) => { });
                            await SaveLatestFileContent(lyric, CubaseMacroMobileConstants.LyricSourceFolder, onError);
                            var newFileContent = LyricContainer.Load(localFile, (rr) => { });
                            newFileContent.CustomOptions = saveCustomOptions.CustomOptions;
                            newFileContent.Save(localFile, (err) => { });
                        }
                    }
                }
            }

            var availableSetLists = await this.client.GetFileIndex(this.configurationService.Configuration.SetListDirectory, (err) =>
            {
                onError?.Invoke(err.Message);
            });

            if (availableSetLists != null)
            {
                foreach (var setList in availableSetLists.Files)
                {
                    var localSetlistVersion = Path.Combine(CubaseMacroMobileConstants.SetlistSourceFolder, setList.Name);

                    if (!File.Exists(localSetlistVersion))
                    {
                        await SaveLatestFileContent(setList, CubaseMacroMobileConstants.SetlistSourceFolder, onError);
                    }
                    else
                    {
                        var localFile = Path.Combine(CubaseMacroMobileConstants.SetlistSourceFolder, setList.Name);
                        if (File.GetLastWriteTimeUtc(localFile) < setList.LastModifiedFileDate)
                        {
                            await SaveLatestFileContent(setList, CubaseMacroMobileConstants.SetlistSourceFolder, onError);
                        }
                    }
                }
            }

            async Task<bool> SaveLatestFileContent(FileModel lyric, string sourceFolder, Action<string> onError)
            {
                var targetFile = Path.Combine(sourceFolder, lyric.Name);

                var lyricContentDownloaded = await this.client.DownloadFile(lyric.Id, targetFile, (err) => { onError?.Invoke(err.Message); });

                return lyricContentDownloaded;
            }
        }

        public async Task<string?> LoadProjectLyricIfAvailable(string projectName)
        {
            if (projectName == null) return null;
            return Directory.GetFiles(CubaseMacroMobileConstants.LyricSourceFolder, $"*{CubaseMacroConstants.NutstoneLyricNotation}")
                     .FirstOrDefault(x => Path.GetFileNameWithoutExtension(x).Equals(projectName, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<List<string>> GetLyricFiles()
        {
            return Directory.GetFiles(CubaseMacroMobileConstants.LyricSourceFolder, $"*{CubaseMacroConstants.NutstoneLyricNotation}").ToList();
        }

        public async Task<List<string>> GetSetlists()
        {
            return Directory.GetFiles(CubaseMacroMobileConstants.SetlistSourceFolder, $"*{CubaseMacroConstants.NutstoneSetListNotation}").ToList();
        }

        public async Task<FileResult?> PickAudioFileAsync()
        {
            var customAudioTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { "audio/*" } },
                    { DevicePlatform.WinUI, new[] { ".mp3", ".wav", ".m4a", ".aac" } },
                });

            var options = new PickOptions
            {
                PickerTitle = "Select an audio file",
                FileTypes = customAudioTypes
            };

            try
            {

                var result = await FilePicker.Default.PickAsync(options);
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return null;
        }

        public async Task<LyricContainer?> LoadSetListLyric(SetListSong setListSong)
        {
            var targetFile = Path.Combine(CubaseMacroMobileConstants.LyricSourceFolder, setListSong.FileName);
            if (File.Exists(targetFile))
            {
                return LyricContainer.Load(targetFile, (err) => { });
            }
            return null;
        }

        public async Task StartAudio(LyricContainer lyricContainer)
        {
            if (File.Exists(lyricContainer.CustomOptions.LyricAudioPath))
            {
                this.audioPlayer = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(lyricContainer.CustomOptions.LyricAudioPath));
                this.audioPlayer.Play();
            }
        }

        public async Task StopAudio()
        {
            if (this.audioPlayer != null)
            {
                this.audioPlayer.Stop();
                this.audioPlayer.Dispose();
                this.audioPlayer = null;
            }
        }

        public void SaveLyric(LyricContainer lyricContainer)
        {
            lyricContainer.Save(CubaseMacroMobileConstants.LyricSourceFolder, (err) => { });
        }
    }
}
