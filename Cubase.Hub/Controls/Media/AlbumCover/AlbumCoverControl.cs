using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.IO;
using Cubase.Hub.Services;

namespace Cubase.Hub.Controls.Media.AlbumCover
{
    public class AlbumCoverControl : PictureBox
    {

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string AlbumCoverFileName { get; set; }
        
        public AlbumCoverControl() : base() 
        {
            this.SizeMode = PictureBoxSizeMode.StretchImage; 
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.RefreshImage();
        }
        

        public void RefreshImage()
        {
            if (string.IsNullOrEmpty(AlbumCoverFileName))
            {
                this.ImageLocation = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), CubaseHubConstants.DefaultAlbumArt);
            }
            else
            {
                this.ImageLocation = this.AlbumCoverFileName;
            }
        }
    }
}
