using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Services.Config;
using Cubase.Hub.Services.FileAndDirectory;
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

        private string selectedSourceFolder;

        public NewAlbumForm(IConfigurationService configurationService, 
                            IDirectoryService directoryService)
        {
            InitializeComponent();
            this.configurationService = configurationService;
            this.directoryService = directoryService;   
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
        }

        private void SourceFoldersListBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
             this.selectedSourceFolder = SourceFoldersListBox.SelectedItem.ToString();   
             this.AlbumDetails.Enabled = true;  
        }
    }
}
