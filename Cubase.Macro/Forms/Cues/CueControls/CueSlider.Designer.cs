namespace Cubase.Macro.Forms.Cues.CueControls
{
    partial class CueSlider
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
            BottomPanel = new Panel();
            TrackName = new Label();
            panel1 = new Panel();
            VolumeText = new Label();
            SliderPanel = new Panel();
            BottomPanel.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // BottomPanel
            // 
            BottomPanel.Controls.Add(TrackName);
            BottomPanel.Dock = DockStyle.Bottom;
            BottomPanel.Location = new Point(0, 332);
            BottomPanel.Name = "BottomPanel";
            BottomPanel.Size = new Size(111, 43);
            BottomPanel.TabIndex = 0;
            // 
            // TrackName
            // 
            TrackName.AutoSize = true;
            TrackName.Dock = DockStyle.Fill;
            TrackName.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            TrackName.Location = new Point(0, 0);
            TrackName.Name = "TrackName";
            TrackName.Size = new Size(65, 25);
            TrackName.TabIndex = 0;
            TrackName.Text = "label1";
            // 
            // panel1
            // 
            panel1.Controls.Add(VolumeText);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 290);
            panel1.Name = "panel1";
            panel1.Size = new Size(111, 42);
            panel1.TabIndex = 1;
            // 
            // VolumeText
            // 
            VolumeText.Dock = DockStyle.Fill;
            VolumeText.Location = new Point(0, 0);
            VolumeText.Name = "VolumeText";
            VolumeText.Size = new Size(111, 42);
            VolumeText.TabIndex = 0;
            VolumeText.Text = "label1";
            // 
            // SliderPanel
            // 
            SliderPanel.Dock = DockStyle.Fill;
            SliderPanel.Location = new Point(0, 0);
            SliderPanel.Name = "SliderPanel";
            SliderPanel.Size = new Size(111, 290);
            SliderPanel.TabIndex = 2;
            // 
            // CueSlider
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(SliderPanel);
            Controls.Add(panel1);
            Controls.Add(BottomPanel);
            Name = "CueSlider";
            Size = new Size(111, 375);
            BottomPanel.ResumeLayout(false);
            BottomPanel.PerformLayout();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel BottomPanel;
        private Label TrackName;
        private Panel panel1;
        private Label VolumeText;
        private Panel SliderPanel;
    }
}
