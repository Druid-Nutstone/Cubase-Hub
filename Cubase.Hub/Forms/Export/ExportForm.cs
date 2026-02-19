using Cubase.Hub.Controls.Export;
using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.Models;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IServiceProvider serviceProvider;

        private CubaseProject Project;

        private ExportType ExportType;

        public ExportForm()
        {
            this.Initialise();
        }

        public ExportForm(IServiceProvider serviceProvider)
        {
            this.Initialise();
            this.serviceProvider = serviceProvider;
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
            switch (ExportType)
            {
                case ExportType.Project:
                    this.Text = $"Export Project {this.Project.Name}";
                    this.LoadDataPanel(this.GetService<ExportProjectControl>().SetProject(this.Project));
                    break;
                case ExportType.Album:
                    break;
            }
        }

        private void LoadDataPanel(Control cntrl)
        {
            this.DataPanel.Controls.Clear();
            this.Size = new Size(cntrl.Size.Width + 40, cntrl.Size.Height + 80);
            cntrl.Dock = DockStyle.Fill;
            this.DataPanel.Controls.Add(cntrl);
            ThemeApplier.ApplyDarkTheme(cntrl);
   
        }

        private T GetService<T>()
        {
            return this.serviceProvider.GetService<T>();
        }
    }
}
