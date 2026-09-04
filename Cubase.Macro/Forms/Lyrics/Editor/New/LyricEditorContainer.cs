using Cubase.Macro.Common.Models;
using System.ComponentModel;

namespace Cubase.Macro.Forms.Lyrics.Editor.New
{
    public partial class LyricEditorContainer : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LyricContainer Lyrics { get; set; } = new LyricContainer();

        private SectionsControl SectionContainer = new SectionsControl();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<int> OnFontSizeChanged { get; set; }

        public LyricEditorContainer()
        {
            InitializeComponent();
            this.PopulateControls();
            this.Duration.ValueChanged += Duration_ValueChanged;
            this.Duration.MinDate = DateTime.MinValue;
            this.AddSection.Bind(this.AddSectionClick, Color.Green, "Add Section", "Adds a new section (verse/chorus etc)");
            this.SectionPanel.Controls.Add(this.SectionContainer);
        }

        private void AddSectionClick()
        {
            var newSection = LyricSection.Create("", 0, "Comments For this section");
            this.Lyrics.Sections.Add(newSection);
            this.SectionContainer.AddSection(newSection);
        }

        private void Duration_ValueChanged(object? sender, EventArgs e)
        {
            this.Lyrics.Duration = new TimeSpan(this.Duration.Value.Ticks);
        }

        public void Initialise(string fileName)
        {
            this.Lyrics = LyricContainer.Load(fileName, (err) =>
            {
                MessageBox.Show($"Error loading lyrics: {err}");
            });
            this.PopulateControls();
        }

        private void PopulateControls()
        {
            this.SongTitle.Bind(nameof(LyricContainer.Title), this.Lyrics, DataSourceUpdateMode.OnPropertyChanged, "Required - name of the song");
            this.Album.Bind(nameof(LyricContainer.Album), this.Lyrics, DataSourceUpdateMode.OnPropertyChanged, "Optional - album name");
            this.BPM.Bind(nameof(LyricContainer.Bpm), this.Lyrics, DataSourceUpdateMode.OnPropertyChanged, "Required - beats per minute");
            this.FontSize.Bind(nameof(LyricContainer.FontSize), this.Lyrics, DataSourceUpdateMode.OnPropertyChanged, "Required - font size");
            this.FontSize.TextChanged += (s, e) =>
            {
                if (int.TryParse(this.FontSize.Text, out int newSize))
                {
                    this.OnFontSizeChanged?.Invoke(newSize);
                }
                this.Lyrics.SetSectionFontSize(this.Lyrics.FontSize);
            };
            this.TimeSignature.Bind(nameof(LyricContainer.TimeSignature), this.Lyrics, DataSourceUpdateMode.OnPropertyChanged, "Required - time signature");

            if (this.Lyrics.Duration == TimeSpan.Zero)
            {
                this.Duration.Value = this.Duration.MinDate;
            }
            else
            {
                this.Duration.Value = new DateTime(this.Lyrics.Duration.Ticks);
            }

            foreach (var control in new Control[] { this.SongTitle, this.Album, this.BPM, this.TimeSignature, this.Duration, this.FontSize })
            {
                control.BackColor = System.Drawing.Color.FromArgb(42, 42, 42);
                control.ForeColor = System.Drawing.Color.White;
            }

            this.LoadSections();
        }

        public void LoadSections()
        {
            this.SectionContainer.Controls.Clear();
            foreach (var section in this.Lyrics.Sections)
            {
                this.SectionContainer.AddSection(section);
            }
        }
    }
}
