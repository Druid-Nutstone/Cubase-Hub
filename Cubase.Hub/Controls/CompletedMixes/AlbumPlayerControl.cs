using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Services.Album;
using Cubase.Hub.Services.Models;
using Cubase.Hub.Services.Track;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace Cubase.Hub.Controls.CompletedMixes
{
    public partial class AlbumPlayerControl : UserControl
    {
        private readonly IServiceProvider serviceProvider;

        private readonly IAlbumService albumService;

        private readonly ITrackService trackService;

        private AlbumLocation albumLocation;

        private MixDownCollection tracks;

        public AlbumPlayerControl()
        {
            InitializeComponent();
        }

        public AlbumPlayerControl(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            ThemeApplier.ApplyDarkTheme(this);
            this.serviceProvider = serviceProvider;
            this.albumService = this.serviceProvider.GetService<IAlbumService>();
            this.trackService = this.serviceProvider.GetService<ITrackService>();
            this.PlayTrackControl.TrackService = this.trackService;
            this.PlayTrackControl.ShowPlay = false;
            this.AlbumArtButton.Click += AlbumArt_Click;
            this.PlayAllButton.Click += PlayAllButton_Click;
        }

        private void PlayAllButton_Click(object? sender, EventArgs e)
        {
            if (this.tracks  != null)
            {
                this.PlayTrackControl.PlayTracks(this.tracks);  
            }
        }


        private void AlbumArt_Click(object? sender, EventArgs e)
        {
            var selectFile = new OpenFileDialog();
            selectFile.Filter = "PNG Image (*.png)|*.png|JPEG Image (*.jpg)|*.jpg|All Files (*.*)|*.*";
            if (selectFile.ShowDialog() == DialogResult.OK)
            {
                if (this.albumService.CopyAlbumArt(this.albumLocation, selectFile.FileName))
                {
                    this.GetAlbumArt();
                }
            }
        }

        public void Play(AlbumLocation albumLocation)
        {
            this.albumLocation = albumLocation;
            this.GetAlbumArt();
            // album Config 
            var albumDetail = this.albumService.GetAlbumConfigurationFromAlbumLocation(albumLocation);
            AlbumTitle.Bind(nameof(AlbumConfiguration.Title), albumDetail);
            AlbumArtist.Bind(nameof(AlbumConfiguration.Artist), albumDetail);
            AlbumYear.Bind(nameof(AlbumConfiguration.Year), albumDetail);
            AlbumGenre.Bind(nameof(AlbumConfiguration.Genre), albumDetail);
            AlbumComments.Bind(nameof(AlbumConfiguration.Comments), albumDetail);
            this.tracks = this.trackService.GetFinalMixesForAlbum(albumLocation);
            this.TrackPlayView.ShowMixes(tracks, this.serviceProvider, this.PlayTrackControl);
        }

        private void GetAlbumArt()
        {
            var albumArt = this.albumService.GetAlbumArt(albumLocation);
            if (albumArt != null)
            {
                AlbumArt.AlbumCoverFileName = albumArt;
                AlbumArt.RefreshImage();
            }
        }

    }
}
