using Cubase.Macro.Common.Models.Lyrics;

namespace Cubase.Macro.Forms.Lyrics.Editor.New
{
    public class LyricRowCanvas : Control
    {
        private readonly string lineText;
        private readonly List<ChordSection> chords;
        private readonly int fontSize;
        private readonly int lineStartGlobalIndex;

        private Font chordFont = new Font("Consolas", 10, FontStyle.Bold);
        private Font lyricFont = new Font("Segoe UI", 12, FontStyle.Regular);

        public LyricRowCanvas(string text, List<ChordSection> lineChords, int globalStartIndex, int fontSize)
        {
            this.lineText = text ?? "";
            this.chords = lineChords ?? new List<ChordSection>();
            this.lineStartGlobalIndex = globalStartIndex;
            this.fontSize = fontSize;
            this.chordFont = new Font("Consolas", fontSize, FontStyle.Bold);
            this.lyricFont = new Font("Segoe UI", fontSize, FontStyle.Regular); // Fixed trailing 'a' typo here
            this.DoubleBuffered = true;
            this.Margin = new Padding(5, 0, 0, 2);
            this.CalculateRequiredBounds();
        }

        private void CalculateRequiredBounds()
        {
            // Measure the sizing bounds of our unbroken line text string
            Size baseLyricSize = TextRenderer.MeasureText(
                string.IsNullOrEmpty(lineText) ? " " : lineText,
                lyricFont,
                Size.Empty,
                TextFormatFlags.NoPadding
            );

            int requiredHeight = baseLyricSize.Height;

            // If this specific row line contains at least one chord, append vertical space
            if (chords.Count > 0)
            {
                Size sampleChordSize = TextRenderer.MeasureText("C", chordFont, Size.Empty, TextFormatFlags.NoPadding);
                requiredHeight = sampleChordSize.Height + 2 + baseLyricSize.Height;
            }

            // Lock this row control height dimensions so the parent TableLayoutPanel handles it smoothly
            this.Height = requiredHeight;
            this.Dock = DockStyle.Top;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            bool hasChords = chords.Count > 0;
            int lyricYPosition = 0;

            if (hasChords)
            {
                // Dynamically calculate the lyric Y position based on the actual chord font height + 2px padding
                Size sampleChordSize = TextRenderer.MeasureText("C", chordFont, Size.Empty, TextFormatFlags.NoPadding);
                lyricYPosition = sampleChordSize.Height + 2;
            }

            // 1. Draw the absolute unbroken raw lyric text string first
            TextRenderer.DrawText(
                e.Graphics,
                lineText,
                lyricFont,
                new Point(0, lyricYPosition),
                Color.FromArgb(230, 230, 230),
                TextFormatFlags.NoPadding
            );

            // 2. Map and overlay chords over characters dynamically
            foreach (var chordItem in chords)
            {
                // Translate global map data back into localized character string counts
                int localCharIndex = chordItem.CharacterIndex - lineStartGlobalIndex;

                // Guard against index clamping bugs
                if (localCharIndex < 0) localCharIndex = 0;
                if (localCharIndex > lineText.Length) localCharIndex = lineText.Length;

                // Substring extraction pass to measure the horizontal length leading up to this chord position
                string textLeadingUpToChord = lineText.Substring(0, localCharIndex);

                Size measuredOffset = TextRenderer.MeasureText(
                    e.Graphics,
                    textLeadingUpToChord,
                    lyricFont,
                    Size.Empty,
                    TextFormatFlags.NoPadding
                );

                // Calculate the horizontal pixel position. If index is 0, start at 0px.
                int chordXPixelPosition = localCharIndex == 0 ? 0 : measuredOffset.Width;

                // Paint the chord wrapped clean at the calculated horizontal coordinates
                TextRenderer.DrawText(
                    e.Graphics,
                    chordItem.Chord.ToLower(),
                    chordFont,
                    new Point(chordXPixelPosition, 0),
                    Color.FromArgb(220, 50, 50), // Muted crimson red chord color
                    TextFormatFlags.NoPadding
                );
            }
        }
    }
}