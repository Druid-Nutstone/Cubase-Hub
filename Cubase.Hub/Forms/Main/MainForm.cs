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
            this.AutoScaleMode = AutoScaleMode.Dpi;
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
                if (this.configurationService.Configuration.MainWindowLocation != null)
                {
                    StartPosition = FormStartPosition.Manual;
                    Bounds = new Rectangle(
                        this.configurationService.Configuration.MainWindowLocation.X,
                        this.configurationService.Configuration.MainWindowLocation.Y,
                        this.configurationService.Configuration.MainWindowLocation.Width,
                        this.configurationService.Configuration.MainWindowLocation.Height);
                }
                this.LoadProjects();
            } 
            
   
        }

        private void LoadProjects()
        {
            this.SuspendLayout();
            var globMessage = this.messageService.OpenMessage("Loading Projects ..", this);
            this.LoadControl(this.projectsControl);
            this.projectsControl.LoadProjects();
            this.ResumeLayout();
            globMessage.Close();
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            //this.Bounds = new Rectangle() {  } 
            // this.configurationService.Configuration.
            var bounds = WindowState == FormWindowState.Normal
            ? Bounds
            : RestoreBounds;

            var settings = new WindowSettings
            {
                X = bounds.X,
                Y = bounds.Y,
                Width = bounds.Width,
                Height = bounds.Height,
            };
            this.configurationService?.Configuration?.MainWindowLocation = settings;
            this.configurationService?.SaveConfiguration(this.configurationService?.Configuration, () => { });
        }


    }
}
