using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Services;
using Cubase.Hub.Services.Audio;
using Cubase.Hub.Services.Config;
using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Models;
using Cubase.Hub.Services.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Hub.Forms.Albums
{
    public partial class ManageAlbumsForm : BaseWindows11Form
    {
        
        private readonly IConfigurationService configurationService;
               
        private readonly IProjectService projectService;

        private readonly IDirectoryService directoryService;

        private readonly IAudioService audioService;

        private readonly IMessageService messageService;

        public ManageAlbumsForm(IConfigurationService configurationService, 
                                IDirectoryService directoryService, 
                                IAudioService audioService,
                                IMessageService messageService,
                                IProjectService projectService)
        {
            InitializeComponent();
            this.configurationService = configurationService;
            this.directoryService = directoryService;
            this.audioService = audioService;   
            this.projectService = projectService;
            this.messageService = messageService;   
            ThemeApplier.ApplyDarkTheme(this);
            this.SelectedAlbum.SelectedIndexChanged += SelectedAlbum_SelectedIndexChanged;
        }

        private void SelectedAlbum_SelectedIndexChanged(object? sender, EventArgs e)
        {
            var selectedAlbum = this.SelectedAlbum.SelectedItem as AlbumLocation;
            // get album
            var albumConfig = AlbumConfiguration.LoadFromFile(Path.Combine(selectedAlbum.AlbumPath, CubaseHubConstants.CubaseAlbumConfigurationFileName));
            this.AlbumConfigurationControl.AlbumConfiguration = albumConfig;
            this.AlbumConfigurationControl.Initialise();
            // get album mixes 
            var msgDialog = this.messageService.OpenMessage("Loading Tracks..", this);
              var mixes = this.directoryService.GetMixes(selectedAlbum.AlbumPath);
              this.audioService.PopulateMixDownCollectionFromTags(mixes);
              this.mixdownControl.ShowMixes(mixes);
            msgDialog.Close();
        }


        public void Initialise()
        {
            this.InitialiseAlbumDropDown();
        }

        private void InitialiseAlbumDropDown()
        {
            this.SelectedAlbum.Items.Clear();
            this.SelectedAlbum.Items.AddRange(this.directoryService.GetCubaseAlbums(this.configurationService.Configuration.SourceCubaseFolders).ToArray());
            this.SelectedAlbum.DisplayMember = nameof(AlbumLocation.AlbumName);
        }


    }
}
