using Cubase.Macro.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Macro.Forms.Cues.CueControls
{
    public partial class CueSlider : UserControl
    {
        public CueLevel CueLevel { get; private set; }

        private int cueIndex;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<CueLevel> OnVolumeChanged { get; set; }

        public CueSlider(CueLevel cueLevel, Action<CueLevel> onVolumeChanged)
        {
            InitializeComponent();
            this.OnVolumeChanged = onVolumeChanged;
            this.Dock = DockStyle.Left;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.CueLevel = cueLevel;
            this.TrackName.Text = cueLevel.TrackName;
            this.VolumeText.Text = cueLevel.Volume.ToString("0.00");
            this.SliderPanel.Controls.Add(new CueTrackBar(cueLevel.Volume, this.VolumeChanged, this.VolumeChanging));
        }

        public void UpdateCueLevel(CueLevel cueLevel)
        {
            this.CueLevel = cueLevel;
            this.TrackName.Text = cueLevel.TrackName;
            this.VolumeText.Text = cueLevel.Volume.ToString("0.00");
            var cueTrackBar = this.SliderPanel.Controls[0] as CueTrackBar;
            cueTrackBar?.UpdateVolume(cueLevel.Volume);
        }

        private void VolumeChanging(double volume)
        {
            this.VolumeText.Text = volume.ToString("0.00");
        }

        private void VolumeChanged(double volume)
        {
            this.CueLevel.Volume = volume;
            this.OnVolumeChanged?.Invoke(this.CueLevel);

        }
    }
}
