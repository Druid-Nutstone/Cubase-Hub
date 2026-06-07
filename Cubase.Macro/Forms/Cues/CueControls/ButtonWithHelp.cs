using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Macro.Forms.Cues.CueControls
{
    public class ButtonWithHelp : Button
    {
        private ToolTip helpProvider = new ToolTip();

        public ButtonWithHelp() : base()
        {
            Cursor = Cursors.Hand;
            this.helpProvider.AutoPopDelay = 5000;
            this.helpProvider.InitialDelay = 500;
            this.helpProvider.ReshowDelay = 100;
            this.helpProvider.ShowAlways = true;
        }

        public void SetHelpText(string helpText)
        {
            this.helpProvider.SetToolTip(this, null);
            this.helpProvider.SetToolTip(this, helpText);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && helpProvider != null)
            {
                helpProvider.Dispose();
            }
            base.Dispose(disposing);

        }
    }
}
