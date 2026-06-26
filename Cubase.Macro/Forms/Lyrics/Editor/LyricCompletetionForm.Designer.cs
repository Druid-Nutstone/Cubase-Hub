namespace Cubase.Macro.Forms.Lyrics.Editor
{
    partial class LyricCompletetionForm
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
            MainPanel = new Panel();
            SuspendLayout();
            // 
            // MainPanel
            // 
            MainPanel.Dock = DockStyle.Fill;
            MainPanel.Location = new Point(0, 0);
            MainPanel.Name = "MainPanel";
            MainPanel.Size = new Size(204, 339);
            MainPanel.TabIndex = 0;
            // 
            // LyricCompletetionForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(204, 339);
            Controls.Add(MainPanel);
            FormBorderStyle = FormBorderStyle.None;
            Name = "LyricCompletetionForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "LyricCompletetionForm";
            ResumeLayout(false);
        }

        #endregion

        private Panel MainPanel;
    }
}