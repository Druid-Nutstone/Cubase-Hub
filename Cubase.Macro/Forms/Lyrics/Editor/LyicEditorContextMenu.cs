using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Forms.Lyrics.Editor
{
    public class LyricEditorContextMenu : ContextMenuStrip
    {
        private readonly LyricEditor lyricEditor;

        private readonly LyricMetaData lyricMetaData;

        public LyricEditorContextMenu(LyricEditor lyricEditor, LyricMetaData lyricMetaData)
        {
            this.lyricEditor = lyricEditor;
            this.lyricMetaData = lyricMetaData;
            this.InitialiseMenus(); 
        }

        private void InitialiseMenus()
        {
            this.Items.Add(new CopyMenuItem(this.lyricEditor));
            this.Items.Add(new PasteMenuItem(this.lyricEditor));
            this.Items.Add(new InsertAlbumMenu(this.lyricEditor, this.lyricMetaData));
        }
    }



    public class BaseMenuItem : ToolStripMenuItem
    {
        protected readonly LyricEditor lyricEditor;
        public BaseMenuItem(LyricEditor lyricEditor, string text) : base(text)
        {
            this.lyricEditor = lyricEditor;
        }
    }

    public class InsertLyricTitleMenu : BaseMenuItem
    {
        public InsertLyricTitleMenu(LyricEditor lyricEditor, LyricMetaData lyricMetaData) : base(lyricEditor, "Insert Track (Lyric)")
        {
            foreach (var title in lyricMetaData.SongTitles)
            {
                this.DropDownItems.Add(new InsertLineMenuItem(this.lyricEditor, title.Title, title.FileName));
            }
        }
    }

    public class InsertAlbumMenu: BaseMenuItem
    {
        public InsertAlbumMenu(LyricEditor lyricEditor, LyricMetaData lyricMetaData) : base(lyricEditor, "Insert Album")
        {
           foreach (var album in lyricMetaData.Albums)
           {
                this.DropDownItems.Add(new InsertExistingAlbumMenuItem(lyricEditor, album));
           }
           this.DropDownItems.Add(new InsertLineMenuItem(this.lyricEditor, "Insert New Album", "{book:}"));
        }
    } 

    public class InsertExistingAlbumMenuItem : BaseMenuItem
    {
        private readonly string albumName;
        public InsertExistingAlbumMenuItem(LyricEditor lyricEditor, string albumName) : base(lyricEditor, albumName)
        {
            this.albumName = albumName;
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.lyricEditor.InsertLine($"{{book:{this.albumName}}}");
        }
    }

    public class InsertChordMenuitem : BaseMenuItem
    {
        public InsertChordMenuitem(LyricEditor lyricEditor) : base(lyricEditor, "Insert Chord")
        {
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.lyricEditor.InsertAtPointer("[]");
        }
    }

    public class CopyMenuItem : BaseMenuItem
    {
        public CopyMenuItem(LyricEditor lyricEditor) : base(lyricEditor, "Copy")
        {
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.lyricEditor.Copy();
        }
    }

    public class PasteMenuItem : BaseMenuItem
    {
        public PasteMenuItem(LyricEditor lyricEditor) : base(lyricEditor, "Paste")
        {
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.lyricEditor.Paste();
            this.lyricEditor.ColourCode();
        }
    }

    public class InsertLineMenuItem : BaseMenuItem
    {
        private readonly string value;

        public InsertLineMenuItem(LyricEditor lyricEditor, string text, string value) : base(lyricEditor, text)
        {
            this.value = value;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.lyricEditor.InsertLine(this.value);
        }
    }
}
