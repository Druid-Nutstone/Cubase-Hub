using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Services.Distributers.SoundCloud;
using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Hub.Forms.Distributers.SoundCloud
{
    public partial class SoundCloudMainControl : UserControl, IDistributerMainControl
    {
        private SoundCloudDistributionProvider soundCloud;

        private SoundCloudPlaylistCollection playLists;

        private AlbumConfiguration albumConfiguration;

        private MixDownCollection mixDowns;

        private SoundCloudDistributer parentForm;

        public SoundCloudMainControl(
               SoundCloudDistributer parentForm,
               SoundCloudDistributionProvider distributionProvider,
               SoundCloudPlaylistCollection playListCollection)
        {
            InitializeComponent();
            this.soundCloud = distributionProvider;
            this.playLists = playListCollection;
            this.parentForm = parentForm;
            this.UploadSelected.Click += UploadSelected_Click;
            this.OpenDistribution.Click += OpenDistribution_Click;
            this.CreateAlbum.Click += CreateAlbum_Click;
            ThemeApplier.ApplyDarkTheme(this);  
        }

        private void CreateAlbum_Click(object? sender, EventArgs e)
        {
            this.parentForm.CreateAlbum();
        }

        private void OpenDistribution_Click(object? sender, EventArgs e)
        {
            this.parentForm.OpenForm();
        }

        private void UploadSelected_Click(object? sender, EventArgs e)
        {
            this.parentForm.UploadSelected();
        }



        public void SetAlbum(AlbumConfiguration albumConfiguration, MixDownCollection mixDowns)
        {
            this.albumConfiguration = albumConfiguration;
            this.mixDowns = mixDowns;
            this.CreateAlbum.Visible = !playLists.HaveAlbum(albumConfiguration.Title);
        }
    }
}
