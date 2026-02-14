using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Cubase.Hub.Controls.MainFormControls.ProjectsControl.ProjectItem
{
    public class ProjectLink : Label
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CubaseProject Project { get; private set; }

        public ProjectLink() : base()
        {
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Font = new System.Drawing.Font(this.Font.FontFamily, this.Font.Size+2, FontStyle.Bold);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.Cursor = Cursors.WaitCursor;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this.Cursor = Cursors.Hand;
        }

        public void Initialise(CubaseProject project)
        {
            this.Project = project;
            this.Text = Path.GetFileNameWithoutExtension(project.Name);
        }
    }
}
