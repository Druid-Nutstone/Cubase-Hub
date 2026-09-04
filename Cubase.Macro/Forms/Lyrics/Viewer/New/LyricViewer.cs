using Cubase.Macro.Common.Models;
using Cubase.Macro.Forms.Lyrics.Editor.New;

namespace Cubase.Macro.Forms.Lyrics.Viewer.New
{
    public class LyricViewer : TableLayoutPanel
    {
        public LyricViewer()
        {
            // 1. Force a strict single-column layout architecture
            this.ColumnCount = 1;
            this.RowCount = 0;

            // 2. Configure sizing styles so it matches parent panel width but stretches vertically
            this.Dock = DockStyle.Top;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            // 3. Define the single column to scale completely horizontally
            this.ColumnStyles.Clear();
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            // 4. CRUCIAL FIXES FOR ZERO VERTICAL PADDING / SPACING AT THE CONTAINER LEVEL
            this.Margin = new Padding(0);
            this.Padding = new Padding(0);

            // 5. Reduce layout flickering during structural section updates
            this.DoubleBuffered = true;
            this.BackColor = Color.FromArgb(25, 20, 25);
        }

        public void RefreshLyrics(LyricContainer lyricContainer)
        {
            this.Controls.Clear();
            this.RowStyles.Clear();
            this.AddSongTitle(lyricContainer);
            this.AddLyricSections(lyricContainer);
        }

        public void AddSongTitle(LyricContainer lyricContainer)
        {
            var titleLabel = new Label
            {
                Text = lyricContainer.Title,
                AutoSize = true,
                Font = new Font("Arial", lyricContainer.FontSize + 3, FontStyle.Bold),
                ForeColor = Color.Yellow,
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Dynamically scale title bottom padding relative to the font size (e.g., 2x font size)
            int titleBottomPadding = (int)(lyricContainer.FontSize * 2.0);
            this.AddControl(titleLabel, titleBottomPadding);
        }

        public void AddLyricSections(LyricContainer lyricContainer)
        {
            int fontSize = lyricContainer.FontSize;

            // Define proportional paddings based on font size
            int headerBottomPadding = (int)(fontSize * 0.4);   // Small gap between header and section lines
            int sectionBottomSpacing = (int)(fontSize * 5);  // Generous gap to clearly separate individual song sections

            foreach (var section in lyricContainer.Sections)
            {
                var sectionHeaderViewer = new SectionHeaderviewer(section);
                sectionHeaderViewer.Tag = section.Bar;
                this.AddControl(sectionHeaderViewer, headerBottomPadding);

                var sectionViewer = new SectionViewer(section);
                this.AddControl(sectionViewer, sectionBottomSpacing);
            }
        }

        private void AddControl(Control control, int bottomPadding = 0)
        {
            this.SuspendLayout();

            control.Margin = new Padding(0);
            control.Padding = new Padding(0, 0, 0, bottomPadding);
            control.Dock = DockStyle.Top;

            this.RowCount++;
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.Controls.Add(control, 0, this.RowCount - 1);

            this.ResumeLayout(true);
        }
    }
}