using Cubase.Macro.Common.Lyrics;
using Cubase.Macro.Common.Lyrics.Services;
using Cubase.Macro.Common.Models;
using Cubase.Macro.Forms.Lyrics.Editor;
using Cubase.Macro.Forms.Lyrics.Viewer;
using Cubase.Macro.Services.Config;
using Cubase.Macro.Services.Midi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

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
        private readonly ILyricService lyricService;
        private readonly IlyricMidiService lyricMidiService;
        private LyricEditor? editor;
        private LyricViewer? viewer;
        private LyricEditorType lyricEditorType;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string FileName { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnumerable<string> SourceLyrics { get; set; } = new List<string>();

        public LyricViewerForm(IConfigurationService configurationService,
                               IlyricMidiService lyricMidiService,
                               ILyricService lyricService)
        {
            InitializeComponent();
            ThemeApplier.ApplyDarkTheme(this);
            this.configurationService = configurationService;
            this.lyricService = lyricService;
            this.lyricMidiService = lyricMidiService;
            SaveButton.Bind(SaveLyrics, "Save", "Save Lyrics to file");
            SaveButton.Enabled = false;
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
            this.lyricService.UseMidi(MidiEnabled.Checked); 
        }

        private void OpenLyrics()
        {
            var fileOpen = new OpenFileDialog();
            fileOpen.InitialDirectory = CubaseMacroConstants.DropBoxBaseDirectory;
            fileOpen.Filter = "Lyric Files (*.txt)|*.txt";
            fileOpen.Title = "Open Lyric File";
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                this.FileName = fileOpen.FileName;
                this.LoadFile();
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.LoadLyricViewer();
            this.LoadFromSource();
        }

        private void SaveLyrics()
        {
            this.SourceLyrics = editor.Lines;
            var fileSave = new SaveFileDialog();
            fileSave.FileName = this.FileName;
            fileSave.InitialDirectory = CubaseMacroConstants.DropBoxBaseDirectory;
            if (fileSave.ShowDialog() == DialogResult.OK)
            {
                this.FileName = fileSave.FileName;
                File.WriteAllLines(fileSave.FileName, this.SourceLyrics);
                MessageBox.Show($"Lyrics and Chords saved to {fileSave.FileName}");
            }
        }

        private void StartScrolling()
        {
            if (ScrollButton.Text == StartAutoScroll)
            {
                ScrollButton.Text = EndAutoScroll;
                this.viewer?.StartAutoScroll();
            }
            else
            {
                this.viewer?.EndAutoScroll();
                ScrollButton.Text = StartAutoScroll;
            }
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
                this.SourceLyrics = editor?.Lines;
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
                    this.editor?.Initialise(SourceLyrics, this.FileName);
                }
                else
                {
                    this.viewer?.Initialise(SourceLyrics);
                }
                this.SetTitle();
            }
        }

        public void LoadFile()
        {
            if (!string.IsNullOrEmpty(this.FileName))
            {
                this.SetTitle();
                this.SourceLyrics = File.ReadAllLines(this.FileName);
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
            EditButton.Bind(this.EditLyric, "E", "Edit Lyrics");
            MidiEnabled.Visible = this.lyricMidiService.IsMidiAvailable();
            lyricEditorType = LyricEditorType.Viewer;
            this.viewer = new LyricViewer(this.lyricService);
            viewer.ScrollUpdateEvent = this.UpdateTransportLocation;
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
            this.editor = new LyricEditor(this.lyricService);
            this.LoadMainPanel(editor);
        }


        public void LoadMainPanel(Control cntrl)
        {
            this.MainPanel.Controls.Clear();
            cntrl.Dock = DockStyle.Fill;
            // ((ILyricEditor)cntrl).SetFontSize(12);
            this.MainPanel.Controls.Add(cntrl);
        }
    }
}
