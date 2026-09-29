using Cubase.Macro.Common.Lyrics.Services.Scrolling;
using Cubase.Macro.Common.Models;
using Cubase.Macro.Common.Models.Lyrics;
using Cubase.Macro.Common.Socket;
using Cubase.Macro.Mobile.Configuration;
using Cubase.Macro.Mobile.Lyrics.LyricMenu;
using Cubase.Macro.Mobile.Services.Lyrics;
using Cubase.Macro.Mobile.Services.Mswin;
using Image = Microsoft.Maui.Controls.Image;

namespace Cubase.Macro.Mobile.Lyrics;

public partial class LyricViewer : ContentPage
{
    private readonly IMsWinService msWinService;

    private readonly CubaseMacroWebSocketClient webSocketClient;

    private readonly ILyricService lyricService;

    private readonly IMobileConfigurationService configurationService;

    private readonly IScrollerService scrollerService;

    private List<LyricSection> lyrics;

    private List<LyricGrid> lyricRows = new();

    private MenuHandler menuHandler;

    private SetListNavigationButton setList;

    private LyricContainer currentLyrics;

    private SetListContainer currentSetlist;

    private BottomMenuHandler bottomMenuHandler;

    private CubaseMidiProjectStatus midiProjectStatus;

    private LyricMenuContainer lyricMenuContainer;

    private bool isFirstAppearance = true;

    public LyricViewer(ILyricService lyricService,
                       IMsWinService msWinService,
                       IScrollerService scrollerService,
                       IMobileConfigurationService mobileConfigurationService,
                       CubaseMacroWebSocketClient webSocketClient)
    {
        InitializeComponent();
        this.BackgroundColor = CubaseMacroMobileConstants.DefaultBackgroundColour;
        this.lyricService = lyricService;
        this.webSocketClient = webSocketClient;
        this.msWinService = msWinService;
        this.scrollerService = scrollerService;
        this.SongTime.Text = "00:00";
        this.configurationService = mobileConfigurationService;
        this.menuHandler = new MenuHandler(this.Menu, this);
        this.bottomMenuHandler = new BottomMenuHandler(this.BottomMenu, this);
        this.lyricMenuContainer = new LyricMenuContainer(this.lyricService);
    }

