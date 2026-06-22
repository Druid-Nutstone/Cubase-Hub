using Cubase.Macro.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.ComponentModel;
using System.Windows;
using Cubase.Macro.Common.Lyrics.Services;
using Cubase.Macro.Services.Midi;

namespace Cubase.Macro.Forms.Lyrics.Editor
{
    
    public class LyricEditor : BaseRichEdit
    {

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string FileName { get; private set; } = string.Empty;

        private ILyricService lyricService;

        private IMidiService midiService;

        public LyricEditor(ILyricService lyricService, IMidiService midiService) : base() 
        { 
            this.lyricService = lyricService;
            this.midiService = midiService;
        }

        public void Initialise(IEnumerable<string> sourceCode, string fileName)
        {
            this.Lines = sourceCode.ToArray();
            this.FileName = fileName;
            var metaData = this.ScanLyricsForAttributes();
            this.ContextMenuStrip = new LyricEditorContextMenu(this, metaData);
            this.RefreshContent();
        }

        private LyricMetaData ScanLyricsForAttributes()
        {
            var metaData = new LyricMetaData();
            var allLyrics = Directory.GetFiles(CubaseMacroConstants.DropBoxBaseDirectory, "*.txt", new EnumerationOptions() { RecurseSubdirectories = true });
            foreach (var lyric in allLyrics)
            {
                var allData = File.ReadAllLines(lyric);

                foreach (var line in allData)
                {
                    if (line.StartsWith("{title:"))
                    {
                        string title = line.Substring("{title:".Length).TrimEnd('}');
                        if (!string.IsNullOrWhiteSpace(title))
                        {
                            metaData.SongTitles.Add(new TitleMetaData() { FileName = Path.GetFileNameWithoutExtension(lyric), Title = title});
                        }

                    }
                    if (line.StartsWith("{book:"))
                    {
                        string album = line.Substring("{book:".Length).TrimEnd('}');
                        if (!string.IsNullOrWhiteSpace(album))
                        {
                            if (!metaData.Albums.Contains(album))
                            {
                                metaData.Albums.Add(album);
                            }

                        }
                    }
                }
            }
            return metaData;
        } 

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Enter)
            {
                this.RefreshContent();
            }
        }

        protected override void RefreshContent()
        {
            int originalSelectionStart = this.SelectionStart;
            int originalSelectionLength = this.SelectionLength;

            // Save scroll position
            POINT scrollPoint = new POINT();
            SendMessage(this.Handle, EM_GETSCROLLPOS, IntPtr.Zero, ref scrollPoint);

            // Stop redraw
            SendMessage(this.Handle, WM_SETREDRAW, false, IntPtr.Zero);

            for (int lineIndex = 0; lineIndex < this.Lines.Length; lineIndex++)
            {
                string line = this.Lines[lineIndex];

                int lineStart = this.GetFirstCharIndexFromLine(lineIndex);

                if (lineStart < 0)
                {
                    continue;
                }

                // Base color for whole line
                this.Select(lineStart, line.Length);

                var startsWithControlCharacter =
                    line.StartsWith("{") || line.EndsWith(":");

                this.SelectionColor = startsWithControlCharacter
                    ? Color.Yellow
                    : Color.AntiqueWhite;

                if (!startsWithControlCharacter)
                {
                    int searchIndex = 0;

                    while (searchIndex < line.Length)
                    {
                        int start = line.IndexOf('[', searchIndex);

                        if (start == -1)
                        {
                            break;
                        }

                        int end = line.IndexOf(']', start);

                        if (end == -1)
                        {
                            end = line.Length - 1;
                        }

                        int length = (end - start) + 1;

                        this.Select(lineStart + start, length);
                        this.SelectionColor = Color.Red;

                        searchIndex = end + 1;
                    }
                }
            }

            // Restore caret/selection
            this.Select(originalSelectionStart, originalSelectionLength);

            // Restore scroll position
            SendMessage(this.Handle, EM_SETSCROLLPOS, IntPtr.Zero, ref scrollPoint);

            // Re-enable redraw
            SendMessage(this.Handle, WM_SETREDRAW, true, IntPtr.Zero);

            this.Invalidate();
        }

        public void InsertAtPointer(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            int pos = this.SelectionStart;

            this.Text = this.Text.Insert(pos, text);

            // Move caret to end of inserted text
            this.SelectionStart = pos + 1;
            this.SelectionLength = 0;

            this.RefreshContent();
            // Optional: keep focus in editor
            this.Focus();
        }

        public void InsertLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return;
            }

            int pos = this.SelectionStart;

            bool needsNewLine =
                pos > 0 &&
                this.Text[pos - 1] != '\n';

            string prefix = needsNewLine
                ? Environment.NewLine
                : string.Empty;

            string textToInsert = prefix + line;

            this.Text = this.Text.Insert(pos, textToInsert);
            this.SelectionStart = pos + textToInsert.Length;
            this.RefreshContent();
        }

    }
}
