using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Macro.Forms.Lyrics.Editor
{
    public partial class LyricCompletetionForm : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<LyricControlCommand?> OnSelected { get; set; }

        private LyricCompletetionListBox lyricControl = new LyricCompletetionListBox();

        public LyricCompletetionForm()
        {
            InitializeComponent();
            lyricControl.Dock = DockStyle.Fill;
            this.MainPanel.Controls.Add(lyricControl);
            lyricControl.Populate(new LyricControlCommandCollection());
            this.MainPanel.BorderStyle = BorderStyle.FixedSingle;
            ThemeApplier.ApplyDarkTheme(this);
            lyricControl.OnSelected = ProcessSelected;
        }

        private void ProcessSelected(LyricControlCommand command)
        {
            this.OnSelected?.Invoke(command);
            this.Close();
        }
    }
}
