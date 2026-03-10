namespace Cubase.Hub.Forms.Distributers.SoundCloud
{
    partial class SoundCloudTrackControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SoundCloudTrackControl));
            groupBox1 = new GroupBox();
            LastUploaded = new Cubase.Hub.Controls.BoundControls.BoundLabel();
            CopyLink = new Cubase.Hub.Controls.BoundControls.ClipBoardCopyControl();
            DeleteTrack = new Button();
            ReUploadTrack = new Button();
            TrackOnSoundCloud = new LinkLabel();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)CopyLink).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(LastUploaded);
            groupBox1.Controls.Add(CopyLink);
            groupBox1.Controls.Add(DeleteTrack);
            groupBox1.Controls.Add(ReUploadTrack);
            groupBox1.Controls.Add(TrackOnSoundCloud);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(694, 61);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Sound Cloud ";
            // 
            // LastUploaded
            // 
            LastUploaded.AutoSize = true;
            LastUploaded.Location = new Point(221, 23);
            LastUploaded.Name = "LastUploaded";
            LastUploaded.Size = new Size(101, 20);
            LastUploaded.TabIndex = 5;
            LastUploaded.Text = "LastUploaded";
            // 
            // CopyLink
            // 
            CopyLink.Image = (Image)resources.GetObject("CopyLink.Image");
            CopyLink.Location = new Point(13, 23);
            CopyLink.Name = "CopyLink";
            CopyLink.Size = new Size(22, 22);
            CopyLink.SizeMode = PictureBoxSizeMode.AutoSize;
            CopyLink.TabIndex = 4;
            CopyLink.TabStop = false;
            // 
            // DeleteTrack
            // 
            DeleteTrack.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            DeleteTrack.Image = Properties.Resources.close;
            DeleteTrack.ImageAlign = ContentAlignment.MiddleLeft;
            DeleteTrack.Location = new Point(555, 15);
            DeleteTrack.Name = "DeleteTrack";
            DeleteTrack.Size = new Size(123, 36);
            DeleteTrack.TabIndex = 3;
            DeleteTrack.Text = "Delete";
            DeleteTrack.UseVisualStyleBackColor = true;
            // 
            // ReUploadTrack
            // 
            ReUploadTrack.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ReUploadTrack.Image = Properties.Resources.arrow_up;
            ReUploadTrack.ImageAlign = ContentAlignment.MiddleLeft;
            ReUploadTrack.Location = new Point(426, 15);
            ReUploadTrack.Name = "ReUploadTrack";
            ReUploadTrack.Size = new Size(123, 36);
            ReUploadTrack.TabIndex = 2;
            ReUploadTrack.Text = "Upload";
            ReUploadTrack.UseVisualStyleBackColor = true;
            // 
            // TrackOnSoundCloud
            // 
            TrackOnSoundCloud.AutoSize = true;
            TrackOnSoundCloud.Location = new Point(45, 23);
            TrackOnSoundCloud.Name = "TrackOnSoundCloud";
            TrackOnSoundCloud.Size = new Size(140, 20);
            TrackOnSoundCloud.TabIndex = 0;
            TrackOnSoundCloud.TabStop = true;
            TrackOnSoundCloud.Text = "Not on SoundCloud";
            // 
            // SoundCloudTrackControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(groupBox1);
            Name = "SoundCloudTrackControl";
            Size = new Size(694, 61);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)CopyLink).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private LinkLabel TrackOnSoundCloud;
        private Button DeleteTrack;
        private Button ReUploadTrack;
        private Controls.BoundControls.ClipBoardCopyControl CopyLink;
        private Controls.BoundControls.BoundLabel LastUploaded;
    }
}
