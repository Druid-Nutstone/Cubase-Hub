using Cubase.Hub.Forms.BaseForm;
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
        private SoundCloudDistributionProvider soundCloud;

        private IMessageService messageService;

        private SoundCloudPlaylistCollection soundCloudAlbums;

        private SoundCloudMainControl mainControl;

        private AlbumConfiguration albumConfiguration;

        private MixDownCollection mixDowns;

        public SoundCloudDistributer()
        {
            InitializeComponent();
        }

        public SoundCloudDistributer(
            IMessageService messageService,        
            SoundCloudDistributionProvider soundCloud) 
        {
            InitializeComponent();
            this.soundCloud = soundCloud;
            this.messageService = messageService;
            ThemeApplier.ApplyDarkTheme(this);
        }

        public void SetAlbum(AlbumConfiguration albumConfiguration, MixDownCollection mixDowns)
        {
            this.albumConfiguration = albumConfiguration; 
            this.mixDowns = mixDowns;
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

        public void Initialise()
        {
            if (this.soundCloud.Connect(this.ShowSoundCloudError))
            {
                this.soundCloudAlbums = this.soundCloud.GetPlayLists(this.ShowSoundCloudError);
                this.mainControl = new SoundCloudMainControl(this, this.soundCloud, this.soundCloudAlbums); 
            }
        }

        public void UploadSelected()
        {

        }

        public void OpenForm()
        {

        }

        public void CreateAlbum()
        {

        }


        private void ShowSoundCloudError(string error)
        {
           this.messageService.ShowError(error); 
        }
    }
}
