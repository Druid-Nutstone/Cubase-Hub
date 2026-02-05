using Cubase.Hub.Services.Audio;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Cubase.Hub.Controls.MainFormControls.ProjectsControl.PlayControls
{
    public partial class PlayControl : UserControl
    {

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string MusicFile { get; set; }

        private System.Windows.Forms.Timer timer;

        private readonly IAudioService audioService;

        public PlayControl(IAudioService audioService)
        {
            InitializeComponent();
            this.audioService = audioService;
            this.Play.Click += Play_Click;
            this.Stop.Click += Stop_Click;
            this.Stop.Enabled = false;
            this.Progress.Minimum = 0;
            this.Progress.Maximum = 1000;
            AutoSize = true;
            Dock = System.Windows.Forms.DockStyle.Fill;
            Padding = new System.Windows.Forms.Padding(4);
        }

        private void Stop_Click(object? sender, EventArgs e)
        {
            this.audioService.Stop();
        }

        private void Play_Click(object? sender, EventArgs e)
        {
            this.audioService.Play(this.MusicFile, PlaybackStopped);
            this.Play.Enabled = false; 
            this.Stop.Enabled = true;
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 200; // ms
            timer.Tick += (s, e) =>
            {
                if (this.audioService.Audio != null)
                {
                    double progress = this.audioService.Audio.CurrentTime.TotalSeconds /
                                      this.audioService.Audio.TotalTime.TotalSeconds;

                    // progress = 0.0 → 1.0
                    int value = (int)(progress * Progress.Maximum);

                    // Safety clamp
                    value = Math.Max(Progress.Minimum,
                            Math.Min(value, Progress.Maximum));
                    Progress.Value = value; 
                    Progress.DisplayText = this.audioService.Audio.CurrentTime.ToString(@"mm\:ss");
                }
            };
            timer.Start();
        }

        private void PlaybackStopped(StoppedEventArgs e)
        {
            this.Play.Enabled = true;
            this.Stop.Enabled = false;
            timer?.Stop();
            Progress.DisplayText = "00:00";
            this.Progress.Value = 0;
        }
    }
}
