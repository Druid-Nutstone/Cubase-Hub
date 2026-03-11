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
            this.DeleteSelected.Click += DeleteSelected_Click;
            this.OpenAlbumLink.Click += OpenAlbumLink_Click;
            this.DeleteAlbum.Click += DeleteAlbum_Click;
            this.UpdateAlbum.Click += UpdateAlbum_Click;
            this.CopyLink.GetClipBoardText = this.CopyLinkClick;
            ThemeApplier.ApplyDarkTheme(this);
        }

        private void UpdateAlbum_Click(object? sender, EventArgs e)
        {
            this.parentForm.UpdateAlbum();
        }

        private void DeleteAlbum_Click(object? sender, EventArgs e)
        {
            this.parentForm.DeleteAlbum();
        }

        private string CopyLinkClick()
        {
            return this.GetAlbumUrl();
        }

        private void OpenAlbumLink_Click(object? sender, EventArgs e)
        {
            var url = GetAlbumUrl();
            if (!string.IsNullOrEmpty(url))
            {
                Process p = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = url,
                        UseShellExecute = true,
                    }
                };
                p.Start();
            }
        }

        private string? GetAlbumUrl()
        {
            var playLists = this.soundCloud.GetPlayLists((err) => { });
            if (playLists != null)
            {
                var album = playLists.GetAlbum(this.albumConfiguration.Title);
                if (album != null)
                {
                    return album.PermalinkUrl;
                }
                return null;
            }
            return null;
        }

        private void DeleteSelected_Click(object? sender, EventArgs e)
        {
            this.parentForm.DeleteSelected();
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
            this.OpenAlbumLink.Text = $"Open on SoundCloud";
        }
    }
}
