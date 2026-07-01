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
                    lyricViewer.LoadFile(File.ReadAllLines(ly.FileName.LyricFullPath()));
                };
                this.Container.Children.Add(fileLabel);
            });
        }

        public async Task CheckForFileUpdates()
        {
            if (!Directory.Exists(CubaseMacroMobileConstants.LyricSourceFolder))
            {
                Directory.CreateDirectory(CubaseMacroMobileConstants.LyricSourceFolder);
            }
            if (this.webSocketClient.Connected)
            {
                var lyricCollection = await this.webSocketClient.GetLyricIndex(this.ErrorHandler);

                if (lyricCollection != null)
                {
                    lyricCollection.SerialiseToFile(CubaseMacroMobileConstants.LyricCollection);

                    // get or update any existing files 
                    foreach (var lyric in lyricCollection.Lyrics)
                    {
                        if (!File.Exists(lyric.FileName.LyricFullPath()))
                        {
                            await SaveLatestFileContent(lyric);
                        }
                        else
                        {
                            if (File.GetLastWriteTimeUtc(lyric.FileName.LyricFullPath()) < lyric.LastModified)
                            {
                                await SaveLatestFileContent(lyric);
                            }
                        }
                    }
                }
            }
            async Task SaveLatestFileContent(Lyric lyric)
            {
                var lyricContent = await this.webSocketClient.GetLyricContent(lyric, (err) => { });
                if (lyricContent != null)
                {
                    File.WriteAllLines(lyric.FileName.LyricFullPath(), lyricContent.Content);
                }
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
