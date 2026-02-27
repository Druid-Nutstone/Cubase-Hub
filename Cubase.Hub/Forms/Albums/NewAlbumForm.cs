using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Services;
using Cubase.Hub.Services.Album;
using Cubase.Hub.Services.Config;
using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Hub.Forms
{
    public partial class NewAlbumForm : BaseWindows11Form
    {
        /*
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AlbumConfiguration AlbumConfiguration { get; set; } = new AlbumConfiguration();
        */

        private readonly IConfigurationService configurationService;

        private readonly IDirectoryService directoryService;

        private readonly IMessageService messageService;

        private readonly IAlbumService albumService;

        private string? albumPath;

        public NewAlbumForm(IConfigurationService configurationService, 
                            IMessageService messageService,
                            IAlbumService albumService,
                            IDirectoryService directoryService)
        {
            InitializeComponent();
            this.configurationService = configurationService;
            this.directoryService = directoryService;
            this.albumService = albumService;   
            this.messageService = messageService;
            ThemeApplier.ApplyDarkTheme(this);
            this.Initialise();
            this.albumService.NewAlbum();
            this.albumConfigurationControl.AlbumConfiguration = this.albumService.Configuration;
            this.albumConfigurationControl.Initialise();
            this.albumService.Configuration.PropertyChanged += AlbumConfiguration_PropertyChanged;
        }

        private void AlbumConfiguration_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            this.UpdatePathRoot();
        }

        private void Initialise() 
        { 
            this.SelectedExistingDirectory.Items.Clear();
            this.configurationService.Configuration.SourceCubaseFolders.ForEach(folder => 
            {
                this.SelectedExistingDirectory.Items.Add(folder);
            });
            this.SelectedExistingDirectory.SelectedIndexChanged += SelectedExistingDirectory_SelectedIndexChanged;
            this.BrowseRootDirectory.Click += BrowseRootDirectory_Click;
            this.CreateAlbumButton.Click += CreateAlbumButton_Click;
        }

        private void BrowseRootDirectory_Click(object? sender, EventArgs e)
        {
            var folderBrowse = new FolderBrowserDialog();
            folderBrowse.Description = "Select new album root directory";
            folderBrowse.ShowNewFolderButton = true;
            if (folderBrowse.ShowDialog() == DialogResult.OK) 
            {
                this.albumConfigurationControl.DisableTitle();
                this.albumPath = folderBrowse.SelectedPath;
                this.SelectedRootDirectory.Text = folderBrowse.SelectedPath;    
                this.UpdatePathRoot();
            }
        }

        private void SelectedExistingDirectory_SelectedIndexChanged(object? sender, EventArgs e)
        {
            this.albumPath = this.SelectedExistingDirectory.SelectedItem.ToString();
            this.UpdatePathRoot();
        }

        private void UpdatePathRoot() 
        {
            if (!string.IsNullOrWhiteSpace(this.albumPath))
            {
                this.NewAlbumRoot.Text = Path.Combine(this.albumPath, this.albumService.Configuration.Title ?? "");
            }
        }

        private void  CreateAlbumButton_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.albumPath))
            {
                return;
            }
            
            var targetDirectory = Path.Combine(this.albumPath, this.albumService.Configuration.Title ?? string.Empty).Trim();

            var verifyTitle = string.IsNullOrEmpty(this.SelectedRootDirectory.Text);


            var verifyAlbumProperties = this.albumService.VerifyAlbum((err) => 
            { 
                this.messageService.ShowError(err);     
            });

            if (!verifyAlbumProperties)
            {
                return;
            }

           
            if (!this.IsValidDirectoryPath(targetDirectory))
            {
                this.messageService.ShowError($"The album directory {targetDirectory} is not valid");
                return;
            }
            
            if (!this.directoryService.MakeSureDirectoryExists(targetDirectory))
            {
                this.messageService.ShowError($"Could NOT create album at {albumPath}");
                return;
            }
            
            if (!string.IsNullOrWhiteSpace(this.SelectedRootDirectory.Text))
            {
                // should return the directory name that has been selected
                this.albumService.Configuration.Title = Path.GetFileName(this.SelectedRootDirectory.Text);
            }

            
            if (this.albumService.SaveAlbum(targetDirectory, this.messageService.ShowError))
            {
                this.Close();
            }
            return;              

        }

        private bool IsValidDirectoryPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            try
            {
                // Normalizes and validates syntax
                Path.GetFullPath(path);
                return true;
            }
            catch (Exception ex) when (
                ex is ArgumentException ||
                ex is NotSupportedException ||
                ex is PathTooLongException)
            {
                return false;
            }
        }
    }
}
