using Cubase.Hub.Services.Models;
using Cubase.Hub.Services.Track;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Hub.Controls.Media.Play
{
    public partial class PlayTrackControl : UserControl
    {
        private enum TrackType
        {
            None,
            Single,
            Multiple
        }
        
        private TrackType trackType;    

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ITrackService TrackService {  get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MixDown MixDown { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ShowPlay { get; set; } = true;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ShowStop { get; set; } = true;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action? OnStopped { get; set; }

        private System.Windows.Forms.Timer timer;

        private bool IsProgressMouseDragging = false;

        private MixDownCollection mixDownCollection;

        private int currentTrackIndex = 0;

        public PlayTrackControl()
        {
            InitializeComponent();
            this.Play.Click += Play_Click;
            this.Stop.Click += Stop_Click;
            this.Progress.MouseDown += Progress_MouseDown;
            this.Progress.MouseMove += Progress_MouseMove;
            this.Progress.MouseUp += Progress_MouseUp;
            this.Progress.MouseEnter += (s, e) => { if (this.TrackService.Audio != null) this.Progress.Cursor = Cursors.VSplit; };
            this.Progress.MouseLeave += (s, e) => { this.Progress.Cursor = Cursors.Default; };
            this.Stop.Enabled = true;
            this.TrackName.Text = string.Empty;
            this.Progress.Minimum = 0;
            this.Progress.Maximum = 1000;
            this.Volume.ValueChanged += Volume_Scroll;
            this.Volume.Minimum = 0;
            this.Volume.Maximum = 100;
            this.Volume.Value = 100;
            this.Play.Visible = false;
        }

        private void Volume_Scroll(object? sender, EventArgs e)
        {
            this.TrackService?.Player?.Volume = this.Volume.Value / 100f; // Assuming Volume is a TrackBar with values from 0 to 100 
        }

        private void Progress_MouseUp(object? sender, MouseEventArgs e)
        {
            this.IsProgressMouseDragging = false;
            this.Progress.Cursor = Cursors.Default;
        }

        private void Progress_MouseMove(object? sender, MouseEventArgs e)
        {
            if (!this.IsProgressMouseDragging && this.TrackService.Audio != null)
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
            if (this.TrackService.Audio == null)
                return;

            if (this.TrackService.Audio.CurrentTime == null)
                return;

            int width = this.Progress.Width;

            // Clamp mouse position
            mouseX = Math.Max(0, Math.Min(mouseX, width));

            double percent = (double)mouseX / width;

            this.TrackService.Audio?.CurrentTime =
                TimeSpan.FromMilliseconds(this.TrackService.Audio.TotalTime.TotalMilliseconds * percent);

            // Optional: update visual position
            this.Progress.Value = (int)(percent * this.Progress.Maximum);
        }

        public void PlayTracks(MixDownCollection mixDowns) 
        {
            this.trackType = TrackType.Multiple;
            currentTrackIndex = 0;
            this.mixDownCollection = mixDowns;
            this.PlayTrack(this.mixDownCollection[0]);
        
        }

        public void PlayTrack(MixDown mixDown, Action? onStopped = null)
        {
            if (this.trackType != TrackType.Multiple)
            {
                this.trackType = TrackType.Single;
            }
            this.OnStopped = onStopped;
            this.Play.Visible = this.ShowPlay;
            this.MixDown = mixDown; 
            this.Cursor = Cursors.WaitCursor;
            this.TrackService.PlayTrack(mixDown, PlaybackStopped);
            this.Play.Enabled = false;
            this.Stop.Enabled = true;
            this.TrackName.Text = $"{mixDown.Title} ({mixDown.Duration} #{mixDown.TrackNumber})";
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 200; // ms
            timer.Tick += (s, e) =>
            {
                if (this.TrackService.Audio != null)
                {
                    this.Volume.Value = (int)(this.TrackService.Player?.Volume * 100 ?? 0);
                    double progress = this.TrackService.Audio.CurrentTime.TotalSeconds /
                                      this.TrackService.Audio.TotalTime.TotalSeconds;

                    // progress = 0.0 → 1.0
                    int value = (int)(progress * Progress.Maximum);

                    // Safety clamp
                    value = Math.Max(Progress.Minimum,
                            Math.Min(value, Progress.Maximum));
                    Progress.Value = value;
                    Progress.DisplayText = this.TrackService.Audio.CurrentTime.ToString(@"mm\:ss");
                }
            };
            timer.Start();
            this.Cursor = Cursors.Default;
        }

        private void Stop_Click(object? sender, EventArgs e)
        {
            this.TrackName.Text = string.Empty;
            this.TrackService.StopTrack();
        }

        private void Play_Click(object? sender, EventArgs e)
        {
            if (this.MixDown != null)
            {
                this.PlayTrack(this.MixDown);
            }
        }

        private void PlaybackStopped(StoppedEventArgs e)
        {
            timer?.Stop();
            Progress.DisplayText = "00:00";
            this.Progress.Value = 0;
            if (this.OnStopped !=null)
            {
                this.OnStopped.Invoke();
            }
            if (this.trackType == TrackType.Multiple)
            {
                if (this.currentTrackIndex < this.mixDownCollection.Count-1) 
                {
                    this.currentTrackIndex++;
                    this.PlayTrack(this.mixDownCollection[this.currentTrackIndex]);
                }
                else
                {
                    this.trackType = TrackType.None;
                }
            }
            else
            {
                this.trackType = TrackType.None;
            }
        }
    }

}
