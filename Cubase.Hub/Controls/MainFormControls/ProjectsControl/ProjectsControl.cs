using Cubase.Hub.Controls.MainFormControls.ProjectsControl;
using Cubase.Hub.Forms.BaseForm;
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

        private CubaseProjectCollection projects;

        private CubaseProjectControl projectPanel;

        public ProjectsControl()
        {
            InitializeComponent();
        }

        public ProjectsControl(IMessageService messageService, 
                               IServiceProvider serviceProvider,
                               IProjectService projectService)
        {
            this.serviceProvider = serviceProvider;
            this.messageService = messageService;   
            this.projectService = projectService;
            InitializeComponent();
            this.ProjectSearch.OnSearchTextChanged += ProjectFilterTextChanged;
            this.DataPanel.AutoScroll = true;
            this.SeperatorPanel.Height = 2;
            this.SeperatorPanel.BorderStyle = BorderStyle.FixedSingle;
            this.HideIndex();
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
                projectPanel = this.GetInstanceOf<CubaseProjectControl>();
                this.PopulateDataPanel(projectPanel);
                this.PopulateProjects(projects);
            }
            this.ResumeLayout();
            this.messageService.ShowMessage("Projects loaded.", false);
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
