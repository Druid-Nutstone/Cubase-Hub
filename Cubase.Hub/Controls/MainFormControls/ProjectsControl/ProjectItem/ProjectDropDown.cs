using Cubase.Hub.Controls.MainFormControls.ProjectsForm;
using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Cubase.Hub.Controls.MainFormControls.ProjectsControl.ProjectItem
{
    public class ProjectDropDown : PictureBox
    {
        public CubaseProject Project { get; private set; }

        public CubaseProjectItemControlState ControlState { get; private set; }

        private bool haveMixes = false;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<CubaseProjectItemControlState>? OnStateChanged { get; set; }

        public ProjectDropDown() : base()    
        {
           this.SizeMode = PictureBoxSizeMode.StretchImage;
           this.Size = new System.Drawing.Size(24, 24);    
           this.Image = Properties.Resources.arrow_down;
           this.Cursor = Cursors.Hand;
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            if (!this.Enabled)
            {
                this.Image = null;
            }
            else
            {
                this.UpdateImage();
            }
        }
        
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (!this.haveMixes) return;
        
            if (this.ControlState == CubaseProjectItemControlState.Minimized)
            {
                this.Image = Properties.Resources.arrow_up; 
                this.ControlState = CubaseProjectItemControlState.Expanded;
                this.OnStateChanged?.Invoke(this.ControlState); 
            }
            else
            {
                this.Image = Properties.Resources.arrow_down; 
                this.ControlState = CubaseProjectItemControlState.Minimized;
                this.OnStateChanged?.Invoke(this.ControlState);
            }
        
        }

        public void Initialise(CubaseProject project)
        {
            this.Project = project;
            this.ControlState = CubaseProjectItemControlState.Minimized;
            this.UpdateImage();
        }

        private void UpdateImage()
        {
            if (this.Project.Mixes != null && Project.Mixes.Count > 0)
            {
                if (this.ControlState == CubaseProjectItemControlState.Minimized)
                {
                    this.Image = Properties.Resources.arrow_down;
                }
                else
                {
                    this.Image = Properties.Resources.arrow_up;
                }
                this.haveMixes = true;
            }
            else
            {
                this.Image = null;
            }

        }

    }
}
