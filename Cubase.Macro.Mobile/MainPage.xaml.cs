using Cubase.Macro.Common.Models;
using Cubase.Macro.Common.Socket;
using Cubase.Macro.Mobile.Controls;

namespace Cubase.Macro.Mobile
{
    public partial class MainPage : ContentPage
    {
        private readonly CubaseMacroWebSocketClient client;

        public MainPage(CubaseMacroWebSocketClient cubaseMacroWebSocketClient)
        {
            this.BackgroundColor = Color.FromRgba("#1E1E1E");
            this.client  = cubaseMacroWebSocketClient;
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
            await this.client.Close();
            this.client.Dispose();
        }

        private async Task LoadMacros()
        {
            var connected = await this.client.Connect(CubaseMacroMobileConstants.TargetIPAddress,
                async (msg) => 
                {
                    await DisplayAlertAsync("Oops", $"Cannot Connect to {CubaseMacroMobileConstants.TargetIPAddress} error: {msg}", "OK");
                }
            );
            if (!connected)
            {
                return;
            }

            // load all macros 
            var macros = await this.client.GetMacroCollection();
        
            foreach (var macro in macros)
            {
                var button = new MacroButton(macro);

                button.OnMacroClicked = this.ButtonClicked;

                this.CollectionsLayout.Add(button);
            } 

        }

        private async void ButtonClicked(CubaseMacro cubaseMacro, bool toggled)
        {
            var midiCommands = cubaseMacro.ToggleOffKeys;
            if (toggled)
            {
                midiCommands = cubaseMacro.ToggleOnKeys;
            }

            foreach (var midiCommand in midiCommands)
            {
                var response = await this.client.SendMidiCommand(midiCommand);
                if (response.Command == WebSocketMidiCommand.Error)
                {
                    await DisplayAlertAsync("Oops", $"Cannot Send Midi Command {midiCommand.Name}", "OK");
                    break;
                }

            }        
        } 
    }
}
