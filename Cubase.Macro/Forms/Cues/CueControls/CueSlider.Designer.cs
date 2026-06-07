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
            ResetFaderButton = new ButtonWithHelp();
            SoloButton = new ToggleCubaseButton();
            MuteButton = new ToggleCubaseButton();
            SliderPanel = new Panel();
            BottomPanel.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // BottomPanel
            // 
            BottomPanel.Controls.Add(TrackName);
            BottomPanel.Dock = DockStyle.Bottom;
            BottomPanel.Location = new Point(0, 332);
            BottomPanel.Name = "BottomPanel";
            BottomPanel.Size = new Size(120, 43);
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
            panel1.Size = new Size(120, 42);
            panel1.TabIndex = 1;
            // 
            // VolumeText
            // 
            VolumeText.Dock = DockStyle.Fill;
            VolumeText.Location = new Point(0, 0);
            VolumeText.Name = "VolumeText";
            VolumeText.Size = new Size(120, 42);
            VolumeText.TabIndex = 0;
            VolumeText.Text = "label1";
            // 
            // panel2
            // 
            panel2.Controls.Add(ResetFaderButton);
            panel2.Controls.Add(SoloButton);
            panel2.Controls.Add(MuteButton);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(120, 33);
            panel2.TabIndex = 2;
            // 
            // ResetFaderButton
            // 
            ResetFaderButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ResetFaderButton.FlatAppearance.BorderSize = 0;
            ResetFaderButton.FlatStyle = FlatStyle.Flat;
            ResetFaderButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ResetFaderButton.Location = new Point(83, 1);
            ResetFaderButton.Name = "ResetFaderButton";
            ResetFaderButton.Size = new Size(30, 30);
            ResetFaderButton.TabIndex = 2;
            ResetFaderButton.Text = "F";
            ResetFaderButton.UseVisualStyleBackColor = true;
            // 
            // SoloButton
            // 
            SoloButton.FlatStyle = FlatStyle.Flat;
            SoloButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            SoloButton.Location = new Point(31, 1);
            SoloButton.Name = "SoloButton";
            SoloButton.OnOff = false;
            SoloButton.Size = new Size(30, 30);
            SoloButton.TabIndex = 1;
            SoloButton.Text = "S";
            SoloButton.UseVisualStyleBackColor = true;
            // 
            // MuteButton
            // 
            MuteButton.FlatStyle = FlatStyle.Flat;
            MuteButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            MuteButton.Location = new Point(1, 1);
            MuteButton.Name = "MuteButton";
            MuteButton.OnOff = false;
            MuteButton.Size = new Size(30, 30);
            MuteButton.TabIndex = 0;
            MuteButton.Text = "M";
            MuteButton.UseVisualStyleBackColor = true;
            // 
            // SliderPanel
            // 
            SliderPanel.Dock = DockStyle.Fill;
            SliderPanel.Location = new Point(0, 33);
            SliderPanel.Name = "SliderPanel";
            SliderPanel.Size = new Size(120, 257);
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
            Size = new Size(120, 375);
            BottomPanel.ResumeLayout(false);
            BottomPanel.PerformLayout();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel BottomPanel;
        private Label TrackName;
        private Panel panel1;
        private Label VolumeText;
        private Panel panel2;
        private Panel SliderPanel;
        private ToggleCubaseButton MuteButton;
        private ToggleCubaseButton SoloButton;
        private ButtonWithHelp ResetFaderButton;
    }
}
