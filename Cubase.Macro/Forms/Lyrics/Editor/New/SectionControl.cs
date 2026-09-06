using Cubase.Macro.Common.Models.Lyrics;

namespace Cubase.Macro.Forms.Lyrics.Editor.New
{
    public partial class SectionControl : UserControl
    {
        private LyricSection section = new LyricSection();

        public SectionControl() : base()
        {
            InitializeComponent();
            this.PopulateControls();
        }

        public SectionControl(LyricSection section) : base()
        {
            InitializeComponent();
            this.section = section;
            this.PopulateControls();
        }

        private void PopulateControls()
        {
            this.BackColor = Color.FromArgb(30, 25, 30);

            // 1. Force the SectionControl itself to scale vertically
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            // 2. Force the inner LyricPanel container to adapt dynamically
            this.LyricPanel.AutoSize = true;
            this.LyricPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            this.SectionName.Bind(nameof(LyricSection.Name), this.section);
            this.SectionBar.Bind(nameof(LyricSection.Bar), this.section);
            this.SectionComments.Bind(nameof(LyricSection.Comments), this.section);

            foreach (var control in new Control[] { this.SectionName, this.SectionBar, this.SectionComments })
            {
                control.BackColor = System.Drawing.Color.FromArgb(42, 42, 42);
                control.ForeColor = System.Drawing.Color.White;
            }

            this.LyricPanel.Controls.Clear();

            // 3. Create the editor instance explicitly
            var editor = new LyricTextEditor(this.section);

            // Ensure the editor fills the horizontal layout of the panel but allows height manipulation
            editor.Dock = DockStyle.Top;

            this.LyricPanel.Controls.Add(editor);
        }

    }
}
