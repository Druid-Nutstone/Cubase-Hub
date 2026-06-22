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

        private readonly IConfigurationService configurationService;
        private readonly IMidiService midiService;
        private readonly ILyricService lyricService;
        private LyricEditor? editor;
        private LyricViewer? viewer;
        private LyricEditorType lyricEditorType;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string FileName { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnumerable<string> SourceLyrics { get; set; } = new List<string>();

        public LyricViewerForm(IConfigurationService configurationService,
                               IMidiService midiService,
                               ILyricService lyricService)
        {
            InitializeComponent();
            ThemeApplier.ApplyDarkTheme(this);
            this.configurationService = configurationService;
            this.midiService = midiService;
            this.lyricService = lyricService;
            this.LoadLyricViewer();
            SaveButton.Bind(SaveLyrics, "Save", "Save Lyrics to file");
            SaveButton.Enabled = false;
            ScrollButton.Enabled = true;
            OpenButton.Bind(OpenLyrics, "Open", "Open A Lyric File");
            ScrollButton.Bind(StartScrolling, "Start Scrolling", "Start auto scrolling");
            FontIncrease.Bind(this.IncreaseFontSize, "+", "Increase Font");
            FontDecrease.Bind(this.DecreaseFontSize, "-", "Decrease Font");
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
            this.viewer?.StartAutoScroll();
        }

        private void EditLyric()
        {
            if (lyricEditorType == LyricEditorType.Viewer)
            {
                SaveButton.Enabled = true;
                ScrollButton.Enabled = false;
                this.LoadLyricEditor();
            }
            else
            {
                SaveButton.Enabled = false;
                ScrollButton.Enabled = true;
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
            }
        }

        public void LoadFile()
        {
            if (!string.IsNullOrEmpty(this.FileName))
            {
                this.SourceLyrics = File.ReadAllLines(this.FileName);
                this.LoadFromSource();
            }
        }

        private void LoadLyricViewer()
        {
            this.Text = $"Viewing Lyrics {this.FileName}";
            EditButton.Bind(this.EditLyric, "E", "Edit Lyrics");
            lyricEditorType = LyricEditorType.Viewer;
            this.viewer = new LyricViewer(this.lyricService, this.midiService);
            viewer.ScrollUpdateEvent = this.UpdateTransportLocation;
            this.LoadMainPanel(viewer);
        }

        private void UpdateTransportLocation(TimeSpan timeSpan)
        {
            this.TransPortLocation.Text = $"{(int)timeSpan.TotalMinutes:D2}:{timeSpan.Seconds:D2}";
            this.TransPortLocation.Update();
        }

        private void LoadLyricEditor()
        {
            this.Text = $"Editing Lyrics {this.FileName}";
            EditButton.Bind(this.EditLyric, "V", "View Lyrics");
            lyricEditorType = LyricEditorType.Editor;
            this.editor = new LyricEditor(this.lyricService, this.midiService);
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
