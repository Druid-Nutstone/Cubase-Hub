using Cubase.Hub.Controls.MainFormControls.ProjectsForm;
using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Forms.Config;
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

namespace Cubase.Hub.Forms.Main
{
    public partial class MainForm : BaseWindows11Form
    {
        private IMessageService messageService;

        private readonly ProjectsControl projectsControl;

        private readonly IConfigurationService configurationService;

        private readonly ConfigurationForm configurationForm;

        public MainForm(IMessageService messageService,
                        ConfigurationForm configurationForm,
                        IConfigurationService configurationService,
                        ProjectsControl projectsControl)
        {
            InitializeComponent();
            ApplyWindows11Look();
            this.projectsControl = projectsControl; 
            this.configurationService = configurationService;
            this.configurationForm = configurationForm;
            messageService.RegisterForMessages(this.OnMessageReceived); 
            this.messageService = messageService;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (this.configurationService.LoadConfiguration(() => 
            { 
                this.configurationForm.Configuration = new CubaseHubConfiguration();
                var result = this.configurationForm.ShowDialog();   
                if (result == DialogResult.OK)
                {
                    this.LoadProjects();
                }
                else
                {
                    this.Close();   
                }
            }) != null)
            {
                // have config .. so load project control 
                this.LoadProjects();
            } 
            
   
        }

        private void LoadProjects()
        {
            this.LoadControl(this.projectsControl);
            this.projectsControl.LoadProjects();
        }

        private void LoadControl(Control control)
        {       
            this.ControlPanel.Controls.Clear();
            control.Dock = DockStyle.Fill;
            this.ControlPanel.Controls.Add(control);
        }

        private void OnMessageReceived(string message, bool waitCursor)
        {
            if (waitCursor)
            {
                this.Cursor = Cursors.WaitCursor;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }   
            this.StatusMessage.Text = message;
            this.StatusStrip.Refresh();
        }
    }
}
