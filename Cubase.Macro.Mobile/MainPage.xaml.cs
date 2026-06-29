using Cubase.Macro.Common.Models;
using Cubase.Macro.Common.Socket;
using Cubase.Macro.Mobile.Controls;
using Cubase.Macro.Mobile.Lyrics;

namespace Cubase.Macro.Mobile
{
    public partial class MainPage : ContentPage
    {
        private readonly CubaseMacroWebSocketClient client;

        private readonly FileHandler fileHandler;

        public MainPage(CubaseMacroWebSocketClient cubaseMacroWebSocketClient, FileHandler fileHandler)
        {
            this.BackgroundColor = CubaseMacroMobileConstants.DefaultBackgroundColour;
            this.client  = cubaseMacroWebSocketClient;
            this.fileHandler = fileHandler;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadMacros();
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
        }

        public async Task LoadMacros()
        {

            if (this.client.Connected)
            {
                await RefreshMacros();
            }
        }

        private async void ProcessError(string errorMessage)
        {
            await DisplayAlertAsync("Error", errorMessage, "OK");
        }

        public async Task RefreshMacros()
        {
            if (this.client != null && this.client.State == System.Net.WebSockets.WebSocketState.Open)
            {
                this.CollectionsLayout.Clear();

                // load all macros 
                var macros = await this.client.GetMacroCollection(this.ProcessError);
                if (macros != null)
                {
                    foreach (var macro in macros)
                    {
                        var button = new MacroButton(macro);

                        button.OnMacroClicked = this.ButtonClicked;

                        this.CollectionsLayout.Add(button);
                    }
                }
            }
        }

        private async void ButtonClicked(CubaseMacro cubaseMacro, bool toggled)
        {
            var midiCommands = cubaseMacro.ToggleOffKeys;
            if (cubaseMacro.ButtonType == CubaseMacroButtonType.Single)
            {
                midiCommands = cubaseMacro.ToggleOnKeys;
            }
            if (toggled)
            {
                midiCommands = cubaseMacro.ToggleOnKeys;
            }

            foreach (var midiCommand in midiCommands)
            {
                var response = await this.client.SendMidiCommand(midiCommand, this.ProcessError);
                if (response != null)
                {
                    if (response.Command == WebSocketMidiCommand.Error)
                    {
                        await DisplayAlertAsync("Oops", $"Cannot Send Midi Command {midiCommand.Name}", "OK");
                        break;
                    }
                    else
                    {
                        if (midiCommand.ThreadWaitAfterExecutionMs > 0)
                        {
                            Task.Delay(midiCommand.ThreadWaitAfterExecutionMs).Wait();
                        }
                    }
                }
            }        
        } 
    }
}
