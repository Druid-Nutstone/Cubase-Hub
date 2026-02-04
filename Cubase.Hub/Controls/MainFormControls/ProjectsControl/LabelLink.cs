using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Controls.MainFormControls.ProjectsControl
{
    public class LabelLink : Label
    {
        public LabelLink()
        {
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Font = new System.Drawing.Font(this.Font, FontStyle.Underline | FontStyle.Bold); 
        }
    }
}
