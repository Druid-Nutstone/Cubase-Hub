namespace Cubase.Hub.Controls.MainFormControls.ProjectsControl.PlayControls
{
    partial class PlayControl
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
            Play = new PictureBox();
            Stop = new PictureBox();
            Progress = new DarkProgressBar();
            ((System.ComponentModel.ISupportInitialize)Play).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Stop).BeginInit();
            SuspendLayout();
            // 
            // Play
            // 
            Play.Cursor = Cursors.Hand;
            Play.Image = Properties.Resources.Play;
            Play.InitialImage = Properties.Resources.Play;
            Play.Location = new Point(0, 5);
            Play.Name = "Play";
            Play.Size = new Size(29, 29);
            Play.SizeMode = PictureBoxSizeMode.StretchImage;
            Play.TabIndex = 0;
            Play.TabStop = false;
            // 
            // Stop
            // 
            Stop.Cursor = Cursors.Hand;
            Stop.Image = Properties.Resources.Stop;
            Stop.InitialImage = Properties.Resources.Play;
            Stop.Location = new Point(30, 5);
            Stop.Name = "Stop";
            Stop.Size = new Size(29, 29);
            Stop.SizeMode = PictureBoxSizeMode.StretchImage;
            Stop.TabIndex = 1;
            Stop.TabStop = false;
            // 
            // Progress
            // 
            Progress.Location = new Point(60, 5);
            Progress.Name = "Progress";
            Progress.Size = new Size(179, 29);
            Progress.TabIndex = 4;
            Progress.Text = "00:00";
            // 
            // PlayControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            Controls.Add(Progress);
            Controls.Add(Stop);
            Controls.Add(Play);
            Name = "PlayControl";
            Size = new Size(249, 40);
            ((System.ComponentModel.ISupportInitialize)Play).EndInit();
            ((System.ComponentModel.ISupportInitialize)Stop).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox Play;
        private PictureBox Stop;
        private DarkProgressBar Progress;
    }
}
