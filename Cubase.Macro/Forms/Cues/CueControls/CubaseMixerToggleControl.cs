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
            this.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        public void Bind(string propertyName, object dataSource, DataSourceUpdateMode propertyUpdateType = DataSourceUpdateMode.OnPropertyChanged)
        {
            this.DataBindings.Clear();
            this.DataBindings.Add(new Binding("Visible", dataSource, propertyName, true, propertyUpdateType));
        }
    }
}
