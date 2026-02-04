using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Services.Config;
using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.Messages;
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
        private readonly IConfigurationService configurationService;

        private readonly IDirectoryService directoryService;

        private readonly IMessageService messageService;

        private string selectedSourceFolder;

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
        }

        private void Initialise() 
        { 
            this.AlbumDetails.Enabled = false;
            this.SourceFoldersListBox.Items.Clear();
            this.configurationService.Configuration.SourceCubaseFolders.ForEach(folder => 
            {
                this.SourceFoldersListBox.Items.Add(folder);
            });
            this.SourceFoldersListBox.SelectedIndexChanged += SourceFoldersListBox_SelectedIndexChanged;
            this.AlbumTitle.Leave += AlbumTitle_Leave;  
        }

        private void AlbumTitle_Leave(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.AlbumTitle.Text))
            {
                this.messageService.ShowError("Album title cannot be empty");
                return;
            }
            
            this.albumPath = GetAlbumName();
            if (!this.directoryService.MakeSureDirectoryExists(this.albumPath))   
            {
                this.messageService.ShowError($"Could NOT create album at {albumPath}");
                this.AlbumTitle.Focus();
                return;
            }
            this.messageService.ShowMessage($"Album created or verified at: {albumPath}", false);
        }

        private void SourceFoldersListBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
             this.selectedSourceFolder = SourceFoldersListBox.SelectedItem.ToString();   
             this.AlbumDetails.Enabled = true;  
        }

        private string GetAlbumName()
        {
            return Path.Combine(this.selectedSourceFolder, this.AlbumTitle.Text);
        }
    }
}
