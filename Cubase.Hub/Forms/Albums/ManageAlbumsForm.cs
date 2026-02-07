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
using System.Diagnostics;
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

        private AlbumLocation CurrentAlbum;

        private MixDownCollection CurrentMixes;

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
            this.OpenAlbumDirectory.Click += OpenAlbumDirectory_Click;
        }

        private void OpenAlbumDirectory_Click(object? sender, EventArgs e)
        {
            if (this.CurrentAlbum != null)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "explorer.exe",
                    Arguments = this.CurrentAlbum.AlbumPath,
                    UseShellExecute = true
                });
            }
        }

        private void SelectedAlbum_SelectedIndexChanged(object? sender, EventArgs e)
        {
            this.CurrentAlbum = this.SelectedAlbum.SelectedItem as AlbumLocation;
            // get album
            var albumConfig = AlbumConfiguration.LoadFromFile(Path.Combine(this.CurrentAlbum.AlbumPath, CubaseHubConstants.CubaseAlbumConfigurationFileName));
            this.AlbumConfigurationControl.AlbumConfiguration = albumConfig;
            this.AlbumConfigurationControl.Initialise(this.OnAlbumChanged);
            // get album mixes 
            var msgDialog = this.messageService.OpenMessage("Loading Tracks..", this);
            this.LoadTracks(this.CurrentAlbum.AlbumPath);
            msgDialog.Close();
        }

        private void LoadTracks(string albumPath)
        {
            var mixes = this.directoryService.GetMixes(albumPath);
            this.CurrentMixes = this.audioService.PopulateMixDownCollectionFromTags(mixes);
            this.mixdownControl.ShowMixes(this.CurrentMixes, this.OnMixChanged, this.audioService);
        }

        private void OnMixChanged(MixDown mixDown, string propertyName)
        {
            this.audioService.SetTagsFromMixDowm(mixDown);  
            if (propertyName == nameof(MixDown.TrackNumber))
            {
                this.LoadTracks(this.CurrentAlbum.AlbumPath);
            }
        }

        private void OnAlbumChanged(AlbumConfiguration albumConfiguration, string propertyName)
        {
            albumConfiguration.SaveToDirectory(this.CurrentAlbum.AlbumPath);
            foreach (var mix in this.CurrentMixes)
            {
                mix.Album = albumConfiguration.Title;
                mix.Year = albumConfiguration.Year;
                mix.Artist = albumConfiguration.Artist;
                mix.Genre = albumConfiguration.Genre;
                this.audioService.SetTagsFromMixDowm(mix);
            }
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
