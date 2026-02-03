using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Projects;
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

        public ProjectsControl()
        {
            InitializeComponent();
        }

        public ProjectsControl(IMessageService messageService, 
                               IProjectService projectService)
        {
            this.messageService = messageService;   
            this.projectService = projectService;
            InitializeComponent();
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
                // todo do something with the project list !!
            }
            this.messageService.ShowMessage("Projects loaded.", false);
        }
    }
}
