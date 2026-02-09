using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Forms.Mixes.MixActions;
using Cubase.Hub.Services.Audio;
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

namespace Cubase.Hub.Forms.Mixes
{
    public partial class ManageMixesForm : BaseWindows11Form
    {
        private readonly IAudioService audioService;
        private readonly IDirectoryService directoryService;
        private readonly IMessageService messageService;
        private MixDownCollection Mixes;

        private MixAction MixAction = MixAction.None;

        public ManageMixesForm()
        {
            InitializeComponent();
        }

        public ManageMixesForm(IAudioService audioService, 
                               IMessageService messageService,
                               IDirectoryService directoryService)
        {
            this.audioService = audioService;
            this.directoryService = directoryService;
            this.messageService = messageService;               InitializeComponent();
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
                FileProgress.Value = 0;
                FileProgress.PerformLayout();
                FileProgress.Refresh();
                ((IMixActionControl)this.ActionControl.Controls[0]).RunAction(
                    this.Mixes,
                    this.TargetDirectory.Text,
                    this.audioService,
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
            this.FileProgress.Value =
                (int)Math.Round((double)fileIndex * 100 / this.Mixes.Count);

            this.FileName.Text = fileName;
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
            // mixdowns just containes selecte mixes
            this.InitialiseControls();
            this.Mixes = mixDowns;  
        }

        public void InitialiseControls()
        {
            this.TargetDirectory.Text = string.Empty;
            this.CopyToDirectoryButton.Checked = false; 
            this.ConvertToMp3Button.Checked = false;    
            this.ConvertToFlacButton.Checked = false;
            this.ActionGroup.Enabled = false;
            this.ActionGroup.Visible = false;
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
