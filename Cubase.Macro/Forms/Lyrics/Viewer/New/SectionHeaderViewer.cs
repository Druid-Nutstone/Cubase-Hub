using Cubase.Macro.Common.Models.Lyrics;

namespace Cubase.Macro.Forms.Lyrics.Editor.New
{
    public class SectionHeaderviewer : TableLayoutPanel
    {
        private readonly LyricSection section;
        private Label lblPlayIndicator;
        private Label lblTitle;
        private Label lblComments;

        public SectionHeaderviewer(LyricSection lyricSection) : base()
        {
            this.section = lyricSection ?? throw new ArgumentNullException(nameof(lyricSection));

            // 1. Configure the horizontal grid layout (3 Columns, 1 Row)
            this.ColumnCount = 3;
            this.RowCount = 1;

            this.Dock = DockStyle.Top;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.BackColor = Color.FromArgb(25, 20, 25); // Sleek DAW theme
            this.Padding = new Padding(10, 6, 10, 6);
            this.Margin = new Padding(0, 15, 0, 2); // Space above the section block
            this.DoubleBuffered = true;

            // 2. Column layout rules
            this.ColumnStyles.Clear();
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 25f));   // Col 0: Play Symbol (▶)
            this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));        // Col 1: Name + Bar (e.g. "VERSE 1 [B.12]")
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));   // Col 2: Comments (Fills remainder)

            this.RowStyles.Clear();
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            this.BuildHeader();
        }

        private void BuildHeader()
        {
            this.SuspendLayout();

            // ---- COLUMN 0: THE LIVE PLAYBACK SYMBOL ----
            lblPlayIndicator = new Label
            {
                Text = "", // Blank by default until activated by the clock
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 255, 120), // Vibrant DAW green play state
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(0)
            };

            // ---- COLUMN 1: SECTION TITLE & BAR CONTEXT ----
            string barText = this.section.Bar >= 0 ? $" [Bar {this.section.Bar}]" : "";
            lblTitle = new Label
            {
                Text = $"{this.section.Name.ToUpper()}{barText}",
                Font = new Font("Segoe UI", this.section.FontSize + 1, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 180, 0), // Clean orange/amber header accent color
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(5, 0, 15, 0) // Margin right gives breathing room before comments
            };

            // ---- COLUMN 2: MUSICAL COMMENTS / STRUCTURAL NOTES ----
            lblComments = new Label
            {
                Text = string.IsNullOrWhiteSpace(this.section.Comments) ? "" : $"// {this.section.Comments}",
                Font = new Font("Segoe UI", this.section.FontSize, FontStyle.Italic),
                ForeColor = Color.FromArgb(150, 145, 155), // Low-contrast muted gray for comments
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(0)
            };

            // Inject controls into Row 0 matrix cells [Column, Row]
            this.Controls.Add(lblPlayIndicator, 0, 0);
            this.Controls.Add(lblTitle, 1, 0);
            this.Controls.Add(lblComments, 2, 0);

            this.ResumeLayout(true);
        }

        /// <summary>
        /// Call this from your master clock tracking thread. 
        /// Lights up the green play arrow if this section is currently active.
        /// </summary>
        public void SetPlayState(bool isActive)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => SetPlayState(isActive)));
                return;
            }

            lblPlayIndicator.Text = isActive ? "▶" : "";

            // Optional visual highlight flare: tint background when song timeline is here
            this.BackColor = isActive
                ? Color.FromArgb(48, 43, 55)   // Brighter highlight zone
                : Color.FromArgb(38, 33, 40);  // Rest color state
        }
    }
}
