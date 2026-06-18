using Cubase.Macro.Common.Models;
using Cubase.Macro.Forms.Lyrics.Editor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Forms.Lyrics
{
    public class TopMenuItems
    {
        private readonly MenuStrip topMenu;

        private readonly LyricsForm lyricEditor;

        public TopMenuItems(MenuStrip topMenu, LyricsForm lyricEditor) 
        { 
            this.topMenu = topMenu;
            this.lyricEditor = lyricEditor;
            topMenu.Items.Add(new FileMenuItem(lyricEditor));
            topMenu.Items.Add(new NewMenuItem(lyricEditor));
            topMenu.Items.Add(new FontSizeMenuItem(lyricEditor));
        }
    }

    public class  FontSizeMenuItem : ToolStripMenuItem
    {
        public FontSizeMenuItem(LyricsForm lyricEditor)
        {
            this.Text = "Font Size";
            this.DropDownItems.Add(new IncreaseFontSizeMenuItem(lyricEditor));
            this.DropDownItems.Add(new DecreaseFontSizeMenuItem(lyricEditor));
        }
    }

    public class DecreaseFontSizeMenuItem : ToolStripMenuItem
    {
        private readonly LyricsForm lyricEditor;
        public DecreaseFontSizeMenuItem(LyricsForm lyricEditor)
        {
            this.Text = "Decrease Font Size";
            this.lyricEditor = lyricEditor;
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.lyricEditor.DecreaseFontSize();
        }
    }

    public class IncreaseFontSizeMenuItem : ToolStripMenuItem
    {
        private readonly LyricsForm lyricEditor;
        public IncreaseFontSizeMenuItem(LyricsForm lyricEditor)
        {
            this.Text = "Increase Font Size";
            this.lyricEditor = lyricEditor;
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.lyricEditor.IncreaseFontSize();
        }
    }

    public class  NewMenuItem : ToolStripMenuItem
    {
        public NewMenuItem(LyricsForm lyricEditor)
        {
            this.Text = "New";
            this.DropDownItems.Add(new NewLyricMenuItem(lyricEditor));
            this.DropDownItems.Add(new NewSetListMenuItem(lyricEditor));    
        }        
    }

    public class NewLyricMenuItem : ToolStripMenuItem
    {
        private readonly LyricsForm lyricEditor;
        public NewLyricMenuItem(LyricsForm lyricEditor)
        {
            this.Text = "New Lyric";
            this.lyricEditor = lyricEditor;
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.lyricEditor.InitialiseEdit([], LyricEditorType.Lyric, string.Empty);
        }
    }

    public class NewSetListMenuItem : ToolStripMenuItem
    {
        private readonly LyricsForm lyricEditor;
        public NewSetListMenuItem(LyricsForm lyricEditor)
        {
            this.Text = "New SetList";
            this.lyricEditor = lyricEditor;
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.lyricEditor.InitialiseEdit([], LyricEditorType.SetList, string.Empty);
        }
    }

    public class FileMenuItem : ToolStripMenuItem
    {
        public FileMenuItem(LyricsForm lyricEditor)
        {
            this.Text = "File";
            this.DropDownItems.Add(new OpenLyricMenuItem(lyricEditor));
            this.DropDownItems.Add(new SaveLyricMenuItem(lyricEditor));
            this.DropDownItems.Add(new OpenSetListMenuItem(lyricEditor));
            this.DropDownItems.Add(new SaveSetListMenuItem(lyricEditor));
        }
    }

    public class OpenLyricMenuItem : ToolStripMenuItem
    {
        private readonly LyricsForm lyricEditor;
        public OpenLyricMenuItem(LyricsForm lyricEditor)
        {
            this.Text = "Open Lyrics";
            this.lyricEditor = lyricEditor;
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = CubaseMacroConstants.DropBoxBaseDirectory;
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.lyricEditor.InitialiseEdit(System.IO.File.ReadAllLines(openFileDialog.FileName), LyricEditorType.Lyric, openFileDialog.FileName);
            }
        }
    }

    public class SaveLyricMenuItem : ToolStripMenuItem
    {
        private readonly LyricsForm lyricEditor;

        public SaveLyricMenuItem(LyricsForm lyricEditor)
        {
            this.Text = "Save Lyrics";
            this.lyricEditor = lyricEditor;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (string.IsNullOrEmpty(this.lyricEditor.FileName))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = CubaseMacroConstants.DropBoxBaseDirectory;
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(saveFileDialog.FileName, this.lyricEditor?.lyricEditor?.Text);
                }
            }
            else
            {
                System.IO.File.WriteAllText(this.lyricEditor.FileName, this.lyricEditor?.lyricEditor?.Text);
            }
        }
    }

    public class OpenSetListMenuItem : ToolStripMenuItem
    {
        private readonly LyricsForm lyricEditor;
        public OpenSetListMenuItem(LyricsForm lyricEditor)
        {
            this.Text = "Open SetList";
            this.lyricEditor = lyricEditor;
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = CubaseMacroConstants.DropBoxBaseDirectory;
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.lyricEditor.InitialiseEdit(System.IO.File.ReadAllLines(openFileDialog.FileName), LyricEditorType.SetList, openFileDialog.FileName);
            }
        }
    }

    public class SaveSetListMenuItem : ToolStripMenuItem
    {
        private readonly LyricsForm lyricEditor;

        public SaveSetListMenuItem(LyricsForm lyricEditor)
        {
            this.Text = "Save SetList";
            this.lyricEditor = lyricEditor;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (string.IsNullOrEmpty(this.lyricEditor.FileName))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = CubaseMacroConstants.DropBoxBaseDirectory;
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(saveFileDialog.FileName, this.lyricEditor.Text);
                }
            }
            else
            {
                System.IO.File.WriteAllText(this.lyricEditor.FileName, this.lyricEditor.Text);
            }
        }
    }
}
