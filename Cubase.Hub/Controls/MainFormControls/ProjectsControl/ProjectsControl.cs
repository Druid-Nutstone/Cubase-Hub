using Cubase.Hub.Controls.Album.Manage;
using Cubase.Hub.Controls.MainFormControls.ProjectsControl;
using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Models;
using Cubase.Hub.Services.Projects;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Hub.Controls.MainFormControls.ProjectsForm
{
    public partial class ProjectsControl : UserControl
    {
        private readonly IMessageService messageService;

        private readonly IProjectService projectService;

        private readonly IServiceProvider serviceProvider;

        private readonly IDirectoryService directoryService;

        private CubaseProjectCollection projects;

        private CubaseProjectControl projectPanel;

        private IEnumerable<AlbumLocation> albumLocations;

        public ProjectsControl()
        {
            InitializeComponent();
        }

        public ProjectsControl(IMessageService messageService,
                               IDirectoryService directoryService,
                               IServiceProvider serviceProvider,
                               IProjectService projectService)
        {
            this.serviceProvider = serviceProvider;
            this.messageService = messageService;
            this.projectService = projectService;
            this.directoryService = directoryService;
            InitializeComponent();
            this.ProjectSearch.OnSearchTextChanged += ProjectFilterTextChanged;
            this.AlbumList.SelectedIndexChanged += AlbumList_SelectedIndexChanged;
            this.ClearAlbumButton.Click += ClearAlbumButton_Click;
            this.DataPanel.AutoScroll = true;
            this.SeperatorPanel.Height = 2;
            this.SeperatorPanel.BorderStyle = BorderStyle.FixedSingle;
            this.RefreshProjectsButton.Click += RefreshProjectsButton_Click;
            this.HideIndex();
            AlbumCommands.Instance.RegisterForAlbumCommand(OnAlbumCommand);


            ThemeApplier.ApplyDarkTheme(this);
        }

        private void RefreshProjectsButton_Click(object? sender, EventArgs e)
        {
            this.LoadProjects();
        }

        private void OnAlbumCommand(AlbumCommandType commandType)
        {
            if (commandType == AlbumCommandType.RefreshTracks)
            {
                this.LoadProjects();
            }
        }

        private void ClearAlbumButton_Click(object? sender, EventArgs e)
        {
            projectPanel.ClearProjects();
            this.PopulateProjects(this.projects);
            this.AlbumList.SelectedIndex = -1;
        }

        private void AlbumList_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (this.AlbumList.SelectedIndex > -1)
            {
                projectPanel.ClearProjects();
                var selectedAlbum = this.AlbumList.SelectedItem as AlbumLocation;
                this.PopulateProjects(this.projects.GetAlbumProjects(selectedAlbum.AlbumName));
            }
        }

        private void ProjectFilterTextChanged(string text)
        {
            projectPanel.ClearProjects();
            if (text.Length == 0)
            {
                this.PopulateProjects(this.projects);
            }
            else
            {
                if (text.Length >= 3)
                {
                    this.PopulateProjects(this.projects.FilteredCollection(text));
                }
            }
        }

        public void LoadProjects()
        {
            this.SuspendLayout();
            this.messageService.ShowMessage("Loading projects...", true);
            this.projects = this.projectService.LoadProjects((err) =>
            {
                this.messageService.ShowError($"Error loading projects: {err}");
            });
            if (this.projects != null)
            {
                this.albumLocations = this.projects.GetAlbums();
                projectPanel = this.GetInstanceOf<CubaseProjectControl>();
                this.PopulateDataPanel(projectPanel);
                this.PopulateProjects(projects);
                this.PopulateAlbums();
            }
            this.ResumeLayout();
            this.messageService.ShowMessage("Projects loaded.", false);
        }

        private void PopulateAlbums()
        {
            this.AlbumList.Items.Clear();
            this.AlbumList.DisplayMember = nameof(AlbumLocation.AlbumName);
            this.AlbumList.Items.AddRange(this.albumLocations.ToArray());
        }

        private void PopulateProjects(CubaseProjectCollection cubaseProjects)
        {
            foreach (var project in cubaseProjects)
            {
                var projectItem = this.GetInstanceOf<CubaseProjectItemControl>();
                projectItem.SetProject(project);
                projectPanel.AddProjectItem(projectItem);
            }
        }

        private void HideIndex()
        {
            this.IndexPanel.Visible = false;
        }

        private void ShowIndex()
        {
            this.IndexPanel.Visible = true;
        }

        private T GetInstanceOf<T>()
        {
            return this.serviceProvider.GetService<T>();
        }

        private void PopulateDataPanel(Control userControl)
        {
            this.DataPanel.Controls.Clear();
            this.DataPanel.Controls.Add(userControl);
        }
    }
}
