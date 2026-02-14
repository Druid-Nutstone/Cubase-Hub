using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Hub.Forms.Export
{
    
    public enum ExportType
    {
        Project,
        Album
    }

    public partial class ExportForm : BaseWindows11Form
    {
        private readonly IDirectoryService directoryService;

        private CubaseProject Project;

        private ExportType ExportType;

        public ExportForm()
        {
            this.Initialise();
        }

        public ExportForm(IDirectoryService directoryService)
        {
            this.Initialise();
            this.directoryService = directoryService;   
        }

        public void SetProject(CubaseProject project)
        {
            this.Project = project;
        }

        public void SetExportType(ExportType exportType)
        {
            this.ExportType = exportType;
        }

        private void Initialise()
        {
            InitializeComponent();
            ThemeApplier.ApplyDarkTheme(this);
            this.AutoScaleMode = AutoScaleMode.Dpi;
        }

        protected override void OnShown(EventArgs e)
        {
            // todo - decide what to show based on export 
        }
    }
}
