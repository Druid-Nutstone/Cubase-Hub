using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Forms.Cues.CueControls
{
    public class CubaseMixerToggleControl : PictureBox
    {
        private Bitmap? enabledImage;
        private Bitmap? disabledImage;

        public CubaseMixerToggleControl() : base() 
        {
            this.Size = new System.Drawing.Size(24, 24);
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Cursor = Cursors.Hand; 
        }

        public void Bind(string propertyName, object dataSource, Bitmap enabled, Bitmap disabled, DataSourceUpdateMode propertyUpdateType = DataSourceUpdateMode.OnPropertyChanged)
        {
            this.enabledImage = enabled;
            this.disabledImage = disabled;
            this.DataBindings.Clear();
            this.DataBindings.Add(new Binding("Enabled", dataSource, propertyName, true, propertyUpdateType));
            this.UpdateImage(); 
        }

        private void UpdateImage()
        {
            this.Image = this.Enabled ? (this.enabledImage == null ? this.Image : this.enabledImage) 
                                      : (this.disabledImage == null ? this.Image : this.disabledImage);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            this.UpdateImage();
        }
    }
}
