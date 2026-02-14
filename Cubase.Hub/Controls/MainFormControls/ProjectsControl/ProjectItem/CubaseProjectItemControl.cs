using Cubase.Hub.Controls.MainFormControls.ProjectsControl;
using Cubase.Hub.Controls.MainFormControls.ProjectsControl.Menu;
using Cubase.Hub.Forms.Albums;
using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Services.Cubase;
using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.Messages;
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
        private CubaseProjectControl? parentCubaseProjectControl = null;

        private CubaseProjectExtendedPropertiesControl extendedPropertiesControl;

        private readonly ICubaseService cubaseService;

        private readonly ManageAlbumsForm manageAlbumsForm;

        private readonly IDirectoryService directoryService;

        private readonly IMessageService messageService;

        private readonly IServiceProvider serviceProvider;

        private CubaseProject project;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<CubaseProjectItemControl>? ProjectExpanded { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<CubaseProjectItemControl>? ProjectMinimized { get; set; }

        public CubaseProjectItemControl(CubaseProjectExtendedPropertiesControl extendedPropertiesControl,
                                        ManageAlbumsForm manageAlbumsForm,
                                        IDirectoryService directoryService,
                                        IMessageService messageService,
                                        IServiceProvider serviceProvider,
                                        ICubaseService cubaseService)
        {
            this.extendedPropertiesControl = extendedPropertiesControl;
            this.manageAlbumsForm = manageAlbumsForm;
            this.cubaseService = cubaseService;
            this.serviceProvider = serviceProvider;
            this.directoryService = directoryService;  
            this.messageService = messageService;   
            this.Initialise();
            this.DoubleBuffered = true;
            this.PrimaryPanel.MouseEnter += PrimaryPanel_MouseEnter;
            this.PrimaryPanel.MouseLeave += PrimaryPanel_MouseLeave;

        }

        private void PrimaryPanel_MouseLeave(object? sender, EventArgs e)
        {
            if (((ProjectContextMenu)this.PrimaryPanel.ContextMenuStrip).IsOpen) return;

            ThemeApplier.ApplyDarkTheme(this.PrimaryPanel);
        }

        private void PrimaryPanel_MouseEnter(object? sender, EventArgs e)
        {
            ThemeApplier.ApplyDarkThemeSelected(this.PrimaryPanel);
            foreach (Control cntrl in this.PrimaryPanel.Controls)
            {
                cntrl.MouseMove += (s, e) =>
                {
                    ThemeApplier.ApplyDarkThemeSelected(this.PrimaryPanel);
                };
            }
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
            this.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.Minimise();
            this.ProjectLink.Click += ProjectLink_LinkClicked;

        }

        private void ProjectLink_LinkClicked(object? sender, EventArgs e)
        {
            this.cubaseService.OpenCubaseProject(this.ProjectLink.Project.FullPath ?? string.Empty);
        }

        private void Minimise()
        {
            this.SecondaryPanel.Visible = false;
            PerformLayout();
            //this.parentCubaseProjectControl?.PerformLayout();
        }

        private void Expand()
        {
            this.SecondaryPanel.Visible = true;
            this.SecondaryPanel.Controls.Clear();
            this.extendedPropertiesControl.SetProject(project);
            this.SecondaryPanel.Controls.Add(this.extendedPropertiesControl);
            this.SecondaryPanel.PerformLayout();
            PerformLayout();
            //this.parentCubaseProjectControl?.PerformLayout();
        }

        public void SetProject(CubaseProject project)
        {
            this.ProjectAlbum.Initialise(project);
            this.PrimaryPanel.ContextMenuStrip = new ProjectContextMenu(project, this.serviceProvider);
            this.ProjectAlbum.OnAlbumClicked += (albumPath) =>
            {
                var albumLocation = new AlbumLocation() { AlbumName = albumPath.Album, AlbumPath = albumPath.AlbumPath };
                this.manageAlbumsForm.Initialise(albumLocation);
                this.manageAlbumsForm.ShowDialog();
            };

            this.ProjectLink.Initialise(project);

            this.ProjectLastModified.Initialise(project);


            this.ProjectExpand.Initialise(project);
            this.ProjectExpand.OnStateChanged += (state) =>
            {
                if (state == CubaseProjectItemControlState.Expanded)
                {
                    this.ProjectExpanded?.Invoke(this);
                    this.Expand();
                }
                else
                {
                    this.ProjectMinimized?.Invoke(this);
                    this.Minimise();
                }
            };
            this.project = project;

        }

    }
}
