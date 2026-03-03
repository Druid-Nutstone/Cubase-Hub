using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Services.Album;
using Cubase.Hub.Services.Distributers.SoundCloud;
using Cubase.Hub.Services.Messages;
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
    public partial class SoundCloudDistributer : BaseWindows11Form, IDistributerForm
    {
        private readonly IAlbumService albumService;

        private readonly IServiceProvider serviceProvider;
        
        private SoundCloudDistributionProvider soundCloud;

        private IMessageService messageService;

        private SoundCloudPlaylistCollection soundCloudAlbums;

        private SoundCloudTrackCollection soundCloudTracks;

        private SoundCloudMainControl mainControl;

        private SoundCloudTrackControl trackControl;

        private AlbumConfiguration albumConfiguration;

        private MixDownCollection mixDowns;

        private SoundCloudMessageForm? MsgHandler;

        public SoundCloudDistributer()
        {

            InitializeComponent();
        }

        public SoundCloudDistributer(
            IMessageService messageService,    
            IAlbumService albumService,
            IServiceProvider serviceProvider,
            SoundCloudDistributionProvider soundCloud) 
        {
            InitializeComponent();
            this.soundCloud = soundCloud;
            this.messageService = messageService;
            this.albumService = albumService;
            this.serviceProvider = serviceProvider;
            ThemeApplier.ApplyDarkTheme(this);
        }

        public void SetAlbum(AlbumConfiguration albumConfiguration, MixDownCollection mixDowns)
        {
            this.albumConfiguration = albumConfiguration; 
            this.mixDowns = mixDowns;
            this.mainControl.SetAlbum(albumConfiguration, mixDowns);
        }

        public UserControl MainControl 
        { 
            get
            {
                return this.mainControl;
            } 
            private set
            {

            }
        }

        public UserControl TrackControl
        {
            get
            {
                return new SoundCloudTrackControl(this.soundCloud, this.serviceProvider, this.soundCloudTracks);
            }
        }

        public void Initialise()
        {
            if (this.soundCloud.Connect(this.ShowSoundCloudError))
            {
                this.mainControl = new SoundCloudMainControl(this, this.soundCloud, this.soundCloudAlbums);
                this.soundCloudTracks = this.soundCloud.GetTracks(this.ShowSoundCloudError);
            }
        }

        public void UploadSelected()
        {
            var selectedMixes = this.mixDowns.GetSelectedMixes();
            if (selectedMixes.Count == 0)
            {
                this.messageService.ShowError($"No tracks have been selected");
            }
            this.MsgHandler = new SoundCloudMessageForm();
            this.MsgHandler.Show();
            var targetPlayListAlbum = selectedMixes.First().Album;
            foreach (var selectedTrack in selectedMixes)
            {
                this.MsgHandler.ShowMessage($"Locating album {selectedTrack.Album}");
                var album = this.soundCloud.GetAlbum(selectedTrack, this.ShowSoundCloudError);
                if (album == null)
                {
                    this.MsgHandler.ShowMessage($"Creating album {selectedTrack.Album}");
                    var allAlbums = this.albumService.GetAlbumList(this.ShowSoundCloudError);
                    var targetAlbum = allAlbums.FirstOrDefault(x => x.AlbumName == selectedTrack.Album);
                    if (targetAlbum == null)
                    {
                        this.ShowSoundCloudError($"Album {selectedTrack.Album} is not an album created in cubase");
                        return;
                    }
                    var albumConfig = this.albumService.GetAlbumConfigurationFromAlbumLocation(targetAlbum);
                    album = this.soundCloud.CreateAlbum(albumConfig, this.ShowSoundCloudError);
                    if (album == null)
                    {
                        return;
                    }

                }
                this.MsgHandler.ShowMessage($"Uploading {selectedTrack.Title} this.may take some time");
                this.soundCloud.UploadTrack(selectedTrack, this.ShowSoundCloudError);
            }
            if (this.MsgHandler != null)
            {
                this.soundCloud.OrderAlbumTracks(targetPlayListAlbum, this.ShowSoundCloudError, this.MsgHandler.ShowMessage);
                this.MsgHandler?.Close();
            }
        }

        public void OpenForm()
        {

        }

        public void DeleteSelected()
        {
            var selectedMixes = this.mixDowns.GetSelectedMixes();
            if (selectedMixes.Count == 0)
            {
                this.messageService.ShowError($"No tracks have been selected");
            }
            this.MsgHandler = new SoundCloudMessageForm();
            this.MsgHandler.Show();
            var targetPlayListAlbum = selectedMixes.First().Album;
            foreach (var selectedTrack in selectedMixes)
            {
                this.soundCloud.DeleteTrack(selectedTrack, this.ShowSoundCloudError);
            }
            this.soundCloud.OrderAlbumTracks(targetPlayListAlbum, this.ShowSoundCloudError, this.MsgHandler.ShowMessage);
            this.MsgHandler.Close();
        }


        private void ShowSoundCloudError(string error)
        {
           if (this.MsgHandler != null)
            {
                this.MsgHandler.Close();
                this.MsgHandler = null;
            }
            this.messageService.ShowError(error); 
        }
    }
}
