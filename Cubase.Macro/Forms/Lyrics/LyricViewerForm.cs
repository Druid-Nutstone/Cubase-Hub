using Cubase.Macro.Common.Lyrics;
using Cubase.Macro.Common.Lyrics.Services;
using Cubase.Macro.Common.Lyrics.Services.Scrolling;
using Cubase.Macro.Common.Models;
using Cubase.Macro.Forms.Lyrics.Editor.New;
using Cubase.Macro.Forms.Lyrics.Viewer.New;
using Cubase.Macro.Services.Config;
using System.ComponentModel;
using System.IO;

namespace Cubase.Macro.Forms.Lyrics
{
    public partial class LyricViewerForm : BaseWindows11Form
    {
        private enum LyricEditorType
        {
            Editor = 0,
            Viewer = 1
        }

        private string StartAutoScroll = "Start Scrolling";
        private string EndAutoScroll = "End Scrolling";

        private readonly IConfigurationService configurationService;
        private readonly IlyricMidiService lyricMidiService;
        private readonly IScrollerService scrollerService;
        private LyricEditorContainer? editor;
        private LyricViewerContainer? viewer;
        private LyricEditorType lyricEditorType;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string FileName { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LyricContainer SourceLyrics { get; set; }

        public LyricViewerForm(IConfigurationService configurationService,
                               IlyricMidiService lyricMidiService,
                               IScrollerService scrollerService)
        {
            InitializeComponent();
            ThemeApplier.ApplyDarkTheme(this);
            this.configurationService = configurationService;
            this.scrollerService = scrollerService;
            this.lyricMidiService = lyricMidiService;
            SaveButton.Bind(SaveLyrics, "Save", "Save Lyrics to file");
            SaveButton.Enabled = true;
            ScrollButton.Enabled = true;
            MidiEnabled.Visible = false;
            MidiEnabled.CheckedChanged += MidiEnabled_CheckedChanged;
            OpenButton.Bind(OpenLyrics, "Open", "Open A Lyric File");
            ScrollButton.Bind(StartScrolling, StartAutoScroll, "Start auto scrolling");
            FontIncrease.Bind(this.IncreaseFontSize, "+", "Increase Font");
            FontDecrease.Bind(this.DecreaseFontSize, "-", "Decrease Font");
        }

        private void MidiEnabled_CheckedChanged(object? sender, EventArgs e)
        {
        }

        private void OpenLyrics()
        {
            var fileOpen = new OpenFileDialog()
            {
                InitialDirectory = CubaseMacroConstants.NutstoneLyricBaseDirectory,
                Filter = $"Lyric Files (*{CubaseMacroConstants.NutstoneLyricNotation})|*{CubaseMacroConstants.NutstoneLyricNotation}",
                Title = "Open Lyric File",

                // 1. CRUCIAL: Breaks the OS "last used history" cache tracking rule
                RestoreDirectory = false,

                // 2. STABILITY: Forces the Win32 dialog engine to strictly evaluate 
                // the network path string viability before fallback routines kick in
                CheckPathExists = true
            };

            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                this.FileName = fileOpen.FileName;
                this.LoadFile();
            }
        }


        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.LoadLyricEditor();
            this.LoadFromSource();
        }

        private void SaveLyrics()
        {
            var sourceFile = this.editor?.Lyrics.Save(CubaseMacroConstants.NutstoneLyricBaseDirectory, (err) =>
            {
                MessageBox.Show($"Error saving lyrics: {err}");
            });
            if (!string.IsNullOrEmpty(sourceFile))
            {
                this.FileName = sourceFile;
                MessageBox.Show($"Lyrics and Chords saved to {sourceFile}");
            }

        }

        private void StartScrolling()
        {
            if (ScrollButton.Text == StartAutoScroll)
            {
                ScrollButton.Text = EndAutoScroll;
                if (MidiEnabled.Checked)
                {
                    // todo process midi ! 
                }
                else
                {
                    this.scrollerService.StartDurationTimer(this.SourceLyrics, this.OnGotoDurationBar, this.OnTransportLocationUpdate);
                }
            }
            else
            {
                this.scrollerService.Stop();
                ScrollButton.Text = StartAutoScroll;
            }
        }

