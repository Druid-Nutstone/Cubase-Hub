using Cubase.Hub.Controls.Album.Manage;
using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Services.Album;
using Cubase.Hub.Services.Background;
using Cubase.Hub.Services.Distributers.SoundCloud;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Models;
using Cubase.Hub.Services.Synchronise;
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
        
        private readonly ISynchroniseService synchroniseService;

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
            ISynchroniseService synchroniseService,
            SoundCloudDistributionProvider soundCloud) 
        {
            InitializeComponent();
            this.soundCloud = soundCloud;
            this.messageService = messageService;
            this.albumService = albumService;
            this.serviceProvider = serviceProvider;
            this.synchroniseService = synchroniseService;
            this.synchroniseService.RegisterForEvent(this.SoundCloudHasBeenUpdated);
            ThemeApplier.ApplyDarkTheme(this);
        }

        private void SoundCloudHasBeenUpdated(SyncEvent syncEvent)
        {
            if (syncEvent == SyncEvent.DistributionMixUpload)
            {
                this.RefreshTrackList();
            }
        }

        public void SetAlbum(AlbumConfiguration albumConfiguration, MixDownCollection mixDowns)
        {
            this.RefreshTrackList();
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
                return new SoundCloudTrackControl(this.soundCloud, this.serviceProvider, this.soundCloudTracks, this);
            }
        }

        public void DeleteSingleTrack(MixDown mixDown)
        {
            this.DeleteSelected(mixDown);
        }

        public void UploadSingleTrack(MixDown mixDown)
        {
            this.UploadSelected(mixDown);   
        }

        public void Initialise()
        {
            if (this.soundCloud.Connect(this.ShowSoundCloudError))
            {
                this.mainControl = new SoundCloudMainControl(this, this.soundCloud, this.soundCloudAlbums);
                this.RefreshTrackList();
            }
        }

        private void RefreshTrackList()
        {
            this.soundCloudTracks = this.soundCloud.GetTracks(this.ShowSoundCloudError);
        }

        private void RefreshAllTracks()
        {
            this.RefreshTrackList();
            AlbumCommands.Instance.RefreshTracks();
        }

        public void UploadSelected(MixDown? singleTrack = null)
        {
            var selectedMixes = singleTrack != null ? MixDownCollection.CreateFromSingleMix(singleTrack)
                                                    : this.mixDowns.GetSelectedMixes();
            if (selectedMixes.Count == 0)
            {
                this.messageService.ShowError($"No tracks have been selected");
                return; 
            }
            this.MsgHandler = new SoundCloudMessageForm();
            this.MsgHandler.Show();
            
            SoundCloudPlaylist? album = null;

            if (selectedMixes.Count > 0)
            {
                var targetPlayListAlbum = selectedMixes.First().Album;
                foreach (var selectedTrack in selectedMixes)
                {
                    this.MsgHandler.ShowMessage($"Locating album {selectedTrack.Album}");
                    album = this.soundCloud.GetAlbum(selectedTrack, this.ShowSoundCloudError);
                    if (album == null)
                    {
                        this.MsgHandler.ShowMessage($"Creating album {selectedTrack.Album}");
                        var albumConfig = GetAlbumConfiguration(selectedTrack.Album);
                        if (albumConfig == null)
                        {
                            return;
                        }
                        album = this.soundCloud.CreateAlbum(albumConfig, this.soundCloud.CreateAlbumComments(albumConfig, this.mixDowns), this.ShowSoundCloudError);
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
                    // reorder tracks in track order 
                    this.soundCloud.OrderAlbumTracks(targetPlayListAlbum, this.ShowSoundCloudError, this.MsgHandler.ShowMessage);
                    // refresh artist list on 
                    if (album != null)
                    {
                        var albumConfig = GetAlbumConfiguration(album?.Title);
                        this.soundCloud.UpdateAlbum(album, albumConfig, this.soundCloud.CreateAlbumComments(albumConfig, this.mixDowns), this.ShowSoundCloudError);
                    }
                    this.MsgHandler?.Close();
                    this.RefreshAllTracks();
                }
            }
            AlbumConfiguration? GetAlbumConfiguration(string albumName)
            {
                var allAlbums = this.albumService.GetAlbumList(this.ShowSoundCloudError);
                var targetAlbum = allAlbums.FirstOrDefault(x => x.AlbumName == albumName);
                if (targetAlbum == null)
                {
                    this.ShowSoundCloudError($"Album {albumName} is not an album created in cubase");
                    return null;
                }
                return this.albumService.GetAlbumConfigurationFromAlbumLocation(targetAlbum);
            }
        }

        public void OpenForm()
        {

        }

        public void UpdateAlbum()
        {
            this.MsgHandler = new SoundCloudMessageForm();
            this.MsgHandler.Show();
            this.MsgHandler.ShowMessage($"Updating {this.albumConfiguration.Title}");
            var album = this.soundCloud.GetAlbum(new MixDown() { Album = this.albumConfiguration.Title }, this.ShowSoundCloudError);
            if (album != null)
            {
                UpdateAlbumArt(album);
            }
            else
            {
                album = this.soundCloud.CreateAlbum(this.albumConfiguration, this.albumConfiguration.Comments, this.ShowSoundCloudError);
                if (album == null)
                {
                    this.MsgHandler.Close();
                    this.MsgHandler = null;
                    return;
                }
                UpdateAlbumArt(album);
            }

            this.MsgHandler.Close();
        
            void UpdateAlbumArt(SoundCloudPlaylist soundCloudPlaylist)
            {
                this.soundCloud?.OrderAlbumTracks(soundCloudPlaylist.Title, this.ShowSoundCloudError, (progress) => { });
                this.soundCloud.UpdateAlbumArtWork(soundCloudPlaylist, this.albumConfiguration, this.ShowSoundCloudError);
            }
        }

        public void DeleteAlbum()
        {
            this.MsgHandler = new SoundCloudMessageForm();
            this.MsgHandler.Show();

            var album = this.soundCloud.GetAlbum(new MixDown() { Album = this.albumConfiguration.Title }, this.ShowSoundCloudError);
            if (album != null)
            {
                this.soundCloud.DeleteAlbum(album, this.ShowSoundCloudError, true);
            }
            else
            {
                this.ShowSoundCloudError($"{this.albumConfiguration.Title} does not exist on sound cloud");
            }
            if (this.MsgHandler != null)
            {
                this.MsgHandler.Close();
            }
            this.RefreshAllTracks();
        }

        public void DeleteSelected(MixDown? singleTrack = null)
        {
            var selectedMixes = singleTrack != null ? MixDownCollection.CreateFromSingleMix(singleTrack)
                                        : this.mixDowns.GetSelectedMixes();
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
            this.RefreshAllTracks();
            this.synchroniseService.RaiseEvent(SyncEvent.DistributionMixUpload);
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
