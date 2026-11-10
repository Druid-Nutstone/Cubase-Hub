namespace Cubase.Macro.Forms.AudioCapture
{
    partial class AudioCaptureForm
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
            label1 = new Label();
            FileName = new TextBox();
            label2 = new Label();
            FolderName = new TextBox();
            FolderBrowseButton = new Button();
            RecordingButton = new Button();
            Progress = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(34, 19);
            label1.Name = "label1";
            label1.Size = new Size(79, 20);
            label1.TabIndex = 0;
            label1.Text = "File Name";
            // 
            // FileName
            // 
            FileName.Location = new Point(39, 44);
            FileName.Name = "FileName";
            FileName.Size = new Size(257, 27);
            FileName.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(34, 94);
            label2.Name = "label2";
            label2.Size = new Size(53, 20);
            label2.TabIndex = 2;
            label2.Text = "Folder";
            // 
            // FolderName
            // 
            FolderName.Location = new Point(39, 119);
            FolderName.Name = "FolderName";
            FolderName.Size = new Size(440, 27);
            FolderName.TabIndex = 3;
            // 
            // FolderBrowseButton
            // 
            FolderBrowseButton.Location = new Point(485, 118);
            FolderBrowseButton.Name = "FolderBrowseButton";
            FolderBrowseButton.Size = new Size(94, 29);
            FolderBrowseButton.TabIndex = 4;
            FolderBrowseButton.Text = "Browse";
            FolderBrowseButton.UseVisualStyleBackColor = true;
            // 
            // RecordingButton
            // 
            RecordingButton.Location = new Point(39, 179);
            RecordingButton.Name = "RecordingButton";
            RecordingButton.Size = new Size(148, 29);
            RecordingButton.TabIndex = 5;
            RecordingButton.Text = "Start Recording";
            RecordingButton.UseVisualStyleBackColor = true;
            // 
            // Progress
            // 
            Progress.AutoSize = true;
            Progress.Location = new Point(42, 249);
            Progress.Name = "Progress";
            Progress.Size = new Size(70, 20);
            Progress.TabIndex = 6;
            Progress.Text = "Waiting ..";
            // 
            // AudioCaptureForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(609, 298);
            Controls.Add(Progress);
            Controls.Add(RecordingButton);
            Controls.Add(FolderBrowseButton);
            Controls.Add(FolderName);
            Controls.Add(label2);
            Controls.Add(FileName);
            Controls.Add(label1);
            Name = "AudioCaptureForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox FileName;
        private Label label2;
        private TextBox FolderName;
        private Button FolderBrowseButton;
        private Button RecordingButton;
        private Label Progress;
    }
}