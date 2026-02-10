using Cubase.Hub.Controls.MainFormControls.ProjectsControl;
using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Services.Cubase;
using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Hub.Controls.MainFormControls.ProjectsForm
{


    public enum CubaseProjectItemControlState
    {
        Minimized,
        Expanded
    }

    public partial class CubaseProjectItemControl : UserControl
    {
        private CubaseProjectItemControlState currentState = CubaseProjectItemControlState.Minimized;

        private CubaseProjectControl? parentCubaseProjectControl = null;

        private CubaseProjectExtendedPropertiesControl extendedPropertiesControl;

        private readonly ICubaseService cubaseService;

        private CubaseProject project;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<CubaseProjectItemControl>? ProjectSelected { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<CubaseProjectItemControl>? ProjectDeselected { get; set; }

        public CubaseProjectItemControl(CubaseProjectExtendedPropertiesControl extendedPropertiesControl,
                                        ICubaseService cubaseService)
        {
            this.extendedPropertiesControl = extendedPropertiesControl;
            this.cubaseService = cubaseService;
            this.Initialise();
            this.DoubleBuffered = true;
        }

        public void SetParent(CubaseProjectControl cubaseProjectControl)
        {
            this.parentCubaseProjectControl = cubaseProjectControl;
        }

        public void Initialise()
        {
            InitializeComponent();

            this.SecondaryPanel.AutoSize = true;

            AutoSize = true;                      // 🔑 REQUIRED
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            Dock = DockStyle.Fill;
            // this.BorderStyle = BorderStyle.FixedSingle;
            this.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.Minimise();
            this.ExpandContractButton.Click += ExpandContractButton_Click;
            this.ProjectLink.Click += ProjectLink_LinkClicked;
            this.ProjectLink.MouseDown += (s, e) =>
            {
               this.Cursor = Cursors.WaitCursor;
            };
            this.ProjectLink.MouseUp += (s, e) =>
            {
                this.Cursor = Cursors.Default;
            };  
        }

        private void ProjectLink_LinkClicked(object? sender, EventArgs e)
        {
            this.cubaseService.OpenCubaseProject(this.ProjectLink.Tag?.ToString() ?? string.Empty);
            this.ProjectSelected?.Invoke(this); 
        }

        private void ExpandContractButton_Click(object? sender, EventArgs e)
        {
            if (this.currentState == CubaseProjectItemControlState.Minimized)
            {
                this.ProjectSelected?.Invoke(this);
                this.Expand();
                this.currentState = CubaseProjectItemControlState.Expanded;
            }
            else
            {
                this.ProjectDeselected?.Invoke(this);
                this.Minimise();
                this.currentState = CubaseProjectItemControlState.Minimized;
            }
        }

        private void Minimise()
        {
            currentState = CubaseProjectItemControlState.Minimized;
            ExpandContractButton.Image = Properties.Resources.arrow_down;
            this.SecondaryPanel.Visible = false;
            PerformLayout();
            // Invalidate();
            this.parentCubaseProjectControl?.PerformLayout();
        }

        private void Expand()
        {
            currentState = CubaseProjectItemControlState.Expanded;
            ExpandContractButton.Image = Properties.Resources.arrow_up;
            this.SecondaryPanel.Visible = true;
            this.SecondaryPanel.Controls.Clear();
            this.extendedPropertiesControl.SetProject(project);
            this.SecondaryPanel.Controls.Add(this.extendedPropertiesControl);
            // Invalidate();
            this.SecondaryPanel.PerformLayout();
            PerformLayout();
            this.parentCubaseProjectControl?.PerformLayout();
        }

        public void SetProject(CubaseProject project)
        {
            if (string.IsNullOrEmpty(project.Album))
            {
                this.FolderLabel.Text = project.FolderPath;
            }
            else
            {
                this.FolderLabel.ForeColor = Color.Green;
                this.FolderLabel.Text = project.Album;
            }
            this.ProjectLink.Text = Path.GetFileNameWithoutExtension(project.Name);
            this.ProjectLink.Tag = project.FullPath;
            this.project = project;

        }
    }
}
