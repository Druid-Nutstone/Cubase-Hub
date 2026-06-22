using Cubase.Macro.Forms.Cues.CueControls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Forms.Main.Buttons
{
    public class ActionButton : ButtonWithHelp
    {
        private Action onClick;

        private Color backColour;

        public ActionButton() : base() 
        {
            this.Size = new System.Drawing.Size(30, 30);
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.Font = new Font(this.Font, FontStyle.Bold);
            this.Text = "X";
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
        }

        public void Bind(Action onClick, Color backColour, string text, string helpText)
        {
            this.onClick = onClick;
            this.backColour = backColour;
            this.BackColor = backColour;
            this.ForeColor = Color.Black;
            this.SetHelpText(helpText);
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
            this.BackColor = this.backColour;
            this.ForeColor = Color.White;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.ForeColor = Color.Black;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.onClick?.Invoke();
        }
    }
}
