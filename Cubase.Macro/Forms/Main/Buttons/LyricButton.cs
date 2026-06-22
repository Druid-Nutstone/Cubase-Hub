using Cubase.Macro.Forms.Cues.CueControls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Forms.Main.Buttons
{
    public class LyricButton : ButtonWithHelp
    {
        private Action onClick;

        public LyricButton() : base() 
        {
            this.Size = new System.Drawing.Size(30, 30);
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.Font = new Font(this.Font, FontStyle.Bold);
            this.Text = "X";
        }

        public void Bind(Action onClick, string text, string helpText)
        {
            this.Text = text;
            this.onClick = onClick;
            this.SetHelpText(helpText);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.onClick?.Invoke();
        }
    }
}
