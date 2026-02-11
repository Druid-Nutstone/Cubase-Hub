using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using Cubase.Hub.Forms.BaseForm;

namespace Cubase.Hub.Controls.MainFormControls.ProjectsControl.ProjectItem
{

    public class ProjectAlbum : Label
    {

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CubaseProject Project { get; private set; }

        public bool HasAlbum => !string.IsNullOrEmpty(this.Project?.Album);

        public ProjectAlbum() : base()
        {
            this.Font = new System.Drawing.Font(this.Font, FontStyle.Bold);
        }

        public void Initialise(CubaseProject project)
        {
            this.Project = project;
            if (!string.IsNullOrEmpty(project.Album))
            {
                this.Text = project.Album;
                this.ForeColor = DarkTheme.ProjectAlbum;
                this.Cursor = Cursors.Hand;
            }
            else
            {
                this.Text = project.FolderPath;
            }
        }
    }
}
