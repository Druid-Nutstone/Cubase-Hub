using Cubase.Hub.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Cubase.Hub.Controls.BoundControls
{
    public class ClipBoardCopyControl : PictureBox
    {
        private ToolTip tooltip;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Func<string> GetClipBoardText { get; set; }

        public ClipBoardCopyControl() : base() 
        {
            this.Image = Resources.ed_copy;
            this.SizeMode = PictureBoxSizeMode.AutoSize;    
            this.Cursor = Cursors.Hand;
            this.tooltip = new ToolTip();
            this.tooltip.SetToolTip(this, "Click to copy to clipboard");
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Clipboard.SetText(this.GetClipBoardText() ?? string.Empty);
            this.tooltip.Show("Copied to clipboard", this);

        }
    }
}
