using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Cubase.Macro.Mobile.Lyrics
{
    public class MenuHandler
    {
        private readonly HorizontalStackLayout container;

        private readonly LyricViewer viewer;

        private List<MenuButton> menuButtons = new List<MenuButton>();

        private Color defaultButtonBackgroundColour = Color.FromArgb("#473C3B");

        public MenuHandler(HorizontalStackLayout container, LyricViewer viewer)
        {
            this.container = container;
            this.viewer = viewer;
            menuButtons.Clear();
        }

        public async Task ResetScroll()
        {
            this.menuButtons.FirstOrDefault(m => m.ButtonName == KnownMenuButton.StartStopScroll)?.MakeInActive();  
        }

        public async Task SetLyricButtonSelected()
        {
            this.menuButtons.First().MakeActive();
        }

        public async Task SetLyricButtonUnSelected()
        {
            this.menuButtons.First().MakeInActive();
        }

        public async Task DisableButtons()
        {
            foreach (var mitem in menuButtons.Skip(1))
            {
                mitem.MenuItem.IsEnabled = false;
            }
        }

        public async Task EnableButtons()
        {
            foreach (var mitem in menuButtons.Skip(1))
            {
                mitem.MenuItem.IsEnabled = true;
            }
        }

        public async Task BuildMenu()
        {
            this.container.Children.Clear();

            var lyricButton = MenuButton.Create("Hide Lyrics", "Show Lyrics", Colors.Green, defaultButtonBackgroundColour, async (btn) => 
            { 
                if (btn.Pressed)
                {
                    await this.viewer.ShowFiles();
                }
                else
                {
                    await this.viewer.CloseFiles();
                }
            }, KnownMenuButton.LyricButton);
            this.container.Children.Add(lyricButton.MenuItem);
            this.menuButtons.Add(lyricButton);

            var fontPlus = MenuButton.Create("Font +", "Font +", defaultButtonBackgroundColour, defaultButtonBackgroundColour, async (btn) =>
            {
                await this.viewer.IncreaseFontSize();
            }, KnownMenuButton.FontPlus);
            this.container.Children.Add(fontPlus.MenuItem);
            this.menuButtons.Add(fontPlus);

            var fontMinus = MenuButton.Create("Font -", "Font -", defaultButtonBackgroundColour, defaultButtonBackgroundColour, async (btn) => 
            {
                await this.viewer.DecreaseFontSize();
            }, KnownMenuButton.FontMinus);
            this.container.Children.Add(fontMinus.MenuItem);
            this.menuButtons.Add(fontMinus);

            var showHideChords = MenuButton.Create("Hide Chords", "Show Chords", Colors.Orange, defaultButtonBackgroundColour, async (btn) => 
            { 
                 await this.viewer.ShowHideChords(btn.Pressed);
            }, KnownMenuButton.ShowHideChords);
            this.container.Children.Add(showHideChords.MenuItem);
            this.menuButtons.Add(showHideChords);

            var startStopScroll = MenuButton.Create("Stop Auto Scroll", "Start Auto Scroll", Colors.DarkSalmon, defaultButtonBackgroundColour, async (btn) => 
            { 
                if (btn.Pressed)
                {
                    await this.viewer.StartAutoScroll();
                }
                else
                {
                    await this.viewer.StopAutoScroll();
                }
            }, KnownMenuButton.StartStopScroll);
            this.container.Children.Add(startStopScroll.MenuItem);
            this.menuButtons.Add(startStopScroll);

        }

    }

    public class MenuButton
    {
        public Button MenuItem { get; set; }

        public string ActiveText { get; set; }

        public string InActiveText { get; set; }

        public Color ActiveColour {  get; set; } 
    
        public Color InActiveColour { get; set; }

        public bool Pressed { get; set; } = false;

        public KnownMenuButton ButtonName { get; set; }

        public Func<MenuButton, Task> OnClicked { get; set; }
        
        
        public void MakeActive()
        {
            this.Pressed = true;
            this.MenuItem.BackgroundColor = this.ActiveColour;
            this.MenuItem.Text = this.ActiveText;
        }

        public void MakeInActive()
        {
            this.Pressed = false;
            this.MenuItem.BackgroundColor = this.InActiveColour;
            this.MenuItem.Text = this.InActiveText;
        }


        public static MenuButton Create(string activeText, string inactiveText, Color activeColour, Color inActiveColour, Func<MenuButton, Task> onClicked, KnownMenuButton buttonName)
        {
            var newButton = new Button()
            {
                Text = inactiveText,
                HeightRequest = 40,
                MinimumWidthRequest = 110,
                CornerRadius = 0,               // Make corners square
                HorizontalOptions = LayoutOptions.Start,
                BackgroundColor = inActiveColour
            };
            newButton.Pressed += async (o, e) =>
            {
                if (newButton.IsEnabled)
                {
                    await newButton.ScaleToAsync(0.9, 100);
                }
            };
            newButton.Released += async (o, e) =>
            {
                if (newButton != null)
                {
                    if (newButton.IsEnabled)
                    {
                        await newButton.ScaleToAsync(1, 100);
                    }
                }
            };

            var newMenuButton = new MenuButton()
            {
                MenuItem = newButton,
                ActiveText = activeText,
                InActiveText = inactiveText,
                ActiveColour = activeColour,
                InActiveColour = inActiveColour,
                OnClicked = onClicked,
                ButtonName = buttonName
            };

            newButton.Clicked += async (o, e) =>
            {
                if (newButton.IsEnabled)
                {
                    _ = newMenuButton.Pressed = !newMenuButton.Pressed;
                    newButton.BackgroundColor = newMenuButton.Pressed ? newMenuButton.ActiveColour : newMenuButton.InActiveColour;
                    newButton.Text = newMenuButton.Pressed ? newMenuButton.ActiveText : newMenuButton.InActiveText;
                    await onClicked(newMenuButton);
                }
            };

            return newMenuButton;
        }
    }

    public enum KnownMenuButton
    {
        LyricButton,
        FontPlus,
        FontMinus,
        ShowHideChords,
        StartStopScroll
    }
}
