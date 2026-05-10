using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Forms.Lyrics.Editor
{
    public class LyricEditorContextMenu : ContextMenuStrip
    {
        private readonly LyricEditor lyricEditor;

        private readonly LyricMetaData lyricMetaData;

        private readonly LyricEditorType editorType;

        public LyricEditorContextMenu(LyricEditor lyricEditor, LyricMetaData lyricMetaData, LyricEditorType editorType)
        {
            this.lyricEditor = lyricEditor;
            this.lyricMetaData = lyricMetaData;
            this.editorType = editorType;
            this.InitialiseMenus(); 
        }

        private void InitialiseMenus()
        {

            this.Items.Add(new PasteMenuItem(this.lyricEditor));
            if (this.editorType == LyricEditorType.Lyric)
            {
                this.Items.Add(new InsertLineMenuItem(this.lyricEditor, "Insert Title", "{title:}"));
                this.Items.Add(new InsertLineMenuItem(this.lyricEditor, "Insert Duration", "{duration:00:00}"));
                this.Items.Add(new InsertLineMenuItem(this.lyricEditor, "Insert Tempo (bpm)", "{tempo:120}"));
                this.Items.Add(new InsertLineMenuItem(this.lyricEditor, "Insert Relative Time for a part", "{d_time:00:00}"));
                this.Items.Add(new InsertLineMenuItem(this.lyricEditor, "Insert Pause (in seconds)", "{pause:1}"));
                this.Items.Add(new InsertAlbumMenu(this.lyricEditor, this.lyricMetaData));
                this.Items.Add(new InsertChordMenuitem(this.lyricEditor));
                this.Items.Add(new InsertLineMenuItem(this.lyricEditor, "Insert a Comment", "{comment:anytext}"));
                this.Items.Add(new InsertLineMenuItem(this.lyricEditor, "Insert a Verse", "Verse X:"));
                this.Items.Add(new InsertLineMenuItem(this.lyricEditor, "Insert a Chorus", "Chorus:"));
                this.Items.Add(new InsertLineMenuItem(this.lyricEditor, "Highlight Chorus Start", "{start_of_chorus}"));
                this.Items.Add(new InsertLineMenuItem(this.lyricEditor, "Highlight Chorus End", "{end_of_chorus}"));
                this.Items.Add(new InsertLineMenuItem(this.lyricEditor, "Insert a Bridge", "Bridge:"));
                this.Items.Add(new InsertLineMenuItem(this.lyricEditor, "Insert a Middle 8", "Middle 8:"));
            }
            if (this.editorType == LyricEditorType.SetList)
            {
                this.Items.Add(new InsertLineMenuItem(this.lyricEditor, "Insert Setlist Name", "--- SetList_Name_Here ---"));
                this.Items.Add(new InsertLineMenuItem(this.lyricEditor, "Insert Setlist comment", "- Comment_Here -"));
                this.Items.Add(new InsertLyricTitleMenu(this.lyricEditor, this.lyricMetaData));
            }
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

    public class PasteMenuItem : BaseMenuItem
    {
        public PasteMenuItem(LyricEditor lyricEditor) : base(lyricEditor, "Paste")
        {
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.lyricEditor.Paste();
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
