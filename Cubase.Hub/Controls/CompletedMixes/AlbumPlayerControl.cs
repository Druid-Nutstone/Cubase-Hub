using Cubase.Hub.Controls.Album.Manage;
using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Forms.Distributers;
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

        private readonly IDistributerForm? distributerForm;

        private AlbumLocation albumLocation;

        private MixDownCollection tracks;

        public AlbumPlayerControl()
        {
            InitializeComponent();
        }

        public AlbumPlayerControl(IServiceProvider serviceProvider, IDistributerForm? distributerForm)
        {
            InitializeComponent();
            ThemeApplier.ApplyDarkTheme(this);
            this.serviceProvider = serviceProvider;
            this.distributerForm = distributerForm;
            this.albumService = this.serviceProvider.GetService<IAlbumService>();
            this.trackService = this.serviceProvider.GetService<ITrackService>();
            this.PlayTrackControl.TrackService = this.trackService;
            this.PlayTrackControl.ShowPlay = false;
            this.AlbumArt.OnClicked = this.ChangeAlbumArt;
            this.PlayAllButton.Click += PlayAllButton_Click;
            this.SelectAllTracks.CheckedChanged += SelectAllTracks_CheckedChanged;
            if (distributerForm != null)
            {
                this.DistributerPanel.Controls.Clear();
                var distributerMainControl = this.distributerForm.MainControl;
                if (distributerMainControl != null)
                {
                    distributerMainControl.Dock = DockStyle.Fill;
                    this.DistributerPanel.Controls.Add(distributerMainControl);
                }
            }
            // global event monitor for commands from distributer controls  
            AlbumCommands.Instance.RegisterForAlbumCommand(this.WaitForAlbumCommands);
        }

        private void WaitForAlbumCommands(AlbumCommandType albumCommandType)
        {
            if (albumCommandType == AlbumCommandType.RefreshTracks)
            {

                this.Play(this.albumLocation);
            }
        }

        private void SelectAllTracks_CheckedChanged(object? sender, EventArgs e)
        {
            this.tracks.SelectDeSelectMixes(this.SelectAllTracks.Checked);
            this.TrackPlayView.ShowMixes(tracks, this.serviceProvider, this.PlayTrackControl, this.distributerForm);
        }

        private void PlayAllButton_Click(object? sender, EventArgs e)
        {
            if (this.tracks != null)
            {
                this.PlayTrackControl.PlayTracks(this.tracks);
            }
        }


        private void ChangeAlbumArt()
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
            AlbumEngineer.Bind(nameof(AlbumConfiguration.Engineer), albumDetail);
            AlbumProducer.Bind(nameof(AlbumConfiguration.Producer), albumDetail);
            AlbumLabel.Bind(nameof(AlbumConfiguration.Label), albumDetail);
            this.tracks = this.trackService.GetFinalMixesForAlbum(albumLocation);
            this.TrackPlayView.ShowMixes(tracks, this.serviceProvider, this.PlayTrackControl, this.distributerForm);
            this.distributerForm?.SetAlbum(albumDetail, this.tracks);
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
