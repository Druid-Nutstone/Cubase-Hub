namespace Cubase.Macro.Mobile.Logging;

public partial class LoggingPage : ContentPage
{
   
    public LoggingPage()
	{
		InitializeComponent();
    }

    private async Task InitialiseViewer()
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var logFiles = Directory.GetFiles(CubaseMacroMobileConstants.LogFilePath, "*.txt")
                .Select(f => new FileInfo(f))
                .OrderByDescending(f => f.LastWriteTime)
                .ToList();
            foreach (var file in logFiles)
            {
                var button = new Button
                {
                    Text = file.Name,
                    HorizontalOptions = LayoutOptions.Fill,
                    BackgroundColor = Colors.LightGray,
                    TextColor = Colors.Black
                };
                button.Clicked += async (s, e) =>
                {
                    await ViewFileAsync(file.FullName);
                };
                LogFileList.Children.Add(button);
            }
        });
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await InitialiseViewer();
    }

    public static async Task ViewFileAsync(string path)
    {
        var request = new OpenFileRequest
        {
            File = new ReadOnlyFile(path)
        };

        await Launcher.Default.OpenAsync(request);
    }
}