namespace Cubase.Hub.Forms.Mixes
{
    partial class ManageMixesForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageMixesForm));
            TopPanel = new Panel();
            ActionGroup = new GroupBox();
            ActionControl = new Panel();
            ButtonPanel = new Panel();
            FileName = new Label();
            FileProgress = new ProgressBar();
            CopyButton = new Button();
            ActionPanel = new Panel();
            CopyToDirectoryButton = new RadioButton();
            ConvertToFlacButton = new RadioButton();
            ConvertToMp3Button = new RadioButton();
            ControlPanel = new Panel();
            BrowseTargetDirectoryButton = new Button();
            TargetDirectory = new TextBox();
            label1 = new Label();
            TopPanel.SuspendLayout();
            ActionGroup.SuspendLayout();
            ButtonPanel.SuspendLayout();
            ActionPanel.SuspendLayout();
            ControlPanel.SuspendLayout();
            SuspendLayout();
            // 
            // TopPanel
            // 
            TopPanel.Controls.Add(ActionGroup);
            TopPanel.Controls.Add(ControlPanel);
            resources.ApplyResources(TopPanel, "TopPanel");
            TopPanel.Name = "TopPanel";
            // 
            // ActionGroup
            // 
            ActionGroup.Controls.Add(ActionControl);
            ActionGroup.Controls.Add(ButtonPanel);
            ActionGroup.Controls.Add(ActionPanel);
            resources.ApplyResources(ActionGroup, "ActionGroup");
            ActionGroup.Name = "ActionGroup";
            ActionGroup.TabStop = false;
            // 
            // ActionControl
            // 
            resources.ApplyResources(ActionControl, "ActionControl");
            ActionControl.Name = "ActionControl";
            // 
            // ButtonPanel
            // 
            ButtonPanel.Controls.Add(FileName);
            ButtonPanel.Controls.Add(FileProgress);
            ButtonPanel.Controls.Add(CopyButton);
            resources.ApplyResources(ButtonPanel, "ButtonPanel");
            ButtonPanel.Name = "ButtonPanel";
            // 
            // FileName
            // 
            resources.ApplyResources(FileName, "FileName");
            FileName.Name = "FileName";
            // 
            // FileProgress
            // 
            resources.ApplyResources(FileProgress, "FileProgress");
            FileProgress.Name = "FileProgress";
            // 
            // CopyButton
            // 
            resources.ApplyResources(CopyButton, "CopyButton");
            CopyButton.Name = "CopyButton";
            CopyButton.UseVisualStyleBackColor = true;
            // 
            // ActionPanel
            // 
            ActionPanel.Controls.Add(CopyToDirectoryButton);
            ActionPanel.Controls.Add(ConvertToFlacButton);
            ActionPanel.Controls.Add(ConvertToMp3Button);
            resources.ApplyResources(ActionPanel, "ActionPanel");
            ActionPanel.Name = "ActionPanel";
            // 
            // CopyToDirectoryButton
            // 
            resources.ApplyResources(CopyToDirectoryButton, "CopyToDirectoryButton");
            CopyToDirectoryButton.Name = "CopyToDirectoryButton";
            CopyToDirectoryButton.TabStop = true;
            CopyToDirectoryButton.UseVisualStyleBackColor = true;
            // 
            // ConvertToFlacButton
            // 
            resources.ApplyResources(ConvertToFlacButton, "ConvertToFlacButton");
            ConvertToFlacButton.Name = "ConvertToFlacButton";
            ConvertToFlacButton.TabStop = true;
            ConvertToFlacButton.UseVisualStyleBackColor = true;
            // 
            // ConvertToMp3Button
            // 
            resources.ApplyResources(ConvertToMp3Button, "ConvertToMp3Button");
            ConvertToMp3Button.Name = "ConvertToMp3Button";
            ConvertToMp3Button.TabStop = true;
            ConvertToMp3Button.UseVisualStyleBackColor = true;
            // 
            // ControlPanel
            // 
            ControlPanel.Controls.Add(BrowseTargetDirectoryButton);
            ControlPanel.Controls.Add(TargetDirectory);
            ControlPanel.Controls.Add(label1);
            resources.ApplyResources(ControlPanel, "ControlPanel");
            ControlPanel.Name = "ControlPanel";
            // 
            // BrowseTargetDirectoryButton
            // 
            resources.ApplyResources(BrowseTargetDirectoryButton, "BrowseTargetDirectoryButton");
            BrowseTargetDirectoryButton.Name = "BrowseTargetDirectoryButton";
            BrowseTargetDirectoryButton.UseVisualStyleBackColor = true;
            // 
            // TargetDirectory
            // 
            resources.ApplyResources(TargetDirectory, "TargetDirectory");
            TargetDirectory.Name = "TargetDirectory";
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // ManageMixesForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(TopPanel);
            Name = "ManageMixesForm";
            TopPanel.ResumeLayout(false);
            ActionGroup.ResumeLayout(false);
            ButtonPanel.ResumeLayout(false);
            ButtonPanel.PerformLayout();
            ActionPanel.ResumeLayout(false);
            ActionPanel.PerformLayout();
            ControlPanel.ResumeLayout(false);
            ControlPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel TopPanel;
        private Panel ControlPanel;
        private GroupBox ActionGroup;
        private Button BrowseTargetDirectoryButton;
        private TextBox TargetDirectory;
        private Label label1;
        private RadioButton ConvertToMp3Button;
        private RadioButton CopyToDirectoryButton;
        private Panel ActionPanel;
        private RadioButton ConvertToFlacButton;
        private Panel ActionControl;
        private Panel ButtonPanel;
        private Button CopyButton;
        private ProgressBar FileProgress;
        private Label FileName;
    }
}