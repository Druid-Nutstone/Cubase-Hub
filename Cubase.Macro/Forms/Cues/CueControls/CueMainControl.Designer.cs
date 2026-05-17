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
            RefreshMixer = new Button();
            MainPanel = new Panel();
            label1 = new Label();
            CueNames = new ComboBox();
            TopPanel.SuspendLayout();
            SuspendLayout();
            // 
            // TopPanel
            // 
            TopPanel.Controls.Add(CueNames);
            TopPanel.Controls.Add(label1);
            TopPanel.Controls.Add(RefreshMixer);
            TopPanel.Dock = DockStyle.Top;
            TopPanel.Location = new Point(0, 0);
            TopPanel.Name = "TopPanel";
            TopPanel.Size = new Size(719, 49);
            TopPanel.TabIndex = 0;
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
            // MainPanel
            // 
            MainPanel.Dock = DockStyle.Fill;
            MainPanel.Location = new Point(0, 49);
            MainPanel.Name = "MainPanel";
            MainPanel.Size = new Size(719, 327);
            MainPanel.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(127, 14);
            label1.Name = "label1";
            label1.Size = new Size(39, 20);
            label1.TabIndex = 1;
            label1.Text = "Cue:";
            // 
            // CueNames
            // 
            CueNames.FormattingEnabled = true;
            CueNames.Location = new Point(172, 10);
            CueNames.Name = "CueNames";
            CueNames.Size = new Size(210, 28);
            CueNames.TabIndex = 2;
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
            TopPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel TopPanel;
        private Panel MainPanel;
        private Button RefreshMixer;
        private Label label1;
        private ComboBox CueNames;
    }
}
