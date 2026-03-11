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
            DeleteAlbum = new Button();
            UpdateAlbum = new Button();
            ((System.ComponentModel.ISupportInitialize)CopyLink).BeginInit();
            SuspendLayout();
            // 
            // UploadSelected
            // 
            UploadSelected.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            UploadSelected.Image = Properties.Resources.arrow_up;
            UploadSelected.ImageAlign = ContentAlignment.MiddleLeft;
            UploadSelected.Location = new Point(607, 2);
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
            OpenDistribution.Location = new Point(231, 3);
            OpenDistribution.Name = "OpenDistribution";
            OpenDistribution.Size = new Size(182, 41);
            OpenDistribution.TabIndex = 1;
            OpenDistribution.Text = "Open Distribution";
            OpenDistribution.UseVisualStyleBackColor = true;
            // 
            // DeleteSelected
            // 
            DeleteSelected.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            DeleteSelected.Image = Properties.Resources.close;
            DeleteSelected.ImageAlign = ContentAlignment.MiddleLeft;
            DeleteSelected.Location = new Point(792, 2);
            DeleteSelected.Name = "DeleteSelected";
            DeleteSelected.Size = new Size(172, 41);
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
            // DeleteAlbum
            // 
            DeleteAlbum.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            DeleteAlbum.Image = Properties.Resources.close;
            DeleteAlbum.ImageAlign = ContentAlignment.MiddleLeft;
            DeleteAlbum.Location = new Point(970, 2);
            DeleteAlbum.Name = "DeleteAlbum";
            DeleteAlbum.Size = new Size(172, 41);
            DeleteAlbum.TabIndex = 5;
            DeleteAlbum.Text = "Delete Album";
            DeleteAlbum.UseVisualStyleBackColor = true;
            // 
            // UpdateAlbum
            // 
            UpdateAlbum.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            UpdateAlbum.Image = Properties.Resources.arrow_up;
            UpdateAlbum.ImageAlign = ContentAlignment.MiddleLeft;
            UpdateAlbum.Location = new Point(419, 3);
            UpdateAlbum.Name = "UpdateAlbum";
            UpdateAlbum.Size = new Size(182, 41);
            UpdateAlbum.TabIndex = 6;
            UpdateAlbum.Text = "Upload Album";
            UpdateAlbum.UseVisualStyleBackColor = true;
            // 
            // SoundCloudMainControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(UpdateAlbum);
            Controls.Add(DeleteAlbum);
            Controls.Add(CopyLink);
            Controls.Add(OpenAlbumLink);
            Controls.Add(DeleteSelected);
            Controls.Add(OpenDistribution);
            Controls.Add(UploadSelected);
            Name = "SoundCloudMainControl";
            Size = new Size(1157, 52);
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
        private Button DeleteAlbum;
        private Button UpdateAlbum;
    }
}
