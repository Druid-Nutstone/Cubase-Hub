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
    public partial class CueMainControl : UserControl
    {
        private CueLevelCollection cueLevels;

        private int defaultCue = 0;

        private int currentCueSlot = -1;

        private int _lastSliderWidth = -1;

        private List<CueLevel>? ActiveCues = null;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<CueLevel, int> OnCueChanged { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action OnRefreshMixer { get; set; }

        public CueMainControl()
        {
            InitializeComponent();
            this.RefreshMixer.Click += (s, e) =>
            {
                this.OnRefreshMixer?.Invoke();
            };
        }


        public void UpdateCueLevels(CueLevelCollection cueLevels)
        {
            this.cueLevels = cueLevels;
            this.BuildMixer(defaultCue);
        }

        private void UpdateMixer(List<CueLevel> activeCues)
        {
            ActiveCues = activeCues;

            MainPanel.SuspendLayout();

            // Remove sliders that no longer exist
            var slidersToRemove =
                MainPanel.Controls
                    .OfType<CueSlider>()
                    .Where(slider =>
                        !activeCues.Any(c =>
                            c.TrackName == slider.CueLevel.TrackName))
                    .ToList();

            foreach (var slider in slidersToRemove)
            {
                MainPanel.Controls.Remove(slider);
                slider.Dispose();
            }

            // Add or update sliders
            foreach (var cue in activeCues)
            {
                var slider = FindSlider(cue);

                if (slider == null)
                {
                    slider = new CueSlider(cue, CueChanged);
                    MainPanel.Controls.Add(slider);
                }
                else
                {
                    slider.UpdateCueLevel(cue);
                }
            }

            ResizeSliders();

            MainPanel.ResumeLayout(true);

            MainPanel.PerformLayout();
            MainPanel.Refresh();
        }

        private CueSlider? FindSlider(CueLevel cueLevel)
        {
            foreach (Control control in MainPanel.Controls)
            {
                if (control is CueSlider slider && slider.CueLevel.TrackName == cueLevel.TrackName)
                {
                    slider.UpdateCueLevel(cueLevel);
                    return slider;
                }
            }
            return null;
        }

        private void BuildMixer(int cueSlot)
        {

            if (this.cueLevels.Count == 0)
                return;

            if (cueSlot >= this.cueLevels.Count)
                cueSlot = 0;

            this.currentCueSlot = cueSlot;

            var currentCue = this.cueLevels[cueSlot];

            var enabledLevels = currentCue.CueLevels.Where(x => x.Enabled).ToList();
            
            ActiveCues =
                enabledLevels
                    .OrderBy(x => x.TrackIndex)
                    .ToList();

            if (this.MainPanel.Controls.Count > 0)
            {
                this.UpdateMixer(ActiveCues);
                return;
            }

            MainPanel.SuspendLayout();
            this.MainPanel.Controls.Clear();
            foreach (var currentCueLevel in ActiveCues)
            {
                var slider = new CueSlider(currentCueLevel, this.CueChanged);
                this.MainPanel.Controls.Add(slider);
            }
            MainPanel.ResumeLayout(true);
            ResizeSliders();
            MainPanel.PerformLayout();
            MainPanel.Refresh();
        }

        private void ResizeSliders()
        {
            if (ActiveCues == null || ActiveCues.Count == 0)
                return;

            int sliderWidth = MainPanel.ClientSize.Width / ActiveCues.Count;

            int x = 0;

            foreach (Control control in MainPanel.Controls)
            {
                control.Dock = DockStyle.None;

                control.SetBounds(
                    x,
                    0,
                    sliderWidth,
                    MainPanel.ClientSize.Height);

                x += sliderWidth;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            MainPanel.SuspendLayout();
            this.ResizeSliders();
            MainPanel.ResumeLayout();
        }



        private void CueChanged(CueLevel cueLevel)
        {
            this.OnCueChanged?.Invoke(cueLevel, this.currentCueSlot);
        }



    }
}
