using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Controls.HorizontalLine
{
    public class HrControl : Label
    {
        public HrControl()
        {
            Height = 1;
            Dock = DockStyle.Top;
            BorderStyle = BorderStyle.Fixed3D;
            Margin = new Padding(0, 8, 0, 8);
        }
    }
}
