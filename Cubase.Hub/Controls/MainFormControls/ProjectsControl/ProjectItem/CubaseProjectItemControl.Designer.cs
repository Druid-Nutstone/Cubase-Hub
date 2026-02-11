namespace Cubase.Hub.Controls.MainFormControls.ProjectsForm
{
    partial class CubaseProjectItemControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CubaseProjectItemControl));
            PrimaryPanel = new Panel();
            ProjectExpand = new Cubase.Hub.Controls.MainFormControls.ProjectsControl.ProjectItem.ProjectDropDown();
            ProjectAlbum = new Cubase.Hub.Controls.MainFormControls.ProjectsControl.ProjectItem.ProjectAlbum();
            ProjectLink = new Cubase.Hub.Controls.MainFormControls.ProjectsControl.ProjectItem.ProjectLink();
            SecondaryPanel = new Panel();
            PrimaryPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ProjectExpand).BeginInit();
            SuspendLayout();
            // 
            // PrimaryPanel
            // 
            PrimaryPanel.Controls.Add(ProjectExpand);
            PrimaryPanel.Controls.Add(ProjectAlbum);
            PrimaryPanel.Controls.Add(ProjectLink);
            PrimaryPanel.Dock = DockStyle.Top;
            PrimaryPanel.Location = new Point(0, 0);
            PrimaryPanel.Name = "PrimaryPanel";
            PrimaryPanel.Size = new Size(704, 58);
            PrimaryPanel.TabIndex = 2;
            // 
            // ProjectExpand
            // 
            ProjectExpand.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ProjectExpand.Image = (Image)resources.GetObject("ProjectExpand.Image");
            ProjectExpand.Location = new Point(648, 17);
            ProjectExpand.Name = "ProjectExpand";
            ProjectExpand.Size = new Size(24, 24);
            ProjectExpand.SizeMode = PictureBoxSizeMode.StretchImage;
            ProjectExpand.TabIndex = 5;
            ProjectExpand.TabStop = false;
            // 
            // ProjectAlbum
            // 
            ProjectAlbum.AutoSize = true;
            ProjectAlbum.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ProjectAlbum.Location = new Point(442, 20);
            ProjectAlbum.Name = "ProjectAlbum";
            ProjectAlbum.Size = new Size(105, 20);
            ProjectAlbum.TabIndex = 4;
            ProjectAlbum.Text = "ProjectAlbum";
            // 
            // ProjectLink
            // 
            ProjectLink.AutoSize = true;
            ProjectLink.Font = new Font("Segoe UI", 9F, FontStyle.Bold | FontStyle.Underline);
            ProjectLink.Location = new Point(32, 20);
            ProjectLink.Name = "ProjectLink";
            ProjectLink.Size = new Size(87, 20);
            ProjectLink.TabIndex = 3;
            ProjectLink.Text = "ProjectLink";
            // 
            // SecondaryPanel
            // 
            SecondaryPanel.Dock = DockStyle.Top;
            SecondaryPanel.Location = new Point(0, 58);
            SecondaryPanel.Name = "SecondaryPanel";
            SecondaryPanel.Size = new Size(704, 281);
            SecondaryPanel.TabIndex = 3;
            SecondaryPanel.Visible = false;
            // 
            // CubaseProjectItemControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.Transparent;
            Controls.Add(SecondaryPanel);
            Controls.Add(PrimaryPanel);
            Name = "CubaseProjectItemControl";
            Size = new Size(704, 339);
            PrimaryPanel.ResumeLayout(false);
            PrimaryPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ProjectExpand).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Panel PrimaryPanel;
        private Panel SecondaryPanel;
        private MainFormControls.ProjectsControl.ProjectItem.ProjectLink ProjectLink;
        private MainFormControls.ProjectsControl.ProjectItem.ProjectAlbum ProjectAlbum;
        private MainFormControls.ProjectsControl.ProjectItem.ProjectDropDown ProjectExpand;
    }
}
