using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.IO;
using Cubase.Hub.Services;

namespace Cubase.Hub.Controls.Media.TrackCover
{
    public class TrackCoverControl : PictureBox
    {

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TrackCoverFileName { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action OnClicked { get; set; }

        public TrackCoverControl() : base() 
        {
            this.SizeMode = PictureBoxSizeMode.StretchImage; 
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.RefreshImage();
            this.Cursor = Cursors.Hand;
            this.Click += (s, e) => { this.OnClicked?.Invoke(); };
        }

        public void RefreshImage() 
        {
            if (string.IsNullOrEmpty(TrackCoverFileName))
            {
                this.ImageLocation = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), CubaseHubConstants.DefaultAlbumArt);
            }
            else
            {
                this.ImageLocation = this.TrackCoverFileName;
            }
        }
    }
}
