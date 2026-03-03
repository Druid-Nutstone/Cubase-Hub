namespace Cubase.Hub.Forms.Distributers.SoundCloud
{
    partial class SoundCloudMainControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SoundCloudMainControl));
            UploadSelected = new Button();
            OpenDistribution = new Button();
            DeleteSelected = new Button();
            OpenAlbumLink = new LinkLabel();
            CopyLink = new Cubase.Hub.Controls.BoundControls.ClipBoardCopyControl();
            ((System.ComponentModel.ISupportInitialize)CopyLink).BeginInit();
            SuspendLayout();
            // 
            // UploadSelected
            // 
            UploadSelected.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            UploadSelected.Image = Properties.Resources.arrow_up;
            UploadSelected.ImageAlign = ContentAlignment.MiddleLeft;
            UploadSelected.Location = new Point(620, 2);
            UploadSelected.Name = "UploadSelected";
            UploadSelected.Size = new Size(179, 41);
            UploadSelected.TabIndex = 0;
            UploadSelected.Text = "Upload Selected";
            UploadSelected.UseVisualStyleBackColor = true;
            // 
            // OpenDistribution
            // 
            OpenDistribution.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            OpenDistribution.Image = Properties.Resources.zoomin;
            OpenDistribution.ImageAlign = ContentAlignment.MiddleLeft;
            OpenDistribution.Location = new Point(218, 2);
            OpenDistribution.Name = "OpenDistribution";
            OpenDistribution.Size = new Size(196, 41);
            OpenDistribution.TabIndex = 1;
            OpenDistribution.Text = "Open Distribution";
            OpenDistribution.UseVisualStyleBackColor = true;
            // 
            // DeleteSelected
            // 
            DeleteSelected.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            DeleteSelected.Image = Properties.Resources.close;
            DeleteSelected.ImageAlign = ContentAlignment.MiddleLeft;
            DeleteSelected.Location = new Point(418, 2);
            DeleteSelected.Name = "DeleteSelected";
            DeleteSelected.Size = new Size(196, 41);
            DeleteSelected.TabIndex = 2;
            DeleteSelected.Text = "Delete Selected";
            DeleteSelected.UseVisualStyleBackColor = true;
            // 
            // OpenAlbumLink
            // 
            OpenAlbumLink.AutoSize = true;
            OpenAlbumLink.Location = new Point(48, 12);
            OpenAlbumLink.Name = "OpenAlbumLink";
            OpenAlbumLink.Size = new Size(130, 20);
            OpenAlbumLink.TabIndex = 3;
            OpenAlbumLink.TabStop = true;
            OpenAlbumLink.Text = "Open SoundCloud";
            // 
            // CopyLink
            // 
            CopyLink.Image = (Image)resources.GetObject("CopyLink.Image");
            CopyLink.Location = new Point(20, 12);
            CopyLink.Name = "CopyLink";
            CopyLink.Size = new Size(22, 22);
            CopyLink.SizeMode = PictureBoxSizeMode.AutoSize;
            CopyLink.TabIndex = 4;
            CopyLink.TabStop = false;
            // 
            // SoundCloudMainControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(CopyLink);
            Controls.Add(OpenAlbumLink);
            Controls.Add(DeleteSelected);
            Controls.Add(OpenDistribution);
            Controls.Add(UploadSelected);
            Name = "SoundCloudMainControl";
            Size = new Size(799, 52);
            ((System.ComponentModel.ISupportInitialize)CopyLink).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button UploadSelected;
        private Button OpenDistribution;
        private Button DeleteSelected;
        private LinkLabel OpenAlbumLink;
        private Controls.BoundControls.ClipBoardCopyControl CopyLink;
    }
}
