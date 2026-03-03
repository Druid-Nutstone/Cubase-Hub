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
            UploadSelected = new Button();
            OpenDistribution = new Button();
            CreateAlbum = new Button();
            SuspendLayout();
            // 
            // UploadSelected
            // 
            UploadSelected.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            UploadSelected.Image = Properties.Resources.arrow_up;
            UploadSelected.ImageAlign = ContentAlignment.MiddleLeft;
            UploadSelected.Location = new Point(402, 2);
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
            OpenDistribution.Location = new Point(200, 2);
            OpenDistribution.Name = "OpenDistribution";
            OpenDistribution.Size = new Size(196, 41);
            OpenDistribution.TabIndex = 1;
            OpenDistribution.Text = "Open Distribution";
            OpenDistribution.UseVisualStyleBackColor = true;
            // 
            // CreateAlbum
            // 
            CreateAlbum.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            CreateAlbum.Image = Properties.Resources.add;
            CreateAlbum.ImageAlign = ContentAlignment.MiddleLeft;
            CreateAlbum.Location = new Point(15, 2);
            CreateAlbum.Name = "CreateAlbum";
            CreateAlbum.Size = new Size(179, 41);
            CreateAlbum.TabIndex = 2;
            CreateAlbum.Text = "Create Album";
            CreateAlbum.UseVisualStyleBackColor = true;
            // 
            // SoundCloudMainControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(CreateAlbum);
            Controls.Add(OpenDistribution);
            Controls.Add(UploadSelected);
            Name = "SoundCloudMainControl";
            Size = new Size(581, 52);
            ResumeLayout(false);
        }

        #endregion

        private Button UploadSelected;
        private Button OpenDistribution;
        private Button CreateAlbum;
    }
}
