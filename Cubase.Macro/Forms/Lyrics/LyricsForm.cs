using Cubase.Macro.Forms.Lyrics.Editor;
using Cubase.Macro.Forms.Lyrics.Viewer;
using Cubase.Macro.Services.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Macro.Forms.Lyrics
{
    public partial class LyricsForm : BaseWindows11Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string FileName { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] SourceCode {  get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LyricEditor? lyricEditor { get; set; }

        private LyricViewer? lyricViewer;

        private ViewState viewState;

        public LyricsForm(IConfigurationService configurationService)
        {
            this.Text = "Create and Amend Lyrics and Setlists";
            InitializeComponent();
            ThemeApplier.ApplyDarkTheme(this);
            configurationService.ReloadConfiguration();
            // this.lyricEditor.SetDefaultFontSize(configurationService.Configuration.LyricFontSize);  
            var menuItems = new TopMenuItems(this.TopMenu, this);
            ViewButton.Click += ViewButton_Click;
            EditButton.Click += EditButton_Click;
            AutoScroll.Click += AutoScroll_Click;
            AutoScroll.Enabled = false;
            this.ToolStrip.GripStyle = ToolStripGripStyle.Hidden;
        }

        private void AutoScroll_Click(object? sender, EventArgs e)
        {
            if (viewState == ViewState.View)
            {
                this.lyricViewer.StartAutoScroll();
            }
        }

        private void EditButton_Click(object? sender, EventArgs e)
        {
            this.LoadEditor();
        }

        private void ViewButton_Click(object? sender, EventArgs e)
        {
            AutoScroll.Enabled = true;
            this.LoadViewer();
        }

        public void InitialiseEdit(string[] sourceCode, LyricEditorType editorType, string fileName)
        {
            this.viewState = ViewState.Edit; 
            this.FileName = fileName;
            this.SourceCode = sourceCode;
            this.lyricEditor = new LyricEditor();
            ThemeApplier.ApplyDarkTheme(this.lyricEditor);
            this.LoadPanel(lyricEditor);
            this.lyricEditor.Initialise(sourceCode, editorType, fileName);
        }

        public void IncreaseFontSize()
        {

        }

        public void DecreaseFontSize() 
        { 
        }

        private void LoadPanel(Control control)
        {
            this.MainPanel.Controls.Clear();
            control.Dock = DockStyle.Fill;
            this.MainPanel.Controls.Add(control);
            ThemeApplier.ApplyDarkTheme(control);
        }

        private void LoadViewer()
        {
            if (this.viewState == ViewState.Edit)
            {
                this.SourceCode = this.lyricEditor?.Lines;
            }
            this.lyricViewer = new LyricViewer();
            this.LoadPanel(this.lyricViewer);
            this.lyricViewer.Initialise(this.SourceCode ?? []);
            this.viewState = ViewState.View;
            this.lyricViewer.ScrollUpdateEvent = this.UpdateScrollDisplay;
        }

        private void UpdateScrollDisplay(TimeSpan timeSpan)
        {
            TrackLocation.Text = timeSpan.ToString();
            TrackLocation.TextBox.Update();
        } 
        
        private void LoadEditor()
        {
            if (this.SourceCode == null || this.SourceCode.Length == 0)
            {
                if (string.IsNullOrEmpty(this.FileName))
                {
                    this.SourceCode = [];
                }
            }
            this.lyricEditor = new LyricEditor();
            this.LoadPanel(lyricEditor);
            this.InitialiseEdit(this.SourceCode, LyricEditorType.Lyric, this.FileName);
        }
    }

    public enum ViewState
    {
        Edit,
        View
    }
}
