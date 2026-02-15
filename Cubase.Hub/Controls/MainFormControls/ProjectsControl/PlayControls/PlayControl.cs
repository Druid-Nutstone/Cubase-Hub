using Cubase.Hub.Services.Audio;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using TagLib.Mpeg;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Cubase.Hub.Controls.MainFormControls.ProjectsControl.PlayControls
{
    public partial class PlayControl : UserControl
    {

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string MusicFile { get; set; }

        private System.Windows.Forms.Timer timer;

        private bool IsProgressMouseDragging = false;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IAudioService AudioService { get; set; }

        public PlayControl()
        {
            InitializeComponent();
            this.Initialise();
        }

        public PlayControl(IAudioService audioService)
        {
            InitializeComponent();
            this.AudioService = audioService;
            this.Initialise();

        }
        private void Initialise()
        {
            this.Play.Click += Play_Click;
            this.Stop.Click += Stop_Click;
            this.Progress.MouseDown += Progress_MouseDown;
            this.Progress.MouseMove += Progress_MouseMove;
            this.Progress.MouseUp += Progress_MouseUp;
            this.Progress.MouseEnter += (s, e) => { if (this.AudioService.Audio != null) this.Progress.Cursor = Cursors.VSplit; };
            this.Progress.MouseLeave += (s, e) => { this.Progress.Cursor = Cursors.Default; };
            this.Stop.Enabled = false;
            this.Progress.Minimum = 0;
            this.Progress.Maximum = 1000;
            this.Volume.ValueChanged += Volume_Scroll;
            AutoSize = true;
            Dock = System.Windows.Forms.DockStyle.Fill;
            Padding = new System.Windows.Forms.Padding(4);
        }

        private void Volume_Scroll(object? sender, EventArgs e)
        {
            this.AudioService.Player?.Volume = this.Volume.Value / 100f; // Assuming Volume is a TrackBar with values from 0 to 100 
        }

        private void Progress_MouseUp(object? sender, MouseEventArgs e)
        {
            this.IsProgressMouseDragging = false;
            this.Progress.Cursor = Cursors.Default;
        }

        private void Progress_MouseMove(object? sender, MouseEventArgs e)
        {
            if (!this.IsProgressMouseDragging && this.AudioService.Audio != null)
                return;
            this.SeekFromMouse(e.X);
        }

        private void Progress_MouseDown(object? sender, MouseEventArgs e)
        {
            this.IsProgressMouseDragging = true;
            this.Progress.Cursor = Cursors.VSplit;
        }

        private void SeekFromMouse(int mouseX)
        {
            if (this.AudioService.Audio == null)
                return;

            if (this.AudioService.Audio.CurrentTime == null)
                return;

            int width = this.Progress.Width;

            // Clamp mouse position
            mouseX = Math.Max(0, Math.Min(mouseX, width));

            double percent = (double)mouseX / width;

            this.AudioService.Audio?.CurrentTime =
                TimeSpan.FromMilliseconds(this.AudioService.Audio.TotalTime.TotalMilliseconds * percent);

            // Optional: update visual position
            this.Progress.Value = (int)(percent * this.Progress.Maximum);
        }

        private void Stop_Click(object? sender, EventArgs e)
        {
            this.AudioService.Stop();
        }

        private void Play_Click(object? sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.AudioService.Play(this.MusicFile, PlaybackStopped);
            this.Play.Enabled = false;
            this.Stop.Enabled = true;
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 200; // ms
            timer.Tick += (s, e) =>
            {
                if (this.AudioService.Audio != null)
                {
                    this.Volume.Value = (int)(this.AudioService.Player?.Volume * 100 ?? 0);
                    double progress = this.AudioService.Audio.CurrentTime.TotalSeconds /
                                      this.AudioService.Audio.TotalTime.TotalSeconds;

                    // progress = 0.0 → 1.0
                    int value = (int)(progress * Progress.Maximum);

                    // Safety clamp
                    value = Math.Max(Progress.Minimum,
                            Math.Min(value, Progress.Maximum));
                    Progress.Value = value;
                    Progress.DisplayText = this.AudioService.Audio.CurrentTime.ToString(@"mm\:ss");
                }
            };
            timer.Start();
            this.Cursor = Cursors.Default;
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