    protected override async void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        this.SetDeviceProperties(width);
    }

    private async void SetDeviceProperties(double width)
    {
        //var mainDisplay = DeviceDisplay.Current.MainDisplayInfo;
        // Calculate smallest width in density-independent pixels (dp)
        //double widthInDp = mainDisplay.Width / mainDisplay.Density;
        //double heightInDp = mainDisplay.Height / mainDisplay.Density;
        //double smallestWidth = Math.Min(widthInDp, heightInDp);
        if (width <= 360)
        {
            this.menuHandler.SetForMobilePortrait(width);
        }
        else
        {
            this.menuHandler.SetForNormalDisplay();
        }
    }

    private async Task<bool> LoadWinFileIfRequired()
    {
        if (this.msWinService.HaveLyricFile())
        {
            if (File.Exists(this.msWinService.LyricFile))
            {
                var lyricContent = Common.Models.Lyrics.LyricContainer.Load(this.msWinService.LyricFile, (err) => { });
                await this.LoadFile(lyricContent);
                return true;
            }
            else
            {
                this.ProcessError($"Cannot find file {this.msWinService.LyricFile}");
            }
        }
        return false;
    }


    public async Task ShowHideChords(bool isVisible)
    {
        this.lyricRows.Where(x => x.Type == LineType.Chord)
            .ToList()
            .ForEach(x => x.IsVisible = isVisible);
    }

    public async Task StartAutoScroll()
    {
        _ = MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await LyricScrollView.ScrollToAsync(0, 0, true);
        });
        if (this.webSocketClient.Connected)
        {
            var currentProject = await this.webSocketClient.GetProjectStatus((err) => { });
            if (currentProject != null)
            {
                if (currentProject.ProjectName.Equals(this.currentLyrics.Title, StringComparison.OrdinalIgnoreCase))
                {

                    await this.scrollerService.StartMidiTimer(this.currentLyrics, this.GotoBar, this.TransportTimeUpdated);
                }
                else
                {
                    await this.StartAudioPlayBackIfRequired();
                    this.scrollerService.StartDurationTimer(this.currentLyrics, this.GotoBar, this.TransportTimeUpdated);
                }
            }
        }
        else
        {
            await this.StartAudioPlayBackIfRequired();
            this.scrollerService.StartDurationTimer(this.currentLyrics, this.GotoBar, this.TransportTimeUpdated);
        }

    }

    private async Task StartAudioPlayBackIfRequired()
    {
        if (this.currentLyrics.CustomOptions.PlayAudioWithScroll)
        {
            if (this.currentLyrics.CustomOptions.PlayAudioWithScroll)
            {
                await this.lyricService.StartAudio(this.currentLyrics);
            }
        }
    }

    private void TransportTimeUpdated(TimeSpan time, int currentBar)
    {
        this.CurrentBar.Text = $"{time.Minutes.ToString().PadLeft(2, '0')}:{time.Seconds.ToString().PadLeft(2, '0')} {currentBar}";
        this.CurrentBar.InvalidateMeasure();
    }

    private async void GotoBar(int bar)
    {
        await this.ResetPointer();
        var sectionGrid = await this.ScrollToBarAsync(bar);
        if (sectionGrid != null)
        {
            ((Image)sectionGrid?.Children[0]).IsVisible = true;
        }
    }

    public async Task<LyricGrid?> ScrollToBarAsync(int targetBar)
    {
        // 1. Find the target grid using your custom Bar property
        var targetGrid = this.lyricRows.FirstOrDefault(r => r.Bar == targetBar);
        if (targetGrid == null) return null;

        // 2. Get the Y position relative to the StackLayout
        double targetY = targetGrid.Y;

        // Fallback: If layout hasn't fully rendered yet and Y is 0, 
        // accumulate heights manually to find the correct vertical offset
        if (targetY == 0 && targetGrid != this.lyricRows.FirstOrDefault())
        {
            foreach (var row in this.lyricRows)
            {
                if (row == targetGrid) break;
                targetY += row.Height + row.Margin.VerticalThickness;
            }
        }

        // 3. Calculate the offset: position the grid 1/3 down the viewport screen
        double viewportHeight = LyricScrollView.Height;
        double offsetPosition = targetY - (viewportHeight / 3.0);

        // Ensure we don't scroll to a negative value
        offsetPosition = Math.Max(0, offsetPosition);

        // 4. Smoothly scroll to the calculated position
        await LyricScrollView.ScrollToAsync(0, offsetPosition, true);
        return targetGrid;
    }

    public async Task StopAutoScroll()
    {
        this.scrollerService.Stop();
        await this.lyricService.StopAudio();
        await ResetPointer();
        _ = MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await LyricScrollView.ScrollToAsync(0, 0, true);
        });
        await this.menuHandler.ResetScroll();
    }

    public async Task IncreaseFontSize()
    {
        SetFontSize((lbl) =>
        {
            this.SetHeightAndWidth(1, lbl);
        });
    }

    public async Task DecreaseFontSize()
    {
        SetFontSize((lbl) =>
        {
            this.SetHeightAndWidth(-1, lbl);
        });
    }

    private void SetHeightAndWidth(int minusOrPlus, IView view)
    {
        if (view is Label)
        {
            ((Label)view).FontSize += minusOrPlus;
        }
        if (view is LyricGrid)
        {
            LyricGrid grid = (LyricGrid)view;
            grid.Padding = new Thickness() { Bottom = grid.Padding.Bottom + minusOrPlus };
        }
        if (view is Image)
        {
            ((Image)view).WidthRequest += minusOrPlus;
            ((Image)view).HeightRequest += minusOrPlus;
        }
    }

    public void SetFontSize(Action<IView> callBack)
    {
        foreach (var grid in this.lyricRows)
        {
            switch (grid.Type)
            {
                case LineType.Header:
                    callBack(grid.Children[0]);
                    break;
                case LineType.EmptyLine:
                    callBack(grid);
                    break;
                case LineType.SectionHeader:
                    callBack(grid.Children[0]);
                    callBack(grid.Children[1]);
                    callBack(grid.Children[2]);
                    break;
                default:
                    callBack(grid.Children[0]);
                    callBack(grid.Children[1]);
                    break;
            }

        }
    }

    public async Task ResetPointer()
    {
        this.CurrentBar.Text = "00:00";
        for (int i = 0; i < lyricRows.Count; i++)
        {
            var grid = lyricRows[i];
            if (grid.Type == LineType.SectionHeader)
            {
                ((Image)grid.Children[0]).IsVisible = false;
            }
        }
    }


    public async Task ShowFiles()
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await this.menuHandler.SetLyricButtonSelected();
            var targetColumn = MainGrid.ColumnDefinitions[0];
            // Create an animation object
            var animation = new Animation(
                callback: (v) =>
                {
                    targetColumn.Width = new GridLength(v);
                    // MainGrid.InvalidateMeasure();
                },
                start: 0,
                end: 300,
                easing: Easing.CubicOut
            );
            // Commit the animation
            // The 'this' refers to the page, "FilesAnimation" is just a unique ID
            animation.Commit(this, "FilesAnimation", length: 250);
            this.FilesBorder.IsVisible = true;
            this.FilesBorder.WidthRequest = 300;
            MainGrid.InvalidateMeasure();
        });
    }

    public async Task<bool> CloseFiles()
    {
        if (!this.FilesBorder.IsVisible)
        {
            return true;
        }

        var tcs = new TaskCompletionSource<bool>();

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            this.FilesBorder.WidthRequest = 300;
            var targetColumn = MainGrid.ColumnDefinitions[0];

            await this.menuHandler.SetLyricButtonUnSelected();
            // Immediately snap or animate to 0
            targetColumn.Width = new GridLength(0);
            var animation = new Animation(
                callback: (v) =>
                {
                    targetColumn.Width = new GridLength(v);
                    // MainGrid.InvalidateMeasure();
                },
                start: 300,
                end: 0,
                easing: Easing.CubicOut
            );

            animation.Commit(this, "HideFilesAnimation", length: 250, finished: (v, canceled) =>
            {
                // Animation finished, signal the Task
                this.FilesBorder.IsVisible = false;
                tcs.SetResult(true);
            });
            MainGrid.InvalidateMeasure();

            return tcs.Task;
        });
        return true;
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();
    }

    private async Task OnLyricClick(LyricContainer lyricContainer, bool closeSetlist)
    {
        if (closeSetlist)
        {
            this.currentSetlist?.CurrentSong = 0;
            SetListButtons.IsVisible = false;
            SetListButtons.InvalidateMeasure();
        }
        await this.LoadFile(lyricContainer);
    }

    private async Task OnSetlistClick(SetListContainer setListContainer)
    {
        await this.LoadSetList(setListContainer);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (isFirstAppearance)
        {
            isFirstAppearance = false;
            await this.menuHandler.BuildMenu();
            await this.BuildSetListButtons();
            if (!await this.LoadWinFileIfRequired())
            {
                await this.lyricMenuContainer.Initialise(this.OnLyricClick,
                                                         this.OnSetlistClick);

                this.LyricMenuLoader.Children.Clear();
                this.LyricMenuLoader.Children.Add(this.lyricMenuContainer);
                if (this.webSocketClient.Connected)
                {
                    var currentCubaseProject = await this.webSocketClient.GetProjectStatus((err) => { });
                    if (currentCubaseProject != null)
                    {
                        var projectFile = await this.lyricService.LoadProjectLyricIfAvailable(currentCubaseProject.ProjectName);
                        if (projectFile != null)
                        {
                            await this.LoadFile(Common.Models.Lyrics.LyricContainer.Load(projectFile, async (err) =>
                            {
                                await DisplayAlertAsync("Load Error", $"Cannot load project file {projectFile}", "OK");
                            }));
                        }
                    }
                    else
                    {
                        await this.menuHandler.DisableButtons();
                        await this.ShowFiles();
                    }
                }
                else
                {
                    // todo need to just get the current cubase project and then load it from local disk 
                    await this.menuHandler.DisableButtons();
                    await this.ShowFiles();
                }
            }
        }
    }

    private async Task BuildSetListButtons()
    {
        this.SetListButtons.Children.Clear();
        var backButton = new SetListNavigationButton(this.OnSetlistNavigation, SetListDirection.Back);
        var forwardButton = new SetListNavigationButton(this.OnSetlistNavigation, SetListDirection.Forward);
        this.SetListButtons.Children.Add(backButton);
        this.SetListButtons.Children.Add(forwardButton);
    }

    private async void OnSetlistNavigation(SetListDirection direction)
    {
        var newPosition = direction == SetListDirection.Forward ? this.currentSetlist.CurrentSong + 1 : this.currentSetlist.CurrentSong - 1;
        this.currentSetlist.CurrentSong = newPosition;
        if (this.currentSetlist.CurrentSong > this.currentSetlist.Songs.Count - 1)
        {
            this.currentSetlist.CurrentSong = 0;
        }
        if (this.currentSetlist.CurrentSong < 0)
        {
            this.currentSetlist.CurrentSong = this.currentSetlist.Songs.Count - 1;
        }
        var setListSong = await this.lyricService.LoadSetListLyric(this.currentSetlist.Songs[this.currentSetlist.CurrentSong]);
        await this.lyricMenuContainer.SetCurrentSetListLyric(this.currentSetlist.CurrentSong);
        await this.LoadFile(setListSong);
    }

    public async Task LoadSetList(SetListContainer container)
    {
        this.currentSetlist = container;
        SetListButtons.IsVisible = true;
        SetListButtons.InvalidateMeasure();
        BottomMenu.InvalidateMeasure();
        await Task.Delay(5); // Brief yield to let the UI thread process native handles
        LyricScrollView.InvalidateMeasure();
    }

    public async Task LoadFile(LyricContainer content)
    {
        this.currentLyrics = content;
        this.SongTime.Text = $"{content.Duration.Minutes.ToString().PadLeft(2, '0')}:{content.Duration.Seconds.ToString().PadLeft(2, '0')}";
        await this.menuHandler.EnableButtons(this.currentLyrics);
        await this.StopAutoScroll();
        // by default hide the chords - this SHOULD BE DONE BY PROFILE !
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            this.lyrics = content.Sections;
            this.lyricRows = new();
            this.Title = $"Lyrics - {this.currentLyrics.Title}  {(this.currentSetlist != null ? $"{this.currentSetlist.Title} ({this.currentSetlist.CurrentSong})" : string.Empty)}";
            if (await this.CloseFiles())
            {
                this.RefreshLyrics(content);
                // Force immediate measurement updates
                LyricContainer.InvalidateMeasure();
                LyricScrollView.InvalidateMeasure();
                MainGrid.InvalidateMeasure();

                await this.ShowHideChords(this.currentLyrics.CustomOptions.ShowChords);
            }
        });
    }

    private async void ProcessError(string errorMessage)
    {
        await DisplayAlertAsync("Error!", $"{errorMessage} {Environment.NewLine} Midi server IP: {this.configurationService.Configuration.MidiServerIpAddress}", "OK");
    }



    private void RefreshLyrics(LyricContainer container)
    {
        LyricContainer.Children.Clear();

        BuildHeader(container);

        foreach (var lyricModel in container.Sections)
        {
            this.BuildSectionHeader(lyricModel);

            var lyricLines = lyricModel.Lyrics.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            int searchStartIndex = 0;

            foreach (var rawLyricLine in lyricLines)
            {
                var lyricLine = rawLyricLine.TrimEnd('\r');

                // Find the exact absolute start index of this line in the original text
                int lineStartIndex = lyricModel.Lyrics.IndexOf(lyricLine, searchStartIndex);
                if (lineStartIndex == -1) lineStartIndex = searchStartIndex;

                var chords = lyricModel.Chords
                                       .Where(x => x.CharacterIndex >= lineStartIndex && x.CharacterIndex < lineStartIndex + lyricLine.Length)
                                       .ToList();

                if (chords.Any())
                {
                    // Calculate position relative to this line's actual start index
                    var relativeChords = chords.Select(x => new ChordSection() { CharacterIndex = x.CharacterIndex - lineStartIndex, Chord = x.Chord });
                    var chordLabelText = "";
                    int currentLength = 0;

                    foreach (var chord in relativeChords)
                    {
                        int spacesNeeded = chord.CharacterIndex - currentLength;

                        if (spacesNeeded > 0)
                        {
                            chordLabelText += new string(' ', spacesNeeded);
                            currentLength += spacesNeeded;
                        }

                        chordLabelText += chord.Chord;
                        currentLength += chord.Chord.Length;
                    }

                    var chordText = new LyricLabel
                    {
                        Text = chordLabelText,
                        TextColor = Colors.Red,
                        Type = LineType.Chord,
                        FontSize = container.FontSize,
                        IsVisible = true
                    };
                    this.AddLyricRow(chordText, LineType.Chord);
                }

                var lyricText = new LyricLabel
                {
                    Text = lyricLine,
                    TextColor = Colors.White,
                    Type = LineType.Lyric,
                    FontSize = container.FontSize,
                    IsVisible = true
                };
                this.AddLyricRow(lyricText, LineType.Lyric);

                foreach (var chord in chords)
                {
                    lyricModel.Chords.Remove(chord);
                }

                // Advance past this line for the next search iteration
                searchStartIndex = lineStartIndex + lyricLine.Length;
            }
            // add some space between sections 
            this.AddEmptyRow(container.FontSize);
        }

    }

    private void BuildSectionHeader(LyricSection section)
    {
        var row = new LyricGrid
        {
            HorizontalOptions = LayoutOptions.Fill,
            ColumnDefinitions = {
                new ColumnDefinition(30), // Arrow
                new ColumnDefinition(GridLength.Auto),
                new ColumnDefinition(GridLength.Star)
            },
            Type = LineType.SectionHeader,
            Bar = section.Bar
        };

        var pointer = this.GetPointer(section.FontSize);
        var sectionHeader = new LyricLabel
        {
            Text = section.Name,
            TextColor = Colors.Yellow,
            Type = LineType.SectionHeader,
            FontSize = section.FontSize + 1,
            Margin = new Thickness(0, 0, 25, 0), // Adds 15 pixels of space on the right
            IsVisible = true
        };
        var comments = new LyricLabel
        {
            Text = !string.IsNullOrEmpty(section.Comments) ? $"[{section.Comments}]" : string.Empty,
            TextColor = Colors.LightSlateGrey,
            Type = LineType.Comment,
            VerticalTextAlignment = TextAlignment.Center,
            FontSize = section.FontSize + 1,
            IsVisible = true
        };
        row.Add(pointer, 0, 0);
        row.Add(sectionHeader, 1, 0);
        row.Add(comments, 2, 0);
        this.lyricRows.Add(row);
        LyricContainer.Children.Add(row);
    }

    private void BuildHeader(LyricContainer container)
    {
        var row = new LyricGrid
        {
            HorizontalOptions = LayoutOptions.Fill,
            ColumnDefinitions = {
                new ColumnDefinition(GridLength.Star) // Text
            },
            Type = LineType.Header
        };
        var headerText = new LyricLabel
        {
            Text = container.Title,
            TextColor = Colors.Yellow,
            Type = LineType.Header,
            FontSize = container.FontSize + 2,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            IsVisible = true
        };

        row.Add(headerText, 0, 0);
        LyricContainer.Children.Add(row);
        this.lyricRows.Add(row);
        this.AddEmptyRow(container.FontSize);
    }

    private void AddEmptyRow(int padding)
    {
        var row = new LyricGrid
        {
            Padding = new Thickness() { Bottom = padding + 20 },
            ColumnDefinitions = {
                new ColumnDefinition(GridLength.Star) // Text
            },
            Type = LineType.EmptyLine
        };
        this.lyricRows.Add(row);
        LyricContainer.Children.Add(row);
    }

    private void AddLyricRow(LyricLabel lyricLabel, LineType lineType)
    {
        var row = new LyricGrid
        {
            HorizontalOptions = LayoutOptions.Fill,
            ColumnDefinitions = {
                new ColumnDefinition(30), // Arrow
                new ColumnDefinition(GridLength.Star) // Text
            },
            Type = lineType
        };

        var arrow = this.GetPointer((int)lyricLabel.FontSize);

        row.Add(arrow, 0, 0);
        row.Add(lyricLabel, 1, 0);
        lyricRows.Add(row);
        LyricContainer.Children.Add(row);
    }

    private Image GetPointer(int size)
    {
        return new Image
        {
            Source = "pointer.png",
            WidthRequest = size,
            HeightRequest = size,
            IsVisible = false
        };
    }

}