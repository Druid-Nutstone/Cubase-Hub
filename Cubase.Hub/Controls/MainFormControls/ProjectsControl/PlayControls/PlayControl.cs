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

        private string musicFile;

        private IWavePlayer outputDevice;
        private AudioFileReader audioFile;

        private System.Windows.Forms.Timer timer;

        public PlayControl(string musicFile)
        {
            InitializeComponent();
            this.musicFile = musicFile; 
            this.Play.Click += Play_Click;
            this.Stop.Click += Stop_Click;
            this.Stop.Enabled = false;
            this.Progress.Minimum = 0;
            this.Progress.Maximum = 1000;
        }

        private void Stop_Click(object? sender, EventArgs e)
        {
            if (audioFile != null && outputDevice != null)
            {
                outputDevice.Stop();
            }
        }

        private void Play_Click(object? sender, EventArgs e)
        {
            outputDevice?.Stop();
            outputDevice?.Dispose();
            audioFile?.Dispose();
            audioFile = new AudioFileReader(this.musicFile); // supports MP3, WAV, FLAC, AIFF
            outputDevice = new WaveOutEvent();
            outputDevice.Init(audioFile);
            outputDevice.Play();
            outputDevice.PlaybackStopped += OutputDevice_PlaybackStopped;   
            this.Play.Enabled = false; 
            this.Stop.Enabled = true;
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 200; // ms
            timer.Tick += (s, e) =>
            {
                if (audioFile != null)
                {
                    double progress = audioFile.CurrentTime.TotalSeconds /
                                      audioFile.TotalTime.TotalSeconds;

                    // progress = 0.0 → 1.0
                    int value = (int)(progress * Progress.Maximum);

                    // Safety clamp
                    value = Math.Max(Progress.Minimum,
                            Math.Min(value, Progress.Maximum));
                    Progress.Value = value; 
                    Progress.DisplayText = audioFile.CurrentTime.ToString(@"mm\:ss");
                }
            };
            timer.Start();
        }

        private void OutputDevice_PlaybackStopped(object? sender, StoppedEventArgs e)
        {
            this.Play.Enabled = true;
            this.Stop.Enabled = false;
            timer?.Stop();
            Progress.DisplayText = "00:00";
            this.Progress.Value = 0;
        }
    }
}
