namespace Cubase.Hub.Controls.Export
{
    partial class ExportProjectControl
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
            label1 = new Label();
            TargetDirectory = new TextBox();
            ProjectProgress = new Cubase.Hub.Controls.MainFormControls.ProjectsControl.PlayControls.DarkProgressBar();
            ExportButton = new Button();
            ProgressLabel = new Label();
            BrowseTargetDirectory = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(33, 22);
            label1.Name = "label1";
            label1.Size = new Size(128, 20);
            label1.TabIndex = 0;
            label1.Text = "Target Directory ";
            // 
            // TargetDirectory
            // 
            TargetDirectory.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            TargetDirectory.Location = new Point(39, 51);
            TargetDirectory.Name = "TargetDirectory";
            TargetDirectory.Size = new Size(479, 27);
            TargetDirectory.TabIndex = 1;
            // 
            // ProjectProgress
            // 
            ProjectProgress.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ProjectProgress.Location = new Point(40, 101);
            ProjectProgress.Name = "ProjectProgress";
            ProjectProgress.Size = new Size(478, 29);
            ProjectProgress.TabIndex = 3;
            ProjectProgress.Text = "darkProgressBar1";
            // 
            // ExportButton
            // 
            ExportButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ExportButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ExportButton.Location = new Point(524, 101);
            ExportButton.Name = "ExportButton";
            ExportButton.Size = new Size(94, 29);
            ExportButton.TabIndex = 4;
            ExportButton.Text = "Export";
            ExportButton.UseVisualStyleBackColor = true;
            // 
            // ProgressLabel
            // 
            ProgressLabel.AutoSize = true;
            ProgressLabel.Location = new Point(39, 136);
            ProgressLabel.Name = "ProgressLabel";
            ProgressLabel.Size = new Size(101, 20);
            ProgressLabel.TabIndex = 5;
            ProgressLabel.Text = "ProgressLabel";
            // 
            // BrowseTargetDirectory
            // 
            BrowseTargetDirectory.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BrowseTargetDirectory.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            BrowseTargetDirectory.Location = new Point(524, 51);
            BrowseTargetDirectory.Name = "BrowseTargetDirectory";
            BrowseTargetDirectory.Size = new Size(94, 29);
            BrowseTargetDirectory.TabIndex = 6;
            BrowseTargetDirectory.Text = "Browse";
            BrowseTargetDirectory.UseVisualStyleBackColor = true;
            // 
            // ExportProjectControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(BrowseTargetDirectory);
            Controls.Add(ProgressLabel);
            Controls.Add(ExportButton);
            Controls.Add(ProjectProgress);
            Controls.Add(TargetDirectory);
            Controls.Add(label1);
            DoubleBuffered = true;
            Name = "ExportProjectControl";
            Size = new Size(636, 185);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox TargetDirectory;
        private MainFormControls.ProjectsControl.PlayControls.DarkProgressBar ProjectProgress;
        private Button ExportButton;
        private Label ProgressLabel;
        private Button BrowseTargetDirectory;
    }
}
