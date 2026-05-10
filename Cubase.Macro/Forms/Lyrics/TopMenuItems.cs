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

        private readonly LyricEditor lyricEditor;

        public TopMenuItems(MenuStrip topMenu, LyricEditor lyricEditor) 
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
        public FontSizeMenuItem(LyricEditor lyricEditor)
        {
            this.Text = "Font Size";
            this.DropDownItems.Add(new IncreaseFontSizeMenuItem(lyricEditor));
            this.DropDownItems.Add(new DecreaseFontSizeMenuItem(lyricEditor));
        }
    }

    public class DecreaseFontSizeMenuItem : ToolStripMenuItem
    {
        private readonly LyricEditor lyricEditor;
        public DecreaseFontSizeMenuItem(LyricEditor lyricEditor)
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
        private readonly LyricEditor lyricEditor;
        public IncreaseFontSizeMenuItem(LyricEditor lyricEditor)
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
        public NewMenuItem(LyricEditor lyricEditor)
        {
            this.Text = "New";
            this.DropDownItems.Add(new NewLyricMenuItem(lyricEditor));
            this.DropDownItems.Add(new NewSetListMenuItem(lyricEditor));    
        }        
    }

    public class NewLyricMenuItem : ToolStripMenuItem
    {
        private readonly LyricEditor lyricEditor;
        public NewLyricMenuItem(LyricEditor lyricEditor)
        {
            this.Text = "New Lyric";
            this.lyricEditor = lyricEditor;
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.lyricEditor.Initialise(string.Empty, LyricEditorType.Lyric, string.Empty);
        }
    }

    public class NewSetListMenuItem : ToolStripMenuItem
    {
        private readonly LyricEditor lyricEditor;
        public NewSetListMenuItem(LyricEditor lyricEditor)
        {
            this.Text = "New SetList";
            this.lyricEditor = lyricEditor;
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.lyricEditor.Initialise(string.Empty, LyricEditorType.SetList, string.Empty);
        }
    }

    public class FileMenuItem : ToolStripMenuItem
    {
        public FileMenuItem(LyricEditor lyricEditor)
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
        private readonly LyricEditor lyricEditor;
        public OpenLyricMenuItem(LyricEditor lyricEditor)
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
                this.lyricEditor.Initialise(System.IO.File.ReadAllText(openFileDialog.FileName), LyricEditorType.Lyric, openFileDialog.FileName);
            }
        }
    }

    public class SaveLyricMenuItem : ToolStripMenuItem
    {
        private readonly LyricEditor lyricEditor;

        public SaveLyricMenuItem(LyricEditor lyricEditor)
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
                    System.IO.File.WriteAllText(saveFileDialog.FileName, this.lyricEditor.Text);
                }
            }
            else
            {
                System.IO.File.WriteAllText(this.lyricEditor.FileName, this.lyricEditor.Text);
            }
        }
    }

    public class OpenSetListMenuItem : ToolStripMenuItem
    {
        private readonly LyricEditor lyricEditor;
        public OpenSetListMenuItem(LyricEditor lyricEditor)
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
                this.lyricEditor.Initialise(System.IO.File.ReadAllText(openFileDialog.FileName), LyricEditorType.SetList, openFileDialog.FileName);
            }
        }
    }

    public class SaveSetListMenuItem : ToolStripMenuItem
    {
        private readonly LyricEditor lyricEditor;

        public SaveSetListMenuItem(LyricEditor lyricEditor)
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
