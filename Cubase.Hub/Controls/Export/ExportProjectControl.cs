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

namespace Cubase.Hub.Controls.Export
{
    public partial class ExportProjectControl : UserControl
    {
        private CubaseProject Project;

        private readonly IDirectoryService directoryService;

        private readonly IConfigurationService configurationService;

        private readonly IMessageService messageService;

        public ExportProjectControl()
        {
            InitializeComponent();
        }

        public ExportProjectControl(IDirectoryService directoryService,
                                    IConfigurationService configurationService,
                                    IMessageService messageService) : this()
        {
            this.directoryService = directoryService;
            this.configurationService = configurationService;
            this.messageService = messageService;
            this.BrowseTargetDirectory.Click += BrowseTargetDirectory_Click;
            this.ExportButton.Click += ExportButton_Click;
        }

        private void ExportButton_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.TargetDirectory.Text))
            {
                this.messageService.ShowError("You must select a directory to export the project to");
                return;
            }
            var sourceFolder = Path.GetDirectoryName(this.Project.FullPath);
            var targetFolder = this.TargetDirectory.Text;

            // get source folder name
            var sourceFolderName = new DirectoryInfo(sourceFolder).Name;

            targetFolder = Path.Combine(targetFolder, sourceFolderName);

            var projectFiles = Directory.GetFiles(sourceFolder, "*.*", new EnumerationOptions() { RecurseSubdirectories = true });

            // todo - setup progress reporting for this

            this.ProjectProgress.Maximum = projectFiles.Length;
            this.ProjectProgress.Minimum = 0;
            this.Cursor = Cursors.WaitCursor;   
            foreach (var projectFile in projectFiles)
            {

                var relativePath = Path.GetRelativePath(sourceFolder, projectFile);
                var targetPath = Path.Combine(targetFolder, relativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
                File.Copy(projectFile, targetPath, true);
                this.ProjectProgress.Value++;
                int percent = (int)((double)this.ProjectProgress.Value
                                    / this.ProjectProgress.Maximum * 100);

                ProjectProgress.DisplayText = $"{percent}%";
                ProgressLabel.Text = $"Exporting {Path.GetFileName(targetPath)}";
                Application.DoEvents();
            }
            this.Cursor = Cursors.Default;  
            this.ProjectProgress.Value = 0;
            ProgressLabel.Text = $"Export Complete!";
        }

        private void BrowseTargetDirectory_Click(object? sender, EventArgs e)
        {
            var folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = "Select Project Export Directory";
            folderBrowser.ShowNewFolderButton = true;
            folderBrowser.UseDescriptionForTitle = true;
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                this.TargetDirectory.Text = folderBrowser.SelectedPath;
                this.configurationService.Configuration.LastExportFolderLocation = this.TargetDirectory.Text;
                this.configurationService.SaveConfiguration((err) => 
                { 
                   this.messageService.ShowError($"Failed to save configuration: {err}");
                });
            }
        }

        public ExportProjectControl SetProject(CubaseProject project)
        {
            this.Project = project;
            this.PopulateProjectDetails();
            return this;
        }

        private void PopulateProjectDetails()
        {
            this.LoadTargetDirectory();
        }

        private void LoadTargetDirectory()
        {
            this.TargetDirectory.Text = this.configurationService.Configuration.LastExportFolderLocation ?? string.Empty;
        }
    }
}
