using Cubase.Hub.Controls.Media.Play;
using Cubase.Hub.Services.Models;
using Cubase.Hub.Services.Track;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Hub.Controls.CompletedMixes.Tracks
{
    public partial class TrackPlayViewControl : UserControl
    {
        private readonly IServiceProvider serviceProvider;

        private readonly PlayTrackControl playTrackControl;

        private readonly ITrackService trackService;

        private MixDown mixDown;

        public TrackPlayViewControl()
        {
            InitializeComponent();
        }

        public TrackPlayViewControl(MixDown mixDown, IServiceProvider serviceProvider, PlayTrackControl playTrackControl)
        {
            this.serviceProvider = serviceProvider;
            this.playTrackControl = playTrackControl;
            this.trackService = serviceProvider.GetService<ITrackService>();
            this.mixDown = mixDown;
            InitializeComponent();
            this.InitialiseMixDown();
            this.LoadArtButton.Click += LoadArtButton_Click;
        }

        private void LoadArtButton_Click(object? sender, EventArgs e)
        {
            var selectFile = new OpenFileDialog();
            selectFile.Filter = "PNG Image (*.png)|*.png|JPEG Image (*.jpg)|*.jpg|All Files (*.*)|*.*";
            if (selectFile.ShowDialog() == DialogResult.OK)
            {
                this.trackService.CopyTrackArt(this.mixDown, selectFile.FileName);
                this.MixCover.TrackCoverFileName = this.GetTrackCover();
                this.MixCover.RefreshImage();
            }
        }

        private void InitialiseMixDown()
        {
            this.MixTitle.Bind(nameof(MixDown.Title), this.mixDown);
            this.MixTitle.Cursor = Cursors.Hand;
            this.MixTitle.Click += MixTitle_Click;
            this.MixTrackNo.Bind(nameof(MixDown.TrackNumber), this.mixDown);
            this.MixDuration.Bind(nameof(MixDown.Duration), this.mixDown);
            this.MixAudioType.Bind(nameof(MixDown.AudioType), this.mixDown);
            this.MixBitrate.Bind(nameof(MixDown.BitRate), this.mixDown);
            this.MixSampleRate.Bind(nameof(MixDown.SampleRate), this.mixDown);
            this.MixSize.Bind(nameof(MixDown.Size), this.mixDown);
            this.MixPerformers.Bind(nameof(MixDown.Performers), this.mixDown);
            this.MixCover.TrackCoverFileName = this.GetTrackCover();
        }

        private string? GetTrackCover()
        {
            return this.trackService.GetTrackCoverArt(this.mixDown);
        }

        private void MixTitle_Click(object? sender, EventArgs e)
        {
            this.playTrackControl.PlayTrack(this.mixDown);
        }

    }
}
