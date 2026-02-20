using Cubase.Hub.Controls.Album.Manage;
using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Services;
using Cubase.Hub.Services.Config;
using Cubase.Hub.Services.Cubase;
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

namespace Cubase.Hub.Forms.Tracks
{
    public partial class NewTrackForm : BaseWindows11Form
    {
        private readonly IProjectService projectService;

        private readonly ICubaseService cubaseService;

        private readonly IDirectoryService directoryService;

        private readonly IConfigurationService configurationService;

        private readonly IMessageService messageService;

        public NewTrackForm(IProjectService projectService, 
                            IDirectoryService directoryService,
                            IMessageService messageService,
                            IConfigurationService configurationService,
                            ICubaseService cubaseService)
        {
            InitializeComponent();
            this.projectService = projectService;
            this.cubaseService = cubaseService; 
            this.configurationService = configurationService;
            this.directoryService = directoryService;
            this.messageService = messageService;
            this.CreateTrackButton.Click += CreateTrackButton_Click;
            ThemeApplier.ApplyDarkTheme(this);
        }

        private void CreateTrackButton_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.TrackName.Text))
            {
                this.messageService.ShowError("No Track name entered");
                return;
            }
            if (this.SelectedAlbum.SelectedIndex < 0)
            {
                this.messageService.ShowError("No Album selected");
                return;
            }
            if (this.SelectedTemplate.SelectedIndex < 0)
            {
                this.messageService.ShowError("No Template selected");
                return;
            }

            var album = this.SelectedAlbum.SelectedItem as AlbumLocation;
            var template = this.SelectedTemplate.SelectedItem as Template;
            var trackDirectory = Path.Combine(album.AlbumPath, this.TrackName.Text);
            
            if (!this.directoryService.MakeSureDirectoryExists(trackDirectory))
            {
                this.messageService.ShowError($"Could NOTcreate {trackDirectory}");
                return;
            }

            var targetTemplate = Path.Combine(trackDirectory, $"{this.TrackName.Text.Trim()}{CubaseHubConstants.CubaseFileExtension}");

            if (!this.directoryService.MakeSureDirectoryExists(Path.Combine(trackDirectory, CubaseHubConstants.MixdownDirectory)))
            { 
                this.messageService.ShowError($"Could NOT create {Path.Combine(trackDirectory, CubaseHubConstants.MixdownDirectory)}. you will have to do it manually"); 
            }

            Clipboard.SetText(trackDirectory);

            File.Copy(template.TemplateLocation, targetTemplate);

            var now = DateTime.Now;

            File.SetCreationTime(targetTemplate, now);
            File.SetLastWriteTime(targetTemplate, now);
            File.SetLastAccessTime(targetTemplate, now);

            this.cubaseService.OpenCubaseProject(targetTemplate);

            AlbumCommands.Instance.RefreshTracks();
            
            this.Close();

        }

        public void Initialise()
        {
            SelectedAlbum.Items.Clear();
            SelectedAlbum.Items.AddRange(
                  this.directoryService.GetCubaseAlbums(this.configurationService.Configuration.SourceCubaseFolders).ToArray()
            );
            SelectedAlbum.DisplayMember = nameof(AlbumLocation.AlbumName);
            SelectedTemplate.Items.Clear();
            SelectedTemplate.Items.AddRange(
                  this.directoryService.GetCubaseTemplates(this.GetTemplatePaths()).ToArray()
            );
            SelectedTemplate.DisplayMember = nameof(Template.TemplateName);
        }

        public List<string> GetTemplatePaths()
        {
            var templates = new List<string>();
            templates.Add(this.configurationService.Configuration.CubaseUserTemplateLocation);
            templates.Add(this.configurationService.Configuration.CubaseTemplateLocation);
            return templates;
        }
    }
}
