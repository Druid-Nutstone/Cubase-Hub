using Cubase.Macro.Common.Socket;

namespace Cubase.Macro.Mobile
{
    public partial class AppShell : Shell
    {
        private readonly CubaseMacroWebSocketClient socketClient;

        private readonly MainPage mainPage;

        public AppShell(CubaseMacroWebSocketClient socketClient, MainPage mainPage)
        {
            InitializeComponent();
            this.socketClient = socketClient;
            this.mainPage = mainPage;
            this.TopMenu.Clear();
            this.TopButton("Close", this.CloseClicked);
            this.TopButton("Refresh", this.RefrehClicked);
        }

        private async void CloseClicked()
        {
            await this.socketClient.Close();
            Application.Current.Quit();
        }


        private async void RefrehClicked()
        {
            await this.mainPage.RefreshMacros();
        }

        private void TopButton(string text, Action onClicked)
        {
            var newButton = new Button()
            {
                Text = text,
                HeightRequest = 40,
                BackgroundColor = Color.FromArgb("#473C3B")
            }; 
            newButton.Clicked += (o, e) => { onClicked.Invoke(); };
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
        }
    }
}
