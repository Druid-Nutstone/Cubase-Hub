using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Cubase.Macro.Forms.Main.Buttons
{
    public class PictureButton : PictureBox
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string HelpText
        {
            get => this.toolTip.GetToolTip(this);
            set => this.toolTip.SetToolTip(this, value);
        }

        private ToolTip toolTip = new ToolTip();

        public PictureButton()
        {
            this.Cursor = Cursors.Hand;
            this.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        public void SetBlockCursor()
        {
            this.Cursor = Cursors.WaitCursor;
            this.Update();
        }

        public void SetDefaultCursor()
        {
            this.Cursor = Cursors.Hand;
            this.Update();
        }
    }
}
