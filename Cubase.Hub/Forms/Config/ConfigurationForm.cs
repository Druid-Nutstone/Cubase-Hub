using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Services.Config;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Hub.Forms.Config
{
    public partial class ConfigurationForm : BaseWindows11Form
    {
        private readonly IConfigurationService configurationService;

        private readonly IMessageService messageService;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CubaseHubConfiguration Configuration { get; set; }

        public ConfigurationForm(IConfigurationService configurationService, IMessageService messageService)
        {
            this.configurationService = configurationService;
            this.messageService = messageService;
            InitializeComponent();
            this.ButtonSave.Click += ButtonSave_Click;
            this.AddSourceFolderButton.Click += AddSourceFolderButton_Click;
            this.BrowseCubaseExeButton.Click += BrowseCubaseExeButton_Click;
            this.BrowseUserTemplateLocationButton.Click += BrowseUserTemplateLocationButton_Click;
            this.BrowseCubaseTemplateButton.Click += BrowseCubaseTemplateButton_Click;
            ThemeApplier.ApplyDarkTheme(this);
        }

        private void BrowseCubaseTemplateButton_Click(object? sender, EventArgs e)
        {
            var dirDialog = new FolderBrowserDialog();
            var result = dirDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.CubaseTemplateLocation.Text = dirDialog.SelectedPath;
            }
        }

        private void BrowseUserTemplateLocationButton_Click(object? sender, EventArgs e)
        {
            var dirDialog = new FolderBrowserDialog();
            var result = dirDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.CubaseUserTemplateLocation.Text = dirDialog.SelectedPath;  
            }
        }

        private void BrowseCubaseExeButton_Click(object? sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Cubase Executable|*.exe";
            fileDialog.Title = "Select Cubase Executable";  
            fileDialog.InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Steinberg");    
            var result = fileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.CubaseExeLocation.Text = fileDialog.FileName;
            }
        }

        private void AddSourceFolderButton_Click(object? sender, EventArgs e)
        {
            var dirDialog = new FolderBrowserDialog();
            var result = dirDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.SourceCubaseFolders.Text += (this.SourceCubaseFolders.Text.Length > 0 ? ";" : "") + dirDialog.SelectedPath;
            }
        }

        private void ButtonSave_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Configuration.SourceCubaseFolders = new List<string>(SourceCubaseFolders.Text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));   
            this.Configuration.CubaseExeLocation = this.CubaseExeLocation.Text;
            this.Configuration.CubaseUserTemplateLocation = this.CubaseUserTemplateLocation.Text;
            this.Configuration.CubaseTemplateLocation = this.CubaseTemplateLocation.Text;
            this.configurationService.SaveConfiguration(this.Configuration, () => 
            { 
                this.messageService.ShowError("An error occurred while saving the configuration. Please try again.");
            });
            this.Close();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.MapConfiguration();
        }

        private void MapConfiguration()
        {
            if (this.Configuration == null)
            {
                this.Configuration = this.configurationService.Configuration;
                if (this.Configuration == null)
                {
                    throw new Exception("No config model has been passed to the ConfigurationForm");
                }
            }
            SourceCubaseFolders.Text = string.Join(";", this.Configuration.SourceCubaseFolders);
            CubaseExeLocation.Text = this.Configuration.CubaseExeLocation;
            CubaseUserTemplateLocation.Text = this.Configuration.CubaseUserTemplateLocation;
            CubaseTemplateLocation.Text = this.Configuration.CubaseTemplateLocation;
        }
    }
}
