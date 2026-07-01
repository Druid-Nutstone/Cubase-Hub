using Cubase.Macro.Common.Socket;
using Cubase.Macro.Mobile.Configuration;
using Cubase.Macro.Mobile.Lyrics;
using System.Net.WebSockets;

namespace Cubase.Macro.Mobile.Nav;

public partial class NavPage : ContentPage
{
	private readonly CubaseMacroWebSocketClient webSocket;
	private readonly FileHandler fileHandler;

	private readonly IMobileConfigurationService configurationService;

    public NavPage(CubaseMacroWebSocketClient webSocket, 
		           FileHandler fileHandler, 
				   IMobileConfigurationService configurationService)
	{
		InitializeComponent();
		this.webSocket = webSocket;
		this.configurationService = configurationService; 
		this.fileHandler = fileHandler;
		MidiConfiguration.Clicked += async (s, e) =>
        {
            await Shell.Current.GoToAsync("ConfigurationPage");
        };
        SetButtonEvents(RetryConnection);
        SetButtonEvents(MidiCommand);
        SetButtonEvents(Lyrics);
        SetButtonEvents(MidiConfiguration);
        RetryConnection.Clicked += async (s, e) =>
        {
            await this.ConnectToMidiServicer();
            if (this.webSocket.Connected)
            {
                await this.ReloadButtons();
            }
        };
        MidiCommand.Clicked += async (s, e) =>
        {
            await Shell.Current.GoToAsync("MainPage");
        };
        Lyrics.Clicked += async (s, e) =>
        {
            await Shell.Current.GoToAsync("LyricViewer");
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await this.configurationService.InitialiseConfiguration();

		await this.ConnectToMidiServicer();	

		if (!this.webSocket.Connected)
		{
            RetryConnection.IsVisible = true;
			RetryConnection.IsEnabled = true;
			await EnableLyrics();
        }
		else
		{
			await this.ReloadButtons();
        }
    }

	private async Task ReloadButtons()
    {
        await this.GetFileUpdates();
        await this.EnableButtons();
		SetMessage("Ready");
    }

    private async Task GetFileUpdates()
	{
        SetMessage("Checking for lyric updates..");
        await this.fileHandler.CheckForFileUpdates();
    }

	private async Task EnableButtons()
	{
        this.MidiCommand.IsEnabled = true;
		await this.EnableLyrics();

    }

	private async Task EnableLyrics()
	{
        this.Lyrics.IsEnabled = true;
    }

	private async Task<bool> ConnectToMidiServicer()
	{
		SetMessage($"Connecting to MidiServicer {this.configurationService.Configuration.MidiServerIpAddress}..");
		if (!this.webSocket.Connected)
		{
			await this.webSocket.Connect(this.configurationService.Configuration.MidiServerIpAddress, (s) => 
			{ 
			    SetMessage($"Failed to connect to MidiServicer: {s}");
            });
			if (this.webSocket.Connected)
			{
				SetMessage($"Connected to MidiServicer {this.configurationService.Configuration.MidiServerIpAddress}");
			}
			else
			{
				SetMessage($"Failed to connect to MidiServicer {this.configurationService.Configuration.MidiServerIpAddress}");
			}
		}
		return this.webSocket.Connected;
    }

	private void SetButtonEvents(Button btn)
	{
        btn.Pressed += async (o, e) =>
        {
            if (btn.IsEnabled)
            {
                await btn.ScaleToAsync(0.9, 100);
            }
        };
        btn.Released += async (o, e) =>
        {
            if (btn != null)
            {
                if (btn.IsEnabled)
                {
                    await btn.ScaleToAsync(1, 100);
                }
            }
        };
    }

    private void SetMessage(string msg)
	{
		Message.Text = msg;
		Message.InvalidateMeasure();
	}
}