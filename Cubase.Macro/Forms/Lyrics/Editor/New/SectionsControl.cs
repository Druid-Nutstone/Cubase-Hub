using Cubase.Macro.Common.Models;

namespace Cubase.Macro.Forms.Lyrics.Editor.New
{
    public class SectionsControl : TableLayoutPanel
    {
        public SectionsControl() : base()
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

            // 4. Reduce layout flickering during structural section updates
            this.DoubleBuffered = true;
        }

        public void AddSection(LyricSection section)
        {
            this.SuspendLayout();

            var sectionControl = new SectionControl(section);


            // Crucial: Tell WinForms to dynamically compute this child's height 
            sectionControl.AutoSize = true;
            sectionControl.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            sectionControl.BorderStyle = BorderStyle.FixedSingle;
            // Force it to fill the entire horizontal space of the column
            sectionControl.Dock = DockStyle.Top;

            this.RowCount++;
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.Controls.Add(sectionControl, 0, this.RowCount - 1);

            this.ResumeLayout(true);
        }
    }
}
