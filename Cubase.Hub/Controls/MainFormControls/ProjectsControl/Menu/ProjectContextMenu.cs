using Cubase.Hub.Controls.Menus;
using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Forms.Export;
using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Hub.Controls.MainFormControls.ProjectsControl.Menu
{
    public class ProjectContextMenu : DarkContextMenu
    {
        public ProjectContextMenu() : base()
        {

        }

        public ProjectContextMenu(CubaseProject cubaseProject,
                                  IServiceProvider serviceProvider) : base()
        {
            this.Initialise(cubaseProject, serviceProvider);
        }



        public void Initialise(CubaseProject cubaseProject, IServiceProvider serviceProvider)
        {
            this.Items.Clear();
            this.Items.Add(new OpenProjectFolder(cubaseProject, serviceProvider)); 
            this.Items.Add(new DeleteProjectCPR(cubaseProject, serviceProvider));
            this.Items.Add(new ExportProject(cubaseProject, serviceProvider));
        }
    }


    public class BaseProjectMenuItem : ToolStripMenuItem
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CubaseProject CubaseProject { get; set; }
        
        protected IServiceProvider ServiceProvider { get; private set; }

        public BaseProjectMenuItem(string text, CubaseProject cubaseProject, IServiceProvider serviceProvider) : base(text)
        {
            this.Text = text;
            this.AutoSize = true;        // KEY
            this.Height = 50;             // Or 36 / 40 for modern look
            this.CubaseProject = cubaseProject;
            this.ServiceProvider = serviceProvider;
        }

        public override Size GetPreferredSize(Size constrainingSize)
        {
            var size = base.GetPreferredSize(constrainingSize);
            size.Height = 50; // your desired row height
            return size;
        }

        protected IDirectoryService DirectoryService => this.ServiceProvider.GetService<IDirectoryService>();
    
        protected IMessageService MessageService => this.ServiceProvider.GetService<IMessageService>(); 
    }

    public class  OpenProjectFolder : BaseProjectMenuItem
    {
        public OpenProjectFolder(CubaseProject cubaseProject, IServiceProvider serviceProvider) : base("Open Track Folder", cubaseProject, serviceProvider)
        {

        }

        protected override void OnClick(EventArgs e)
        {
            this.DirectoryService.OpenExplorer(Path.GetDirectoryName(this.CubaseProject.FullPath));
        }
    }

    public class ExportProject : BaseProjectMenuItem
    {
        public ExportProject(CubaseProject cubaseProject, IServiceProvider serviceProvider) : base("Export Project", cubaseProject, serviceProvider)
        {
        }

        protected override void OnClick(EventArgs e)
        {
            var exportForm = this.ServiceProvider.GetService<ExportForm>(); 
            exportForm?.SetProject(this.CubaseProject);
            exportForm?.SetExportType(ExportType.Project);
            exportForm?.ShowDialog();
        }
    }

    public class DeleteProjectCPR : BaseProjectMenuItem
    {
        public DeleteProjectCPR(CubaseProject cubaseProject, IServiceProvider serviceProvider) : base("Delete Cubase Project File (.cpr)", cubaseProject, serviceProvider)
        {

        }

        protected override void OnClick(EventArgs e)
        {
            if (this.MessageService.AskMessage($"Are you sure you want to delete {Path.GetFileName(this.CubaseProject.FullPath)}") == DialogResult.Yes)
            {
                try
                {
                    File.Delete(this.CubaseProject.FullPath);
                    this.MessageService.ShowMessage("Project file deleted successfully.", false);

                }
                catch (Exception ex)
                {
                    this.MessageService.ShowError($"Failed to delete project file: {ex.Message}");
                }
            }
        }
    }

}
