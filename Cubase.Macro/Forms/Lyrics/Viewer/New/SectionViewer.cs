using System;
using System.Drawing;
using System.Windows.Forms;
using Cubase.Macro.Common.Models;

namespace Cubase.Macro.Forms.Lyrics.Editor.New
{
    public class SectionViewer : TableLayoutPanel
    {
        private readonly LyricSection section;

        public SectionViewer(LyricSection lyricSection) : base()
        {
            this.section = lyricSection ?? throw new ArgumentNullException(nameof(lyricSection));

            // 1. Two columns to match the 25px offset of the header marker column
            this.ColumnCount = 2;
            this.RowCount = 0;

            this.Dock = DockStyle.Top;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.BackColor = Color.FromArgb(25, 20, 25);
            this.Padding = new Padding(10, 0, 10, 0);
            this.Margin = new Padding(0);
            this.DoubleBuffered = true;

            this.ColumnStyles.Clear();
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 25f));  // Col 0: Play Symbol alignment space
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f)); // Col 1: High-Performance Canvas

            this.PopulateViewer();
        }

        private void PopulateViewer()
        {
            this.SuspendLayout();
            this.Controls.Clear();
            this.RowStyles.Clear();
            this.RowCount = 0;

            string[] cleanLines = this.section.GetRootLines();
            if (cleanLines == null || cleanLines.Length == 0) return;

            int runningGlobalCharCount = 0;

            foreach (var lineText in cleanLines)
            {
                this.RowCount++;

                // FIX: Explicitly set Height = 0 inside AutoSize RowStyle. 
                // This forces WinForms to strictly crop the row cell height to the child canvas control dimensions!
                this.RowStyles.Add(new RowStyle(SizeType.AutoSize, 0f));

                int currentRowIndex = this.RowCount - 1;

                // ---- COLUMN 0: THE ALIGNMENT SPACER ----
                // FIX: Force the spacer margin to zero so it doesn't inflate row bounds context
                Label lblSpacer = new Label { Text = "", AutoSize = true, Margin = new Padding(0), Padding = new Padding(0) };

                // ---- COLUMN 1: THE INTEGRATED CANVASES ----
                int lineStartGlobalIndex = runningGlobalCharCount;
                int lineEndGlobalIndex = lineStartGlobalIndex + lineText.Length;

                var lineChords = this.section.Chords
                    .Where(c => c.CharacterIndex >= lineStartGlobalIndex && c.CharacterIndex <= lineEndGlobalIndex)
                    .OrderBy(c => c.CharacterIndex)
                    .ToList();

                var lyricRowCanvas = new LyricRowCanvas(lineText, lineChords, lineStartGlobalIndex, this.section.FontSize);

                this.Controls.Add(lblSpacer, 0, currentRowIndex);
                this.Controls.Add(lyricRowCanvas, 1, currentRowIndex);

                runningGlobalCharCount += lineText.Length + Environment.NewLine.Length;
            }

            this.ResumeLayout(true);
        }

    }
}
