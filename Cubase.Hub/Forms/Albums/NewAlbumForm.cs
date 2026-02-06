using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Services;
using Cubase.Hub.Services.Config;
using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Hub.Forms
{
    public partial class NewAlbumForm : BaseWindows11Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AlbumConfiguration AlbumConfiguration { get; set; } = new AlbumConfiguration();

        private readonly IConfigurationService configurationService;

        private readonly IDirectoryService directoryService;

        private readonly IMessageService messageService;

        private string? albumPath;

        public NewAlbumForm(IConfigurationService configurationService, 
                            IMessageService messageService,
                            IDirectoryService directoryService)
        {
            InitializeComponent();
            this.configurationService = configurationService;
            this.directoryService = directoryService;   
            this.messageService = messageService;
            ThemeApplier.ApplyDarkTheme(this);
            this.Initialise();
            this.albumConfigurationControl.AlbumConfiguration = this.AlbumConfiguration;
            this.albumConfigurationControl.Initialise();
            this.AlbumConfiguration.PropertyChanged += AlbumConfiguration_PropertyChanged;
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
                this.NewAlbumRoot.Text = Path.Combine(this.albumPath, this.AlbumConfiguration.Title ?? "");
            }
        }

        private void  CreateAlbumButton_Click(object? sender, EventArgs e)
        {
            var targetDirectory = Path.Combine(this.albumPath, this.AlbumConfiguration.Title ?? string.Empty).Trim();

            var verifyTitle = string.IsNullOrEmpty(this.SelectedRootDirectory.Text);
                

            if (!this.AlbumConfiguration.Verify(out string propertyInError, verifyTitle))
            {
                this.messageService.ShowError($"Album configuration is invalid. Please check the {propertyInError} field.");
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
                this.AlbumConfiguration.Title = Path.GetFileName(this.SelectedRootDirectory.Text);
            }

            this.AlbumConfiguration.SaveToDirectory(targetDirectory);
            
            this.messageService.ShowMessage($"Album created or verified at: {albumPath}", false);

            this.Close();
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
