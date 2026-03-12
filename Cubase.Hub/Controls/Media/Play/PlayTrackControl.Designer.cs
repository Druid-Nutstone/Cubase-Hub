namespace Cubase.Hub.Controls.Media.Play
{
    partial class PlayTrackControl
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
            Progress = new Cubase.Hub.Controls.MainFormControls.ProjectsControl.PlayControls.DarkProgressBar();
            Volume = new TrackBar();
            Play = new PictureBox();
            Stop = new PictureBox();
            TrackName = new Label();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)Volume).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Play).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Stop).BeginInit();
            SuspendLayout();
            // 
            // Progress
            // 
            Progress.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Progress.Location = new Point(115, 34);
            Progress.Name = "Progress";
            Progress.Size = new Size(761, 32);
            Progress.TabIndex = 0;
            Progress.Text = "darkProgressBar1";
            // 
            // Volume
            // 
            Volume.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Volume.AutoSize = false;
            Volume.Location = new Point(107, 70);
            Volume.Name = "Volume";
            Volume.Size = new Size(781, 32);
            Volume.TabIndex = 1;
            // 
            // Play
            // 
            Play.Cursor = Cursors.Hand;
            Play.Image = Properties.Resources.Play;
            Play.Location = new Point(20, 34);
            Play.Name = "Play";
            Play.Size = new Size(32, 32);
            Play.SizeMode = PictureBoxSizeMode.StretchImage;
            Play.TabIndex = 2;
            Play.TabStop = false;
            // 
            // Stop
            // 
            Stop.Cursor = Cursors.Hand;
            Stop.Image = Properties.Resources.Stop;
            Stop.Location = new Point(58, 34);
            Stop.Name = "Stop";
            Stop.Size = new Size(32, 32);
            Stop.SizeMode = PictureBoxSizeMode.StretchImage;
            Stop.TabIndex = 3;
            Stop.TabStop = false;
            // 
            // TrackName
            // 
            TrackName.AutoSize = true;
            TrackName.Location = new Point(115, 11);
            TrackName.Name = "TrackName";
            TrackName.Size = new Size(202, 20);
            TrackName.TabIndex = 4;
            TrackName.Text = "This will show the track name";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 78);
            label1.Name = "label1";
            label1.Size = new Size(59, 20);
            label1.TabIndex = 5;
            label1.Text = "Volume";
            // 
            // PlayTrackControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(label1);
            Controls.Add(TrackName);
            Controls.Add(Stop);
            Controls.Add(Play);
            Controls.Add(Volume);
            Controls.Add(Progress);
            Name = "PlayTrackControl";
            Size = new Size(917, 108);
            ((System.ComponentModel.ISupportInitialize)Volume).EndInit();
            ((System.ComponentModel.ISupportInitialize)Play).EndInit();
            ((System.ComponentModel.ISupportInitialize)Stop).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MainFormControls.ProjectsControl.PlayControls.DarkProgressBar Progress;
        private TrackBar Volume;
        private PictureBox Play;
        private PictureBox Stop;
        private Label TrackName;
        private Label label1;
    }
}
