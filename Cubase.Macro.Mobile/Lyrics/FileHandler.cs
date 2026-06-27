using Cubase.Macro.Common.Models;
using Cubase.Macro.Common.Socket;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Mobile.Lyrics
{
    public class FileHandler
    {
        private CubaseMacroWebSocketClient webSocketClient;
        
        private VerticalStackLayout Container;

        private Action<string> ErrorHandler;

        private LyricIndexCollection Lyrics;

        private LyricViewer lyricViewer;

        public FileHandler(CubaseMacroWebSocketClient webSocketClient)
        {
            this.webSocketClient = webSocketClient;
        }

        public async Task Initialise(VerticalStackLayout container,
                                     LyricViewer lyricViewer,
                                     Action<string> errorHandler)
        {
            this.Container = container;
            this.ErrorHandler = errorHandler;
            this.lyricViewer = lyricViewer;
            this.Lyrics = await this.GetLyricCollection();
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
                    lyricViewer.LoadFile(File.ReadAllLines(ly.FileName.LyricFullPath()));
                };
                this.Container.Children.Add(fileLabel);
            });
        }

        private async Task<LyricIndexCollection?> GetLyricCollection()
        {
            var lyricCollection = new LyricIndexCollection();
            if (!Directory.Exists(CubaseMacroMobileConstants.LyricSourceFolder))
            {
                Directory.CreateDirectory(CubaseMacroMobileConstants.LyricSourceFolder);
            }

            if (!File.Exists(CubaseMacroMobileConstants.LyricCollection))
            {
                if (webSocketClient.Connected)
                {
                    lyricCollection = await GetFromMidi();
                    if (lyricCollection == null)
                    {
                        return lyricCollection;
                    }
                    else
                    {
                        lyricCollection.SerialiseToFile(CubaseMacroMobileConstants.LyricCollection);
                    }
                }
                else
                {
                    ErrorHandler.Invoke($"Cannot load file index because it does not exist and there is no midi connection");
                    return null;
                }
            }

            lyricCollection = LyricIndexCollection.DeserialiseFromFile(CubaseMacroMobileConstants.LyricCollection);

            if (webSocketClient.Connected)
            {
                FileInfo fi = new FileInfo(CubaseMacroMobileConstants.LyricCollection);
                if (fi.LastWriteTimeUtc.Date < DateTime.UtcNow.Date)
                {
                    lyricCollection = await GetFromMidi();
                    lyricCollection.SerialiseToFile(CubaseMacroMobileConstants.LyricCollection);
                }
            }

            // check all files exist locally 
            if (webSocketClient.Connected)
            {
                foreach (var lyric in lyricCollection.Lyrics)
                {
                    if (!File.Exists(lyric.FileName.LyricFullPath()))
                    {
                        if (!await SaveLatestFileContent(lyric))
                        {
                            ErrorHandler.Invoke($"Could not get latest file content for {lyric.FileName}");
                            return lyricCollection;
                        }
                        ;
                    }
                    else
                    {
                        if (File.GetLastWriteTimeUtc(lyric.FileName.LyricFullPath()) < lyric.LastModified)
                        {
                            if (!await SaveLatestFileContent(lyric))
                            {
                                ErrorHandler.Invoke($"Could not get latest file content for {lyric.FileName}");
                                return lyricCollection;
                            }
                        }
                    }
                }
            }

            return lyricCollection;

            async Task<LyricIndexCollection?> GetFromMidi()
            {
                var lyricCollection = await this.webSocketClient.GetLyricIndex(this.ErrorHandler);
                if (lyricCollection != null) 
                {
                    lyricCollection.SerialiseToFile(CubaseMacroMobileConstants.LyricCollection);
                }
                return lyricCollection;
            }

            async Task<bool> SaveLatestFileContent(Lyric lyric)
            {
                var lyricContent = await this.webSocketClient.GetLyricContent(lyric, this.ErrorHandler);
                if (lyricContent != null)
                {
                    File.WriteAllLines(lyric.FileName.LyricFullPath(), lyricContent.Content);
                    return true;
                }
                return false;
            }
        } 

    }
}
