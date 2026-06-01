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
            panel2 = new Panel();
            SoloButton = new CubaseMixerToggleControl();
            RecordButton = new CubaseMixerToggleControl();
            MuteButton = new CubaseMixerToggleControl();
            SliderPanel = new Panel();
            BottomPanel.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SoloButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RecordButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MuteButton).BeginInit();
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
            // panel2
            // 
            panel2.Controls.Add(SoloButton);
            panel2.Controls.Add(RecordButton);
            panel2.Controls.Add(MuteButton);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(111, 39);
            panel2.TabIndex = 2;
            // 
            // SoloButton
            // 
            SoloButton.Location = new Point(78, 3);
            SoloButton.Name = "SoloButton";
            SoloButton.Size = new Size(30, 30);
            SoloButton.SizeMode = PictureBoxSizeMode.StretchImage;
            SoloButton.TabIndex = 2;
            SoloButton.TabStop = false;
            // 
            // RecordButton
            // 
            RecordButton.Location = new Point(35, 3);
            RecordButton.Name = "RecordButton";
            RecordButton.Size = new Size(30, 30);
            RecordButton.SizeMode = PictureBoxSizeMode.StretchImage;
            RecordButton.TabIndex = 1;
            RecordButton.TabStop = false;
            // 
            // MuteButton
            // 
            MuteButton.Location = new Point(3, 3);
            MuteButton.Name = "MuteButton";
            MuteButton.Size = new Size(30, 30);
            MuteButton.SizeMode = PictureBoxSizeMode.StretchImage;
            MuteButton.TabIndex = 0;
            MuteButton.TabStop = false;
            // 
            // SliderPanel
            // 
            SliderPanel.Dock = DockStyle.Fill;
            SliderPanel.Location = new Point(0, 39);
            SliderPanel.Name = "SliderPanel";
            SliderPanel.Size = new Size(111, 251);
            SliderPanel.TabIndex = 3;
            // 
            // CueSlider
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(SliderPanel);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(BottomPanel);
            Name = "CueSlider";
            Size = new Size(111, 375);
            BottomPanel.ResumeLayout(false);
            BottomPanel.PerformLayout();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SoloButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)RecordButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)MuteButton).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel BottomPanel;
        private Label TrackName;
        private Panel panel1;
        private Label VolumeText;
        private Panel panel2;
        private Panel SliderPanel;
        private CubaseMixerToggleControl MuteButton;
        private CubaseMixerToggleControl RecordButton;
        private CubaseMixerToggleControl SoloButton;
    }
}
