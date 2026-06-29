using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Mobile.Lyrics
{
    public class MenuHandler
    {
        private readonly HorizontalStackLayout container;

        private readonly LyricViewer viewer;

        private Color defaultButtonBackgroundColour = Color.FromArgb("#473C3B");

        private Button lyricButton;

        private string showLyrics = "Lyrics";
        private string hideLyrics = "Hide Lyrics"; 

        private string startAutoScroll = "Start Auto Scroll";
        private string stopAutoScroll = "Stop Auto Scroll";

        private string showChords = "Show Chords";
        private string hideChords = "Hide Chords";

        public MenuHandler(HorizontalStackLayout container, LyricViewer viewer)
        {
            this.container = container;
            this.viewer = viewer;
        }

        public async Task SetLyricButtonSelected()
        {
            this.lyricButton.BackgroundColor = Colors.DarkGreen;
            this.lyricButton.Text = hideLyrics;
        }

        public async Task SetLyricButtonUnSelected()
        {
            this.lyricButton.BackgroundColor = defaultButtonBackgroundColour;
            this.lyricButton.Text = showLyrics;
        }

        public async Task BuildMenu()
        {
            
            this.container.Children.Clear();
            
            this.lyricButton = await this.MenuButton(showLyrics, LayoutOptions.Start, async (btn) =>
            {
                switch (btn.Text)
                {
                    case "Lyrics":
                        btn.Text = this.hideLyrics;
                        btn.BackgroundColor = Colors.Green;
                        await this.viewer.ShowFiles();

                        break;
                    case "Hide Lyrics":
                        btn.Text = this.showLyrics;
                        btn.BackgroundColor = defaultButtonBackgroundColour;
                        await this.viewer.CloseFiles();
                        break;
                }
            });
            await this.MenuButton("Functions", LayoutOptions.Start, async (btn) =>
            {
                await Shell.Current.GoToAsync("//MainPage");
            });
            await this.MenuButton("Font +", LayoutOptions.Start, async (btn) =>
            {
                await this.viewer.IncreaseFontSize();
            });
            await this.MenuButton("Font -", LayoutOptions.Start, async (btn) =>
            {
                await this.viewer.DecreaseFontSize();
            });
            await this.MenuButton(this.hideChords, LayoutOptions.Start, async (btn) =>
            {
                if (btn.Text == hideChords)
                {
                    btn.BackgroundColor = defaultButtonBackgroundColour;
                    btn.Text = showChords;
                    await this.viewer.ShowHideChords(false);
                }
                else
                {
                    btn.BackgroundColor = Colors.DarkSalmon;
                    btn.Text = hideChords;
                    await this.viewer.ShowHideChords(true);
                }
            }, () => { return Colors.DarkSalmon; });
            
            await this.MenuButton(startAutoScroll, LayoutOptions.Start, async (btn) =>
            {
                if (btn.Text == startAutoScroll)
                {
                    btn.BackgroundColor = Colors.Red;
                    btn.Text = stopAutoScroll;
                    await this.viewer.StartAutoScroll();
                }
                else
                {
                    await this.viewer.StopAutoScroll();
                    btn.BackgroundColor = Colors.Green;
                    btn.Text = startAutoScroll;
                }
            }, () => { return Colors.Green; });
        }

        private async Task<Button> MenuButton(string text, LayoutOptions location, Func<Button, Task> onClicked, Func<Color>? onBackgroundColour = null)
        {
            var newButton = new Button()
            {
                Text = text,
                HeightRequest = 40,
                MinimumWidthRequest = 110,
                CornerRadius = 0,               // Make corners square
                HorizontalOptions = location,
                BackgroundColor = defaultButtonBackgroundColour
            };
            
            if (onBackgroundColour != null)
            {
                newButton.BackgroundColor = onBackgroundColour.Invoke();
            }
            
            
            newButton.Clicked += async (o, e) =>
            {
                await onClicked.Invoke(newButton);
            };
            newButton.Pressed += async (o, e) =>
            {
                await newButton.ScaleToAsync(0.9, 100);
            };
            newButton.Released += async (o, e) =>
            {
                if (newButton != null)
                {
                    await newButton.ScaleToAsync(1, 100);
                }
            };
            this.container.Add(newButton);
            return newButton;
        }
    }
}
