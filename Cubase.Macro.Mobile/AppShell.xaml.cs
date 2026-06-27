using Cubase.Macro.Common.Socket;
using Cubase.Macro.Mobile.Lyrics;

namespace Cubase.Macro.Mobile
{
    public partial class AppShell : Shell
    {
        private readonly CubaseMacroWebSocketClient socketClient;

        private readonly MainPage mainPage;

        private readonly LyricViewer lyricPage;

        private string startAutoScroll = "Start Auto Scroll";
        private string stopAutoScroll = "Stop Auto Scroll";

        private string showChords = "Show Chords";
        private string hideChords = "Hide Chords";

        private Color defaultButtonBackgroundColour = Color.FromArgb("#473C3B");

        public AppShell(CubaseMacroWebSocketClient socketClient, MainPage mainPage, LyricViewer lyricViewer)
        {
            InitializeComponent();
            Routing.RegisterRoute("LyricViewer", typeof(LyricViewer));
            Routing.RegisterRoute("MainPage", typeof(MainPage));
            
            this.socketClient = socketClient;
            this.mainPage = mainPage;
            this.lyricPage = lyricViewer;
            this.SetPrimaryScreenTopMenu();
            TopMenu.BackgroundColor = Colors.Black;
        }

        private async void CloseClicked(Button button)
        {
            await this.socketClient.Close();
            Application.Current.Quit();
        }

        private async void SetPrimaryScreenTopMenu()
        {
            this.TopMenu.Clear();
            this.TopButton("Close", LayoutOptions.Start, this.CloseClicked);
            this.TopButton("Refresh", LayoutOptions.Start, this.RefrehClicked);
            this.TopButton("Open Lyrics", LayoutOptions.End, this.OpenLyricMenu);
        } 

        private async void SetLyricTopMenu()
        {
            this.TopMenu.Clear();
            this.TopButton("Files >>", LayoutOptions.Start, this.ShowFileMenu);
            this.TopButton("Back To Main", LayoutOptions.Start, this.OpenMainScreen);
            this.TopButton("Font +", LayoutOptions.Start, this.IncreaseFontSize);
            this.TopButton("Font -", LayoutOptions.Start, this.DecreaseFontSize);
            var showHideChords = this.TopButton(this.hideChords, LayoutOptions.Start, this.ShowHideChords);
            showHideChords.BackgroundColor = Colors.DarkSalmon;
            var startStopButton = this.TopButton(startAutoScroll, LayoutOptions.Start, this.StartStop);
            startStopButton.BackgroundColor = Colors.Green;
        }

        
        private async void ShowFileMenu(Button button)
        {
            this.lyricPage.ShowFiles();
        }
        
        private async void ShowHideChords(Button button)
        {
            if (button.Text == hideChords)
            {
                button.BackgroundColor = defaultButtonBackgroundColour;
                button.Text = showChords;
                this.lyricPage.ShowHideChords(false);
            }
            else
            {
                button.BackgroundColor = Colors.DarkSalmon;
                button.Text = hideChords;
                this.lyricPage.ShowHideChords(true);
            }
        } 

        private async void StartStop(Button button)
        {
            if (button.Text == startAutoScroll)
            {
                button.BackgroundColor = Colors.Red;
                button.Text = stopAutoScroll;
                this.lyricPage.StartAutoScroll();
            }
            else
            {
                this.lyricPage.StopAutoScroll();
                button.BackgroundColor = Colors.Green;
                button.Text = startAutoScroll;
            }
        }

        private async void IncreaseFontSize(Button button)
        {
            this.lyricPage.IncreaseFontSize();
        }
        private async void DecreaseFontSize(Button button)
        {
            this.lyricPage.DecreaseFontSize();
        }


        private async void OpenMainScreen(Button button)
        {
            this.SetPrimaryScreenTopMenu();
            await Shell.Current.GoToAsync("//MainPage");
        }

        private async void RefrehClicked(Button button)          
        {
            await this.mainPage.RefreshMacros();
        }

        private async void OpenLyricMenu(Button button)
        {
            this.SetLyricTopMenu();
            await Shell.Current.GoToAsync("LyricViewer");
        }

        public async void OpenLyrics()
        {
            this.SetLyricTopMenu();
            await Shell.Current.GoToAsync("LyricViewer");
        }

        private Button TopButton(string text, LayoutOptions location, Action<Button> onClicked)
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
            newButton.Clicked += (o, e) => { onClicked.Invoke(newButton); };
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
            this.TopMenu.Add(newButton);
            return newButton;
        }
    }
}
