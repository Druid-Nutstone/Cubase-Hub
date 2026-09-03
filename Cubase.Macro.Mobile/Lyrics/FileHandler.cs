using Cubase.Macro.Common.Models;
using Cubase.Macro.Common.Socket;
using Cubase.Macro.Mobile.Configuration;
using Nutstone.Server.Common.Client;
using Nutstone.Server.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Mobile.Lyrics
{
    public class FileHandler
    {
        private CubaseMacroWebSocketClient webSocketClient;

        private readonly IMobileConfigurationService mobileConfigurationService;

        private VerticalStackLayout Container;

        private Action<string> ErrorHandler;

        private LyricIndexCollection Lyrics;

        private LyricViewer lyricViewer;

        private NutstoneFilesClient lyricsFilesClient;

        public FileHandler(CubaseMacroWebSocketClient webSocketClient, 
                           IMobileConfigurationService mobileConfigurationService)
        {
            mobileConfigurationService.InitialiseConfiguration();
            this.webSocketClient = webSocketClient;
            this.mobileConfigurationService = mobileConfigurationService;
            this.lyricsFilesClient = new NutstoneFilesClient(mobileConfigurationService.Configuration.NutstoneServer, mobileConfigurationService.Configuration.SecurityKeys);    
        }

        public async Task Initialise(VerticalStackLayout container,
                                     LyricViewer lyricViewer,
                                     Action<string> errorHandler)
        {
            this.Container = container;
            this.ErrorHandler = errorHandler;
            this.lyricViewer = lyricViewer;
            this.Lyrics = await this.GetLyricCollection(errorHandler);
            if (this.Lyrics == null) return;
            await this.BuildScreen();
        }

        private async Task BuildScreen()
        {
            this.Container.Children.Clear();
            this.Lyrics.Lyrics.ForEach((ly) =>
            {
                var fileLabel = new Button()
                {
                    Text = ly.TrackName,
                    TextTransform = TextTransform.Uppercase,
                    HorizontalOptions = LayoutOptions.Start,
                    BackgroundColor = CubaseMacroMobileConstants.DefaultBackgroundColour,
                    TextColor = Colors.White,
                };
                fileLabel.Clicked += (s, e) =>
                {
                    lyricViewer.LoadFile(LyricContainer.Load(ly.FileName.LyricFullPath(), (err) => { }));
                };
                this.Container.Children.Add(fileLabel);
            });
        }



        public async Task CheckForFileUpdates(Action<string> onError)
        {
            if (!Directory.Exists(CubaseMacroMobileConstants.LyricSourceFolder))
            {
                Directory.CreateDirectory(CubaseMacroMobileConstants.LyricSourceFolder);
            }

            // using nutstone server to get the files 

            var availableLyrics = await this.lyricsFilesClient.GetFileIndex(this.mobileConfigurationService.Configuration.LyricDirectory, (err) => 
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
                        await SaveLatestFileContent(lyric, onError);

                    }
                    else
                    {
                        if (File.GetLastWriteTimeUtc(lyric.Name.LyricFullPath()) < lyric.LastModifiedFileDate)
                        {
                            await SaveLatestFileContent(lyric, onError);
                        }
                    }
                }
                var lyricIndex = new LyricIndexCollection();
                lyricIndex.PopulateLyricFiles(CubaseMacroMobileConstants.LyricSourceFolder);
                lyricIndex.SerialiseToFile(CubaseMacroMobileConstants.LyricCollection);
            }

            async Task<bool> SaveLatestFileContent(FileModel lyric, Action<string> onError)
            {
                var targetFile = Path.Combine(CubaseMacroMobileConstants.LyricSourceFolder, lyric.Name);

                var lyricContentDownloaded = await this.lyricsFilesClient.DownloadFile(lyric.Id, targetFile, (err) => { onError?.Invoke(err.Message); });

                return lyricContentDownloaded;
            }
        }

        private async Task<LyricIndexCollection?> GetLyricCollection(Action<string> messageHandler)
        {

            if (!File.Exists(CubaseMacroMobileConstants.LyricCollection)) 
            {
                messageHandler($"There are no lyrics in {CubaseMacroMobileConstants.BaseFolder}. Enable the midi connection and restart this app");
                return null; 
            }

            return LyricIndexCollection.DeserialiseFromFile(CubaseMacroMobileConstants.LyricCollection);
        }

    }
}
