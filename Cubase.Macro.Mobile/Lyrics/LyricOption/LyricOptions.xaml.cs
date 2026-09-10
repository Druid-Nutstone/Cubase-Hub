using Cubase.Macro.Common.Models.Lyrics;
using Cubase.Macro.Mobile.Controls;
using Cubase.Macro.Mobile.Services.Lyrics;

namespace Cubase.Macro.Mobile.Lyrics.LyricOption;

public partial class LyricOptions : ContentPage
{
    private readonly ILyricService lyricService;

    private Color DefaultBackgroundColour = Color.FromArgb("#2e2e2e");

    private LyricContainer currentLyric;

    public LyricOptions(ILyricService lyricService)
    {
        InitializeComponent();
        this.lyricService = lyricService;
        this.currentLyric = this.lyricService.CurrentLyric;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        this.AudioFile.Children.Clear();

        var grid = this.Create2ColumnGrid();


        // AUDIO file
        grid.AddHeader(new Label() { Text = "Audio File To Play", TextColor = Colors.White });
        var headerAudioPath = new BoundYamlTextBox()
        {
            Text = this.currentLyric.CustomOptions.LyricAudioPath,
            PlaceholderColor = Colors.LightGray,
            TextColor = Colors.White,
            BackgroundColor = DefaultBackgroundColour
        };
        headerAudioPath.Bind(nameof(LyricCustomOptions.LyricAudioFile), this.currentLyric.CustomOptions);
        var editAudioFileButton = new BaseImageButton()
        {
            Source = "edit.png",
            BackgroundColor = Color.FromArgb("#2e2e2e"),
            HeightRequest = 32,
            WidthRequest = 32,
            HorizontalOptions = LayoutOptions.End
        };
        editAudioFileButton.Clicked += async (s, e) =>
        {
            var result = await this.lyricService.PickAudioFileAsync();
            this.currentLyric.CustomOptions.LyricAudioPath = result?.FullPath;
            this.currentLyric.CustomOptions.LyricAudioFile = Path.GetFileName(result?.FullPath);
            this.SaveLyricOptions();
        };
        grid.AddRow(0, headerAudioPath, editAudioFileButton);

        // PLAY AUDIO DURING SCROLL YES/NO
        grid.AddHeader(new Label() { Text = "Play audio when scrolling lyric", TextColor = Colors.White });
        var playAudioYesNo = new BoundYamlSwitch();
        playAudioYesNo.BackgroundColor = DefaultBackgroundColour;
        playAudioYesNo.VerticalOptions = LayoutOptions.Center;
        playAudioYesNo.Bind(nameof(LyricCustomOptions.PlayAudioWithScroll), this.currentLyric.CustomOptions);
        playAudioYesNo.Toggled += (s, e) =>
        {
            this.SaveLyricOptions();
        };
        grid.AddRow(0, playAudioYesNo, null);

        // Show Chords 
        grid.AddHeader(new Label() { Text = "Show Chords in lyrics", TextColor = Colors.White });
        var showChordsYesNo = new BoundYamlSwitch();
        showChordsYesNo.BackgroundColor = DefaultBackgroundColour;
        showChordsYesNo.VerticalOptions = LayoutOptions.Center;
        showChordsYesNo.Bind(nameof(LyricCustomOptions.ShowChords), this.currentLyric.CustomOptions);
        showChordsYesNo.Toggled += (s, e) =>
        {
            this.SaveLyricOptions();
        };
        grid.AddRow(0, showChordsYesNo, null);


        // BAR OFFSET 
        grid.AddHeader(new Label() { Text = "Bar offset for playback audio", TextColor = Colors.White });
        var offset = new BoundYamlInteger();
        offset.Bind(nameof(LyricCustomOptions.BarOffset), this.currentLyric.CustomOptions);
        offset.BackgroundColor = DefaultBackgroundColour;
        offset.TextColor = Colors.White;
        offset.TextChanged += (s, e) =>
        {
            this.SaveLyricOptions();
        };
        grid.AddRow(0, offset, null);


        this.AudioFile.Children.Add(grid);
    }



    private void SaveLyricOptions()
    {
        this.lyricService.SaveLyric(this.currentLyric);
    }

    private Grid Create2ColumnGrid()
    {
        return new Grid()
        {
            ColumnDefinitions = new ColumnDefinitionCollection()
            {
                {new ColumnDefinition() {Width = GridLength.Star } },
                {new ColumnDefinition() {Width = GridLength.Auto} }
            },
        };
    }
}

public static class GridExtentions
{
    public static void AddHeader(this Grid grid, IView view, int columnSpan = 2)
    {
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        int rowIndex = grid.RowDefinitions.Count - 1;
        grid.Add(view, 0, rowIndex);
        grid.SetColumnSpan(view, columnSpan);
    }

    public static void AddRow(this Grid grid, int column, IView view, IView? view2)
    {
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        if (view is View v1) v1.Margin = new Thickness(0, 0, 0, 20);
        if (view2 is View v2) v2.Margin = new Thickness(0, 0, 0, 20);
        grid.Add(view, column, grid.RowDefinitions.Count - 1);
        if (view2 != null)
        {
            grid.Add(view2, column + 1, grid.RowDefinitions.Count - 1);
        }
        else
        {
            grid.Add(new Label() { Margin = new Thickness(0, 0, 0, 20) }, column + 1, grid.RowDefinitions.Count - 1);
        }
    }
}