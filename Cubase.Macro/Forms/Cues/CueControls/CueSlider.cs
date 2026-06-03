using Cubase.Macro.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Resources;
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
            this.BindToggleButtons(cueLevel);
        }

        public void UpdateCueLevel(CueLevel cueLevel)
        {
            this.BindToggleButtons(cueLevel);
            this.CueLevel = cueLevel;
            this.TrackName.Text = cueLevel.TrackName;
            this.VolumeText.Text = cueLevel.Volume.ToString("0.00");
            var cueTrackBar = this.SliderPanel.Controls[0] as CueTrackBar;
            cueTrackBar?.UpdateVolume(cueLevel.Volume);
        }

        private void BindToggleButtons(CueLevel cueLevel)
        {
            this.MuteButton.Bind(nameof(cueLevel.Mute), cueLevel, Properties.Resources.MuteActive, Properties.Resources.MuteInactive); 
            // this.RecordButton.Bind(nameof(CueLevel.RecordEnable), cueLevel, Properties.Resources.RecordActive, Properties.Resources.RecordInactive);
            this.SoloButton.Bind(nameof(cueLevel.Solo), cueLevel, Properties.Resources.SoloActive, Properties.Resources.SoloInactive);
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
