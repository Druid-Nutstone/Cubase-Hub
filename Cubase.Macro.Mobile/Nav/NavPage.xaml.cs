using Cubase.Macro.Common.Socket;
using Cubase.Macro.Mobile.Lyrics;

namespace Cubase.Macro.Mobile.Nav;

public partial class NavPage : ContentPage
{
	private readonly CubaseMacroWebSocketClient webSocket;
	private readonly FileHandler fileHandler;

    public NavPage(CubaseMacroWebSocketClient webSocket, FileHandler fileHandler)
	{
		InitializeComponent();
		this.webSocket = webSocket;
		this.fileHandler = fileHandler;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.webSocket.Connect(CubaseMacroMobileConstants.TargetIPAddress, (s) => { });

		if (!this.webSocket.Connected)
		{
			// get any updates !
			await this.fileHandler.CheckForFileUpdates();
			// route directly to lyrics 
			await Shell.Current.GoToAsync("LyricViewer");
		}
		Message.Text = "Midi Connected.";
		this.MidiCommand.IsEnabled = true;
		this.Lyrics.IsEnabled = true;
		this.MidiCommand.Clicked += async (s, e) => 
		{
			await Shell.Current.GoToAsync("MainPage");
		};

		this.Lyrics.Clicked += async (s, e) =>
		{
			await Shell.Current.GoToAsync("LyricViewer");
		};
    }
}