using Cubase.Hub.Controls.MainFormControls.ProjectsForm;
using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Forms.Config;
using Cubase.Hub.Forms.Main.Menu;
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

        private MenuContent menuContent;

        public MainForm(IMessageService messageService,
                        ConfigurationForm configurationForm,
                        IConfigurationService configurationService,
                        MenuContent menuContent,
                        ProjectsControl projectsControl)
        {
            InitializeComponent();
            this.projectsControl = projectsControl; 
            this.menuContent = menuContent;
            this.configurationService = configurationService;
            this.configurationForm = configurationForm;
            messageService.RegisterForMessages(this.OnMessageReceived); 
            this.messageService = messageService;
            this.MainMenu.Renderer = new DarkToolStripRenderer();
            menuContent.Initialise(this.MainMenu, this);
            ThemeApplier.ApplyDarkTheme(this);
        }



        protected override void OnShown(EventArgs e)
        {
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
            this.SuspendLayout();
            this.LoadControl(this.projectsControl);
            this.projectsControl.LoadProjects();
            this.ResumeLayout();
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
