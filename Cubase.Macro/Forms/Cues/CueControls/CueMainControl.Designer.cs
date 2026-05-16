namespace Cubase.Macro.Forms.Cues.CueControls
{
    partial class CueMainControl
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
            TopPanel = new Panel();
            MainPanel = new Panel();
            RefreshMixer = new Button();
            TopPanel.SuspendLayout();
            SuspendLayout();
            // 
            // TopPanel
            // 
            TopPanel.Controls.Add(RefreshMixer);
            TopPanel.Dock = DockStyle.Top;
            TopPanel.Location = new Point(0, 0);
            TopPanel.Name = "TopPanel";
            TopPanel.Size = new Size(719, 49);
            TopPanel.TabIndex = 0;
            // 
            // MainPanel
            // 
            MainPanel.Dock = DockStyle.Fill;
            MainPanel.Location = new Point(0, 49);
            MainPanel.Name = "MainPanel";
            MainPanel.Size = new Size(719, 327);
            MainPanel.TabIndex = 1;
            // 
            // RefreshMixer
            // 
            RefreshMixer.Location = new Point(4, 10);
            RefreshMixer.Name = "RefreshMixer";
            RefreshMixer.Size = new Size(94, 29);
            RefreshMixer.TabIndex = 0;
            RefreshMixer.Text = "Refresh";
            RefreshMixer.UseVisualStyleBackColor = true;
            // 
            // CueMainControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(MainPanel);
            Controls.Add(TopPanel);
            Name = "CueMainControl";
            Size = new Size(719, 376);
            TopPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel TopPanel;
        private Panel MainPanel;
        private Button RefreshMixer;
    }
}
