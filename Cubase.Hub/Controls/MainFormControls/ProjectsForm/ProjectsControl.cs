using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Services.Messages;
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
            this.toolStrip.BackColor = DarkTheme.BackColor;
            this.HideIndex();
        }

        public void LoadProjects()
        {
            this.messageService.ShowMessage("Loading projects...", true);
            var projects = this.projectService.LoadProjects((err) => 
            { 
                this.messageService.ShowError($"Error loading projects: {err}");   
            });  
            if (projects != null)
            {
                var projectPanel = this.GetInstanceOf<CubaseProjectControl>();
                this.PopulateDataPanel(projectPanel);
                projectPanel.SuspendLayout();
                foreach (var project in projects)
                {
                    var projectItem =  this.GetInstanceOf<CubaseProjectItemControl>();
                    projectItem.SetProject(project);
                    projectPanel.AddProjectItem(projectItem);
                }
                projectPanel.ResumeLayout();
            }
            this.messageService.ShowMessage("Projects loaded.", false);
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
