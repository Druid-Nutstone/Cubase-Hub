using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Controls.MainFormControls.ProjectsControl.ProjectItem
{
    public class ProjectLastModified : Label
    {
        public ProjectLastModified() : base()
        {
            this.Font = new System.Drawing.Font(this.Font, FontStyle.Italic);
        }
        
        public void Initialise(CubaseProject project)
        {
            this.Text = $"{project.LastModified.ToString("g")}";
        }

    }
}
