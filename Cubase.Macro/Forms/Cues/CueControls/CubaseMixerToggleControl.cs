using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Forms.Cues.CueControls
{
    public class CubaseMixerToggleControl : PictureBox
    {
        public CubaseMixerToggleControl() : base() 
        {
            this.Size = new System.Drawing.Size(30, 30);
            this.SizeMode = PictureBoxSizeMode.Normal;
        }

        public void Bind(string propertyName, object dataSource, Bitmap bitmap, DataSourceUpdateMode propertyUpdateType = DataSourceUpdateMode.OnPropertyChanged)
        {
            this.Image = bitmap;
            this.DataBindings.Clear();
            this.DataBindings.Add(new Binding("Visible", dataSource, propertyName, true, propertyUpdateType));
        }
    }
}
