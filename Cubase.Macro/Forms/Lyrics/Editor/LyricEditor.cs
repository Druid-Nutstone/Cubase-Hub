using Cubase.Macro.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.ComponentModel;

namespace Cubase.Macro.Forms.Lyrics.Editor
{
    
    public enum LyricEditorType
    {
        Lyric,
        SetList,
        NotSpecified
    }

    public class LyricEditor : RichTextBox
    {

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LyricEditorType EditorType { get; private set; } = LyricEditorType.Lyric;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string FileName { get; private set; } = string.Empty;

        private const int WM_SETREDRAW = 0x000B;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(
            IntPtr hWnd,
            int msg,
            bool wParam,
            IntPtr lParam);


        public LyricEditor() : base() 
        { 
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 12);
        }

        public void Initialise(string text, LyricEditorType editorType, string fileName)
        {
            this.Text = text;
            this.EditorType = editorType;
            this.FileName = fileName;
            var metaData = this.ScanLyricsForAttributes();
            this.ContextMenuStrip = new LyricEditorContextMenu(this, metaData, editorType);
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
                this.ColorizeLines();
            }
        }

        private void ColorizeLines()
        {
            int originalSelectionStart = this.SelectionStart;
            int originalSelectionLength = this.SelectionLength;

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

                this.SelectionColor = line.StartsWith("{")
                    ? Color.Yellow
                    : Color.AntiqueWhite;

                // Find ALL [ ... ] pairs
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
                        // No closing ] found
                        end = line.Length - 1;
                    }

                    int length = (end - start) + 1;

                    this.Select(lineStart + start, length);
                    this.SelectionColor = Color.Red;

                    searchIndex = end + 1;
                }
            }

            // Restore caret
            this.Select(originalSelectionStart, originalSelectionLength);

            SendMessage(this.Handle, WM_SETREDRAW, true, IntPtr.Zero);

            this.Refresh();
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

            this.ColorizeLines();
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
            this.ColorizeLines();
        }

    }
}
