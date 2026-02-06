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
            FolderLabel = new Label();
            PrimaryPanel = new Panel();
            ProjectLink = new Cubase.Hub.Controls.MainFormControls.ProjectsControl.LabelLink();
            ExpandContractButton = new PictureBox();
            SecondaryPanel = new Panel();
            PrimaryPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ExpandContractButton).BeginInit();
            SuspendLayout();
            // 
            // FolderLabel
            // 
            FolderLabel.AutoSize = true;
            FolderLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            FolderLabel.Location = new Point(446, 20);
            FolderLabel.Name = "FolderLabel";
            FolderLabel.Size = new Size(90, 20);
            FolderLabel.TabIndex = 0;
            FolderLabel.Text = "FolderLabel";
            // 
            // PrimaryPanel
            // 
            PrimaryPanel.Controls.Add(ProjectLink);
            PrimaryPanel.Controls.Add(ExpandContractButton);
            PrimaryPanel.Controls.Add(FolderLabel);
            PrimaryPanel.Dock = DockStyle.Top;
            PrimaryPanel.Location = new Point(0, 0);
            PrimaryPanel.Name = "PrimaryPanel";
            PrimaryPanel.Size = new Size(704, 58);
            PrimaryPanel.TabIndex = 2;
            // 
            // ProjectLink
            // 
            ProjectLink.AutoSize = true;
            ProjectLink.Font = new Font("Segoe UI", 11F, FontStyle.Bold | FontStyle.Underline);
            ProjectLink.Location = new Point(36, 15);
            ProjectLink.Name = "ProjectLink";
            ProjectLink.Size = new Size(103, 25);
            ProjectLink.TabIndex = 3;
            ProjectLink.Text = "labelLink1";
            // 
            // ExpandContractButton
            // 
            ExpandContractButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ExpandContractButton.Image = Properties.Resources.arrow_down;
            ExpandContractButton.Location = new Point(667, 13);
            ExpandContractButton.Name = "ExpandContractButton";
            ExpandContractButton.Size = new Size(24, 24);
            ExpandContractButton.SizeMode = PictureBoxSizeMode.AutoSize;
            ExpandContractButton.TabIndex = 2;
            ExpandContractButton.TabStop = false;
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
            ((System.ComponentModel.ISupportInitialize)ExpandContractButton).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label FolderLabel;
        private Panel PrimaryPanel;
        private Panel SecondaryPanel;
        private PictureBox ExpandContractButton;
        private MainFormControls.ProjectsControl.LabelLink ProjectLink;
    }
}