        private void OnGotoDurationBar(int bar)
        {
            this.viewer?.GotoBar(bar);
        }

        private void OnTransportLocationUpdate(TimeSpan response)
        {
            this.TransPortLocation.Text = $"{(int)response.TotalMinutes:D2}:{response.Seconds:D2}";
            this.TransPortLocation.Update();
        }

        private void EditLyric()
        {
            if (lyricEditorType == LyricEditorType.Viewer)
            {
                SaveButton.Enabled = true;
                ScrollButton.Enabled = false;
                MidiEnabled.Visible = false;
                this.LoadLyricEditor();
            }
            else
            {
                SaveButton.Enabled = false;
                ScrollButton.Enabled = true;
                MidiEnabled.Visible = this.lyricMidiService.IsMidiAvailable();
                this.SourceLyrics = editor?.Lyrics;
                this.LoadLyricViewer();
            }
            this.LoadFromSource();
        }

        private void IncreaseFontSize()
        {
            ((ILyricEditor)this.MainPanel.Controls[0]).IncreaseFont();
        }

        private void DecreaseFontSize()
        {
            ((ILyricEditor)this.MainPanel.Controls[0]).DecreaseFont();
        }

        public void LoadFromSource()
        {
            if (SourceLyrics != null)
            {
                if (lyricEditorType == LyricEditorType.Editor)
                {
                    if (!string.IsNullOrEmpty(this.FileName))
                    {
                        this.editor.Initialise(this.FileName);
                    }
                }
                else
                {
                    //this.viewer?.Initialise(SourceLyrics);
                }
                this.SetTitle();
            }
        }

        public void LoadFile()
        {
            if (!string.IsNullOrEmpty(this.FileName))
            {
                this.SetTitle();
                this.SourceLyrics = LyricContainer.Load(this.FileName, (err) =>
                {
                    MessageBox.Show("Could not load lyric file: " + err);
                });
                this.LoadFromSource();
            }
        }

        private void SetTitle()
        {
            var titleType = lyricEditorType == LyricEditorType.Editor ? "Edit" : "View";
            var titleFile = string.IsNullOrEmpty(this.FileName) ? "No File" : Path.GetFileNameWithoutExtension(this.FileName);
            this.Text = $"{titleType} - {titleFile}";
        }

        private void LoadLyricViewer()
        {
            MidiEnabled.Visible = this.lyricMidiService.IsMidiAvailable();
            var externalViewer = this.configurationService.Configuration.LyricViewerFilePath;
            this.viewer = new LyricViewerContainer();
            this.viewer.Initialise(this.SourceLyrics);
            this.lyricEditorType = LyricEditorType.Viewer;
            this.LoadMainPanel(viewer);
        }

        private void UpdateTransportLocation(ScrollResponse response)
        {
            switch (response.LocationType)
            {
                case TransportLocationType.Time:
                    this.TransPortLocation.Text = $"{(int)response.TransportLocation.TotalMinutes:D2}:{response.TransportLocation.Seconds:D2}";
                    break;
                case TransportLocationType.Bar:
                    this.TransPortLocation.Text = $"Bar: {response.Bar}";
                    break;
                default:
                    this.TransPortLocation.Text = "????";
                    break;
            }
            this.TransPortLocation.Update();
        }

        private void LoadLyricEditor()
        {
            EditButton.Bind(this.EditLyric, "V", "View Lyrics");
            lyricEditorType = LyricEditorType.Editor;
            this.editor = new LyricEditorContainer();
            this.LoadMainPanel(editor);
        }


        public void LoadMainPanel(Control cntrl)
        {
            this.MainPanel.Controls.Clear();
            cntrl.Dock = DockStyle.Fill;
            this.MainPanel.Controls.Add(cntrl);
        }
    }
}
