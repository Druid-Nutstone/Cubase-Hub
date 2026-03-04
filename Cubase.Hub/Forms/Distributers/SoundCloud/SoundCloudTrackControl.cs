using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Services.Distributers.SoundCloud;
using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Hub.Forms.Distributers.SoundCloud
{
    public partial class SoundCloudTrackControl : UserControl, IDistributerTrackControl
    {
        private readonly SoundCloudDistributionProvider soundCloudProvider;

        private readonly IServiceProvider serviceProvider;

        private SoundCloudTrackCollection soundCloudTracks;

        private SoundCloudTrack? soundCloudTrack;

        private SoundCloudDistributer mainControl;

        private MixDown mixDown;

        public SoundCloudTrackControl()
        {
            InitializeComponent();
        }

        public SoundCloudTrackControl(SoundCloudDistributionProvider soundCloudProvider, IServiceProvider serviceProvider, SoundCloudTrackCollection soundCloudTracks, SoundCloudDistributer parent)
        {
            InitializeComponent();
            this.soundCloudProvider = soundCloudProvider;
            this.serviceProvider = serviceProvider;
            this.soundCloudTracks = soundCloudTracks;
            this.mainControl = parent;
            ThemeApplier.ApplyDarkTheme(this);
        }

        private void InitialiseControls()
        {
            if (this.soundCloudTrack != null)
            {
                this.TrackOnSoundCloud.Text = $"{this.mixDown.Title} link";
                this.TrackOnSoundCloud.ForeColor = Color.LightBlue;
                this.TrackOnSoundCloud.Click += TrackOnSoundCloud_Click;
                this.CopyLink.GetClipBoardText = this.CopyLinkClick;
                this.ReUploadTrack.Click += ReUploadTrack_Click;
                this.DeleteTrack.Click += DeleteTrack_Click;
            }
            else
            {
                this.CopyLink.Visible = false;
                this.DeleteTrack.Visible = false;
                this.ReUploadTrack.Visible = false;
            }
        }

        private void DeleteTrack_Click(object? sender, EventArgs e)
        {
            this.mainControl.DeleteSingleTrack(this.mixDown);
        }

        private void ReUploadTrack_Click(object? sender, EventArgs e)
        {
            this.mainControl.UploadSingleTrack(this.mixDown);   
        }

        private string CopyLinkClick()
        {
            return this.soundCloudTrack?.PermalinkUrl;
        }

        private void TrackOnSoundCloud_Click(object? sender, EventArgs e)
        {
            Process p = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = this.soundCloudTrack?.PermalinkUrl,
                    UseShellExecute = true,
                }
            };
            p.Start();
        }

        public void SetMix(MixDown mixDown)
        {
            this.mixDown = mixDown;
            this.soundCloudTrack = this.soundCloudTracks.GetTrackByTitle(this.mixDown.Title);
            this.InitialiseControls();
        }
    }
}
