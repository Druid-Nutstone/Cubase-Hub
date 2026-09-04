using Cubase.Hub.Services;
using System.ComponentModel;
using System.IO;

namespace Cubase.Hub.Controls.Media.AlbumCover
{
    public class AlbumCoverControl : PictureBox
    {

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string AlbumCoverFileName { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action OnClicked { get; set; }

        public AlbumCoverControl() : base()
        {
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Cursor = Cursors.Hand;
            this.Click += (s, e) => { this.OnClicked?.Invoke(); };
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
