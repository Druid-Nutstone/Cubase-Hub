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
            ApplyWindows11Look();
            this.ButtonSave.Click += ButtonSave_Click;
            this.AddSourceFolderButton.Click += AddSourceFolderButton_Click;    
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
                throw new Exception("No config model has been passed to the ConfigurationForm");   
            }
            SourceCubaseFolders.Text = string.Join(";", this.Configuration.SourceCubaseFolders);
        }
    }
}
