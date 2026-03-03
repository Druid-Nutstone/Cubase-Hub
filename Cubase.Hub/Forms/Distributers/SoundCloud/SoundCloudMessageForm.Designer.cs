namespace Cubase.Hub.Forms.Distributers.SoundCloud
{
    partial class SoundCloudMessageForm
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
            MessageLabel = new Label();
            SuspendLayout();
            // 
            // MessageLabel
            // 
            MessageLabel.AutoSize = true;
            MessageLabel.Location = new Point(55, 56);
            MessageLabel.Name = "MessageLabel";
            MessageLabel.Size = new Size(91, 20);
            MessageLabel.TabIndex = 0;
            MessageLabel.Text = "Initialising .. ";
            // 
            // SoundCloudMessageForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(694, 149);
            ControlBox = false;
            Controls.Add(MessageLabel);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "SoundCloudMessageForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sound Cloud Messages";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label MessageLabel;
    }
}