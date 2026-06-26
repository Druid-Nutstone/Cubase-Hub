using Cubase.Macro.Common.Lyrics;
using Cubase.Macro.Common.Lyrics.Services;
using Cubase.Macro.Common.Socket;

namespace Cubase.Macro.Mobile.Lyrics;

public partial class LyricViewer : ContentPage
{
    private readonly ILyricService lyricService;

    private readonly CubaseMacroWebSocketClient webSocketClient;

    private MobileLyricCollection lyrics;

    private IDispatcherTimer timer;

    private List<Grid> lyricRows = new();

    public LyricViewer(ILyricService lyricService, CubaseMacroWebSocketClient webSocketClient)
    {
        InitializeComponent();
        this.BackgroundColor = Color.FromRgba("#1E1E1E");
        this.lyricService = lyricService;
        this.webSocketClient = webSocketClient;
    }


    public void ShowHideChords(bool isVisible)
    {
        for (int i = 0; i < lyricRows.Count; i++)
        {
            var lyric = (LyricLabel)lyricRows[i].Children[1];
            if (lyric.LineType == LyricLineType.Chord) 
            { 
               lyric.IsVisible = isVisible;
            }
        }
    }

    public void StartAutoScroll()
    {
        timer.Start();
    }

    public void StopAutoScroll()
    {
        timer.Stop();
    }

    public void IncreaseFontSize()
    {
        SetFontSize((lbl) => 
        {
            lbl.FontSize += 1;
        });
    }

    public void DecreaseFontSize()
    {
        SetFontSize((lbl) =>
        {
            lbl.FontSize -= 1;
        });
    }

    public void SetFontSize(Action<Label> callBack)
    {
        for (int i = 0; i < lyricRows.Count; i++)
        {
            var arrow = (Label)lyricRows[i].Children[0];
            var lyric = (Label)lyricRows[i].Children[1];
            callBack(arrow);
            callBack(lyric);
        }
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();
        if (timer != null)
        {
            timer.Stop();
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        timer = Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromMilliseconds(500);
        timer.Tick += TimerElapsed;
        var lyricContent = await this.webSocketClient.GetCurrentLyric();
        if (lyricContent.IsSuccess)
        {
            MainThread.BeginInvokeOnMainThread(() => {
                var lyricBuffer = this.lyricService.ParseLyrics(lyricContent.LyricContent, 0, ' ');
                this.lyrics = new MobileLyricCollection(lyricBuffer);
                this.RefreshLyrics(lyrics);
                if (this.lyricService.LyricCollection != null)
                {
                    this.ProcessControlStatements(this.lyricService.LyricCollection);
                }
            });
        }
        else
        {
            await DisplayAlertAsync("Oops", lyricContent.ErrorMessage, "OK");
        }
    }

    private void ProcessControlStatements(LyricChordCollection sections)
    {
        var requestedFontSize = sections.GetControlValue(ControlLyricKeyword.Font_Size);
        if (requestedFontSize != null)
        {
            var fs = int.Parse(requestedFontSize);
            this.SetFontSize((lbl) => 
            { 
               lbl.FontSize = fs;
            });
        }
    }

    private async void TimerElapsed(object? sender, EventArgs e)
    {
        var transportLocation = await this.webSocketClient.GetTransportLocation();
        if (transportLocation.TransportType == Common.Models.TransportType.BarsBeats)
        {
            var lyricItemBar = this.lyrics.GetBar(transportLocation.BarBeatTime);
            if (lyricItemBar != null)
            {
                var index = this.lyrics.GetIndex(lyricItemBar);
                if (index >= 0 && index < lyricRows.Count)
                {
                    UpdateArrowVisibility(index);
                    EnsureVisible(index);
                }
            }
        }
    }

    private void UpdateArrowVisibility(int targetIndex)
    {
        for (int i = 0; i < lyricRows.Count; i++)
        {
            var arrow = (Label)lyricRows[i].Children[0];
            arrow.IsVisible = (i == targetIndex);
        }
    }

    private void EnsureVisible(int index)
    {
        var targetRow = lyricRows[index];

        // The desired position is 1/3 of the way down the visible viewport
        double desiredFraction = 0.33;
        double targetY = targetRow.Y;
        double viewportHeight = LyricScrollView.Height;

        // Calculate the scroll position needed to place targetY at 1/3 of viewportHeight
        // ScrollY = targetY - (1/3 * viewportHeight)
        double scrollTo = targetY - (viewportHeight * desiredFraction);

        // Ensure we don't scroll to a negative value
        scrollTo = Math.Max(0, scrollTo);

        // Optional: Only scroll if the target is actually outside the current view bounds
        // Or, if you want the arrow to ALWAYS center at the 1/3 mark, remove this if-check
        double currentScroll = LyricScrollView.ScrollY;
        if (targetY < currentScroll || targetY > (currentScroll + viewportHeight - targetRow.Height))
        {
            LyricScrollView.ScrollToAsync(0, scrollTo, true);
        }
    }

    private void RefreshLyrics(MobileLyricCollection lyrics)
    {
        LyricContainer.Children.Clear();

        foreach (var lyricModel in lyrics)
        {
            // 1. Create your custom row
            var row = new Grid
            {
                ColumnDefinitions = {
                new ColumnDefinition(40), // Arrow
                new ColumnDefinition(GridLength.Star) // Text
            }
            };

            var arrow = new Label
            {
                Text = "▶",
                TextColor = Colors.Red,
                IsVisible = lyricModel.IsCurrent
            };
            var text = new LyricLabel
            {
                Text = lyricModel.Lyric,
                LineType = lyricModel.LineType,
                TextColor = lyricModel.ForegoundColour,
                IsVisible = true

            };

            row.Add(arrow, 0, 0);
            row.Add(text, 1, 0);
            lyricRows.Add(row);
            // 2. Add to the container
            LyricContainer.Children.Add(row);
        }
    }

}