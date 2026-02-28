namespace Cubase.Hub.Controls.CompletedMixes.Tracks
{
    partial class TrackPlayViewControl
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
            MixTitle = new Cubase.Hub.Controls.BoundControls.BoundLabel();
            MixTrackNo = new Cubase.Hub.Controls.BoundControls.BoundLabel();
            MixDuration = new Cubase.Hub.Controls.BoundControls.BoundLabel();
            MixAudioType = new Cubase.Hub.Controls.BoundControls.BoundLabel();
            MixBitrate = new Cubase.Hub.Controls.BoundControls.BoundLabel();
            MixSampleRate = new Cubase.Hub.Controls.BoundControls.BoundLabel();
            MixSize = new Cubase.Hub.Controls.BoundControls.BoundLabel();
            MixPerformers = new Cubase.Hub.Controls.BoundControls.BoundLabel();
            MixCover = new Cubase.Hub.Controls.Media.TrackCover.TrackCoverControl();
            LoadArtButton = new Button();
            ((System.ComponentModel.ISupportInitialize)MixCover).BeginInit();
            SuspendLayout();
            // 
            // MixTitle
            // 
            MixTitle.AutoSize = true;
            MixTitle.Location = new Point(114, 38);
            MixTitle.Name = "MixTitle";
            MixTitle.Size = new Size(67, 20);
            MixTitle.TabIndex = 0;
            MixTitle.Text = "MixTtitle";
            // 
            // MixTrackNo
            // 
            MixTrackNo.AutoSize = true;
            MixTrackNo.Location = new Point(285, 38);
            MixTrackNo.Name = "MixTrackNo";
            MixTrackNo.Size = new Size(63, 20);
            MixTrackNo.TabIndex = 1;
            MixTrackNo.Text = "TrackNo";
            // 
            // MixDuration
            // 
            MixDuration.AutoSize = true;
            MixDuration.Location = new Point(345, 38);
            MixDuration.Name = "MixDuration";
            MixDuration.Size = new Size(45, 20);
            MixDuration.TabIndex = 2;
            MixDuration.Text = "MLen";
            // 
            // MixAudioType
            // 
            MixAudioType.AutoSize = true;
            MixAudioType.Location = new Point(408, 38);
            MixAudioType.Name = "MixAudioType";
            MixAudioType.Size = new Size(53, 20);
            MixAudioType.TabIndex = 3;
            MixAudioType.Text = "MType";
            // 
            // MixBitrate
            // 
            MixBitrate.AutoSize = true;
            MixBitrate.Location = new Point(467, 38);
            MixBitrate.Name = "MixBitrate";
            MixBitrate.Size = new Size(57, 20);
            MixBitrate.TabIndex = 4;
            MixBitrate.Text = "BitRate";
            // 
            // MixSampleRate
            // 
            MixSampleRate.AutoSize = true;
            MixSampleRate.Location = new Point(530, 38);
            MixSampleRate.Name = "MixSampleRate";
            MixSampleRate.Size = new Size(89, 20);
            MixSampleRate.TabIndex = 5;
            MixSampleRate.Text = "SampleRate";
            // 
            // MixSize
            // 
            MixSize.AutoSize = true;
            MixSize.Location = new Point(625, 38);
            MixSize.Name = "MixSize";
            MixSize.Size = new Size(36, 20);
            MixSize.TabIndex = 6;
            MixSize.Text = "Size";
            // 
            // MixPerformers
            // 
            MixPerformers.AutoSize = true;
            MixPerformers.Location = new Point(697, 38);
            MixPerformers.Name = "MixPerformers";
            MixPerformers.Size = new Size(104, 20);
            MixPerformers.TabIndex = 7;
            MixPerformers.Text = "MixPerformers";
            // 
            // MixCover
            // 
            MixCover.ImageLocation = "C:\\Users\\david\\AppData\\Local\\Microsoft\\VisualStudio\\18.0_113ede53\\WinFormsDesigner\\bnvyi04p.kmd\\NoImage.png";
            MixCover.Location = new Point(23, 16);
            MixCover.Name = "MixCover";
            MixCover.Size = new Size(65, 62);
            MixCover.SizeMode = PictureBoxSizeMode.StretchImage;
            MixCover.TabIndex = 8;
            MixCover.TabStop = false;
            // 
            // LoadArtButton
            // 
            LoadArtButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            LoadArtButton.Location = new Point(954, 34);
            LoadArtButton.Name = "LoadArtButton";
            LoadArtButton.Size = new Size(45, 29);
            LoadArtButton.TabIndex = 9;
            LoadArtButton.Text = "Art";
            LoadArtButton.UseVisualStyleBackColor = true;
            // 
            // TrackPlayViewControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(LoadArtButton);
            Controls.Add(MixCover);
            Controls.Add(MixPerformers);
            Controls.Add(MixSize);
            Controls.Add(MixSampleRate);
            Controls.Add(MixBitrate);
            Controls.Add(MixAudioType);
            Controls.Add(MixDuration);
            Controls.Add(MixTrackNo);
            Controls.Add(MixTitle);
            Name = "TrackPlayViewControl";
            Size = new Size(1013, 92);
            ((System.ComponentModel.ISupportInitialize)MixCover).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private BoundControls.BoundLabel MixTitle;
        private BoundControls.BoundLabel MixTrackNo;
        private BoundControls.BoundLabel MixDuration;
        private BoundControls.BoundLabel MixAudioType;
        private BoundControls.BoundLabel MixBitrate;
        private BoundControls.BoundLabel MixSampleRate;
        private BoundControls.BoundLabel MixSize;
        private BoundControls.BoundLabel MixPerformers;
        private Media.TrackCover.TrackCoverControl MixCover;
        private Button LoadArtButton;
    }
}
