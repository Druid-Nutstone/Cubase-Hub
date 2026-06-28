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
                    var lyricItemBar = this.lyrics.GetBar(transportLocation.BarBeatTime+1);
                    await Task.Delay(250);
                    if (lyricItemBar != null)
                    {
                        // await Task.Delay(100); // allow last lyric line to be sung!?
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
                
                if (currentBar > this.lyrics.MaxBar())
                {
                    timer.Stop();
                    return;
                }
                
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

    private void EnsureVisible(int index)
    {
        // Dispatch to ensure layout is settled
        Dispatcher.Dispatch(async () =>
        {
            // Give the UI a tiny moment to stabilize if it was just added
            await Task.Delay(10);

            var targetRow = lyricRows[index];
            var endRowLyric = this.lyrics.Where(x => x.Lyric.Trim().Length == 0 || x.Bar > -1)
                                     .Skip(index)
                                     .FirstOrDefault();

            int endRowLineIndex = (endRowLyric != null) ? this.lyrics.IndexOf(endRowLyric) : this.lyrics.Count - 1;
            var endRow = lyricRows[endRowLineIndex];

            double viewportHeight = LyricScrollView.Height;
            double desiredFraction = 0.20;
            double targetSectionHeight = (endRow.Y + endRow.Height) - targetRow.Y;

            // Calculate boundaries:
            // We want targetRow.Y to be at (viewportHeight * 0.33)
            double idealScrollY = targetRow.Y - (viewportHeight * desiredFraction);

            // If the section is too tall, the bottom of the section must be at the bottom of the viewport
            // bottom of section = (endRow.Y + endRow.Height)
            // bottom of viewport = (scrollTo + viewportHeight)
            // Therefore: scrollTo = (endRow.Y + endRow.Height) - viewportHeight
            double maxScrollForBottom = (endRow.Y + endRow.Height) - viewportHeight;

            // We use Math.Min and Math.Max to clamp the value without logic branches
            // 1. We don't want to scroll higher than targetRow.Y (to keep top visible)
            // 2. We don't want to scroll lower than maxScrollForBottom (to keep bottom visible)
            double scrollTo = Math.Clamp(idealScrollY, 0, Math.Max(0, maxScrollForBottom));

            // Use false for 'animated' if you want to eliminate the native animation "jump" 
            // while the system tries to interpolate the scroll distance
            await LyricScrollView.ScrollToAsync(0, scrollTo, true);
        });
    }

    //private async void EnsureVisible(int index)
    //{
    //    var targetRow = lyricRows[index];

    //    // Determine the end of the section
    //    var endRowLyric = this.lyrics.Where(x => x.Lyric.Trim().Length == 0 || x.Bar > -1)
    //                             .Skip(index)
    //                             .FirstOrDefault();

    //    int endRowLineIndex = (endRowLyric != null) ? this.lyrics.IndexOf(endRowLyric) : this.lyrics.Count - 1;
    //    var endRow = lyricRows[endRowLineIndex];

    //    double viewportHeight = LyricScrollView.Height;
    //    double desiredFraction = 0.33;

    //    // Calculate how much space the target section takes up
    //    double targetSectionHeight = endRow.Y + endRow.Height - targetRow.Y;

    //    // 1. Calculate the ideal scroll position (target at 1/3)
    //    double idealScrollTo = targetRow.Y - (viewportHeight * desiredFraction);

    //    // 2. Calculate the maximum safe scroll position
    //    // If we scroll past (targetRow.Y), the top of the section goes off-screen.
    //    // If the section is huge, we don't want to scroll past targetRow.Y.
    //    double maxScrollTo = targetRow.Y;

    //    // 3. Determine the final position
    //    // If the section is larger than the available space (2/3 of viewport), 
    //    // clamp it to the start of the section.
    //    double scrollTo;
    //    if (targetSectionHeight > (viewportHeight * (1 - desiredFraction)))
    //    {
    //        // Section is too big; pin the top of the section to the top of the viewport 
    //        // (or slightly below if you prefer a small padding)
    //        scrollTo = Math.Max(0, targetRow.Y);
    //    }
    //    else
    //    {
    //        // Section fits; use the 1/3 offset
    //        scrollTo = Math.Max(0, idealScrollTo);
    //    }

    //    await LyricScrollView.ScrollToAsync(0, scrollTo, true);
    //}



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