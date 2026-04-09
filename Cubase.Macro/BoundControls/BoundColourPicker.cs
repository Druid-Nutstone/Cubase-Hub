using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Cubase.Macro.BoundControls
{
    public class BoundColourPicker : ColorDialog
    {
        public BoundColourPicker() : base()
        {
             
        
        }

        public void Bind(string propertyName, object dataSource)
        {
            var colourValue = dataSource.GetType().GetProperty(propertyName)?.GetValue(dataSource);     
            if (colourValue != null)
            {
                int val = (int)colourValue;
                this.Color = System.Drawing.Color.FromArgb(val);
            }
        }

    }
}
