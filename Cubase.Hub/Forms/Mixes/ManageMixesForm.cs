using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Forms.Mixes.MixActions;
using Cubase.Hub.Services.Audio;
using Cubase.Hub.Services.Config;
using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Models;
using Cubase.Hub.Services.Track;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Hub.Forms.Mixes
{
    public partial class ManageMixesForm : BaseWindows11Form
    {
        private readonly ITrackService trackService;
        private readonly IDirectoryService directoryService;
        private readonly IMessageService messageService;
        private readonly IConfigurationService configurationService;
        private MixDownCollection Mixes;

        private MixAction MixAction = MixAction.None;

        public ManageMixesForm()
        {
            InitializeComponent();
        }

        public ManageMixesForm(ITrackService trackService,
                               IMessageService messageService,
                               IConfigurationService configurationService,
                               IDirectoryService directoryService)
        {
            this.trackService = trackService;
            this.directoryService = directoryService;
            this.messageService = messageService;
            this.configurationService = configurationService;
            InitializeComponent();
            ThemeApplier.ApplyDarkTheme(this);
            this.InitialiseControls();
            this.BrowseTargetDirectoryButton.Click += BrowseTargetDirectoryButton_Click;
            this.CopyToDirectoryButton.CheckedChanged += CopyToDirectoryButton_CheckedChanged;
            this.ConvertToMp3Button.CheckedChanged += ConvertToMp3Button_CheckedChanged;
            this.ConvertToFlacButton.CheckedChanged += ConvertToFlacButton_CheckedChanged;
            this.CopyButton.Click += CopyButton_Click;
        }

        private void CopyButton_Click(object? sender, EventArgs e)
        {
            if (this.MixAction != MixAction.None)
            {
                ((IMixActionControl)this.ActionControl.Controls[0]).RunAction(
                    this.Mixes,
                    this.TargetDirectory.Text,
                    this.trackService,
                    this.messageService,
                    this.directoryService,
                    (fileIndex, fileName) =>
                    {
                        UpdateProgress(fileIndex, fileName);
                        return true;
                    });
            }
        }

        void UpdateProgress(int fileIndex, string fileName)
        {
            if (fileIndex < 0)
            {
                this.FileName.Text = "Complete";
                this.FileName.ForeColor = Color.Green; 
                this.CurrentFile.Text = "";
            }
            else
            {
                this.FileName.Text = $"File {fileIndex}";
                this.CurrentFile.Text = $"Converting {fileName}";
                this.FileName.ForeColor = this.CurrentFile.ForeColor;
            }
            Application.DoEvents();
        }

        private void ConvertToFlacButton_CheckedChanged(object? sender, EventArgs e)
        {
            this.MixAction = MixAction.Flac;
            this.LoadActionControl(new MixActionFlacControl());
        }

        private void ConvertToMp3Button_CheckedChanged(object? sender, EventArgs e)
        {
            this.MixAction = MixAction.Mp3;
            this.LoadActionControl(new MixActionMp3Control());
        }

        private void CopyToDirectoryButton_CheckedChanged(object? sender, EventArgs e)
        {
            this.MixAction = MixAction.Copy;
            this.LoadActionControl(new MixActionCopyControl());
        }

        private void LoadActionControl(IMixActionControl mixActionControl)
        {
            this.ActionControl.Controls.Clear();
            var cntrl = mixActionControl as UserControl;
            cntrl.Dock = DockStyle.Fill;
            this.ActionControl.Controls.Add(cntrl);
        }

        private void BrowseTargetDirectoryButton_Click(object? sender, EventArgs e)
        {
            var targetFolder = new FolderBrowserDialog();
            targetFolder.Description = "Select the target directory for the mixes";
            targetFolder.UseDescriptionForTitle = true;
            targetFolder.ShowNewFolderButton = true;
            targetFolder.Multiselect = false;
            if (targetFolder.ShowDialog() == DialogResult.OK)
            {
                this.TargetDirectory.Text = targetFolder.SelectedPath;
                this.ActionGroup.Enabled = true;
                this.ActionGroup.Visible = true;
            }
        }

        public void Initialise(MixDownCollection mixDowns)
        {
            this.Mixes = mixDowns;
            if (mixDowns.Count > 0)
            {
                this.TargetDirectory.Text = this.Mixes.First().ExportLocation ?? string.Empty;
            }
            this.InitialiseControls();
        }

        public void InitialiseControls()
        {
            this.CopyToDirectoryButton.Checked = false;
            this.ConvertToMp3Button.Checked = false;
            this.ConvertToFlacButton.Checked = false;
            this.ActionGroup.Enabled = !string.IsNullOrEmpty(this.TargetDirectory.Text);
            this.ActionGroup.Visible = !string.IsNullOrEmpty(this.TargetDirectory.Text);
        }
    }

    public enum MixAction
    {
        None = 0,
        Copy = 1,
        Mp3 = 2,
        Flac = 3,
    }
}
