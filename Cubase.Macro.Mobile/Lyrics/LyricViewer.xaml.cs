using Cubase.Macro.Common.Lyrics;
using Cubase.Macro.Common.Lyrics.Services;
using Cubase.Macro.Common.Models;
using Cubase.Macro.Common.Socket;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Cubase.Macro.Mobile.Lyrics;

public partial class LyricViewer : ContentPage
{
    private readonly ILyricService lyricService;

    private readonly CubaseMacroWebSocketClient webSocketClient;

    private readonly FileHandler fileHandler;

    private MobileLyricCollection lyrics;

    private int BPM = -1; // beats per minute  

    private int BPB = -1; // beats per bar 

    private int StartBar = 0;

    private Stopwatch stopwatch;

    private IDispatcherTimer timer;

    private List<Grid> lyricRows = new();



    public LyricViewer(ILyricService lyricService,
                       FileHandler fileHandler,
                       CubaseMacroWebSocketClient webSocketClient)
    {
        InitializeComponent();
        this.BackgroundColor = CubaseMacroMobileConstants.DefaultBackgroundColour;
        this.lyricService = lyricService;
        this.fileHandler = fileHandler;
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
        SetPrimaryPointer();
        stopwatch = new Stopwatch();
        stopwatch.Start();
        LyricScrollView.ScrollToAsync(0, 0, true);
        timer.Start();
    }

    public void StopAutoScroll()
    {
        timer.Stop();
        ResetPointer();
        LyricScrollView.ScrollToAsync(0, 0, true);
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

    public void ResetPointer()
    {
        for (int i = 0; i < lyricRows.Count; i++)
        {
            var arrow = (Label)lyricRows[i].Children[0];
            arrow.IsVisible = false;
        }
    }

    private void SetPrimaryPointer()
    {
        if (!webSocketClient.Connected)
        {
            if (StartBar > 0)
            {
                var startAt = this.lyrics.FirstOrDefault(x => x.Bar <= StartBar && x.Bar > -1);
                if (startAt != null)
                {
                    SetBarLocation(startAt.Bar);
                }
            }
        }
    }

    private async void HideFiles(object sender, EventArgs e)
    {
        CloseFiles();
    }

    public async void ShowFiles()
    {
        FilesBorder.InputTransparent = false;
        await FilesBorder.TranslateToAsync(0, 0, 250, Easing.CubicOut);
        await this.fileHandler.Initialise(this.FileContainer, this, this.ProcessError);
    }

    public async void CloseFiles()
    {
        await FilesBorder.TranslateToAsync(-300, 0, 250, Easing.CubicIn);
        // Prevent interaction with the frame while hidden
        FilesBorder.InputTransparent = true;
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
        if (webSocketClient.Connected)
        {
            var lyricContent = await this.webSocketClient.GetCurrentLyric((err) =>
            {
                this.ShowFiles();
            });
            if (lyricContent != null)
            {
                if (lyricContent.IsSuccess)
                {
                    this.LoadFile(lyricContent.LyricContent);
                }
                else
                {
                    this.ShowFiles();
                }
            }
        }
        else
        {
            this.ShowFiles();
        }
    }

    public async void LoadFile(IEnumerable<string> content)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            StartBar = 0;
            BPB = -1;
            BPM = -1;
            if (!FilesBorder.InputTransparent)
            {
                this.CloseFiles();
            }
            var lyricBuffer = this.lyricService.ParseLyrics(content, 0, ' ');
            this.lyrics = new MobileLyricCollection(lyricBuffer);
            this.RefreshLyrics(lyrics);
            if (this.lyricService.LyricCollection != null)
            {
                this.ProcessControlStatements(this.lyricService.LyricCollection);
            }
        });
    }

    private async void ProcessError(string errorMessage)
    {
        await DisplayAlertAsync("Error!", errorMessage, "OK");
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
        var bpmControl = sections.GetControlValue(ControlLyricKeyword.Tempo);
        if (bpmControl != null)
        {
            BPM = int.Parse(bpmControl);
        }
        var bpbControl = sections.GetControlValue(ControlLyricKeyword.Beats_Per_Bar);
        if (bpbControl != null)
        {
            BPB = int.Parse(bpbControl);
        }
        var startBar = sections.GetControlValue(ControlLyricKeyword.Start_Bar);
        if (startBar != null)
        {
            StartBar = int.Parse(startBar);
        }

    }

    private async void TimerElapsed(object? sender, EventArgs e)
    {
        if (webSocketClient.Connected)
        {
            var transportLocation = await this.webSocketClient.GetTransportLocation(this.ProcessError);
            if (transportLocation != null)
            {
                if (transportLocation.TransportType == Common.Models.TransportType.BarsBeats)
                {
                    // look for the bar 1 ahead of the current position to allow for scrolling in 'good' time!
                    var lyricItemBar = this.lyrics.GetBar(transportLocation.BarBeatTime);
                    
                    if (lyricItemBar != null)
                    {
                        SetBarLocation(lyricItemBar.Bar);
                    }
                }
            }
        }
        else // manual calculation
        {
            if (BPM > -1 && BPB > -1)
            {
                double totalSeconds = stopwatch.Elapsed.TotalSeconds;
                double secondsPerBeat = 60.0 / BPM;
                // 2. Seconds per full bar
                double secondsPerBar = secondsPerBeat * BPB;
                // 3. Calculate current bar (adding 1 because we start counting at Bar 1)
                double currentBar = (int)(totalSeconds / secondsPerBar) + StartBar;
                Debug.WriteLine($"Calculated Bar: {currentBar}");
                var lyricItemBar = this.lyrics.GetBar((int)currentBar);
                if (lyricItemBar != null)
                {
                    SetBarLocation(lyricItemBar.Bar);
                }
            }
        }


    }

    private void SetBarLocation(int targetBar)
    {
        var lyricItemBar = this.lyrics.GetBar(targetBar);
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

    private void UpdateArrowVisibility(int targetIndex)
    {
        for (int i = 0; i < lyricRows.Count; i++)
        {
            var arrow = (Label)lyricRows[i].Children[0];
            arrow.IsVisible = (i == targetIndex);
        }
    }

    private void EnsureVisible(int index, lastIndex)
    {
        need to see if the last line in the section still going to be visible ?! 
        
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
        LyricScrollView.ScrollToAsync(0, scrollTo, true);


        // not implementing the next bit 'cos i always want to scroll so the whole verse is displayed 

        // Optional: if i want to scroll if the target is actually outside the current view bounds
        // currentScroll = LyricScrollView.ScrollY;
        //if (targetY < currentScroll || targetY > (currentScroll + viewportHeight - targetRow.Height))
        //{
        //    LyricScrollView.ScrollToAsync(0, scrollTo, true);
        //}
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
                IsVisible = lyricModel.IsCurrent,
                BackgroundColor = Colors.Transparent
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