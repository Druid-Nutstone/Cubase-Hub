using Cubase.Macro.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Forms.Main.Buttons
{
    public class MacroButton : Button
    {
        private Action<CubaseMacro> OnMacroClicked;

        private CubaseMacro Macro;

        public MacroButton(CubaseMacro macro, Action<CubaseMacro> OnMacroClicked) : base()
        {
            this.Macro = macro;
            this.OnMacroClicked = OnMacroClicked;
            this.Text = macro.Title;
            this.Dock = DockStyle.Fill;
            this.SetColours();
            this.Font = new Font(this.Font, FontStyle.Bold);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.Macro.ToggleState = this.Macro.ToggleState == CubaseMacroToggleState.On ? CubaseMacroToggleState.Off : CubaseMacroToggleState.On;
            this.SetColours();
            this.OnMacroClicked?.Invoke(this.Macro);
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;

        }

        private void SetColours()
        {
            if (this.Macro.ToggleState == CubaseMacroToggleState.On)
            {
                this.BackColor = Color.FromArgb(this.Macro.ToggleBackgroundColourARGB);
                this.ForeColor = Color.FromArgb(this.Macro.ToggleForegroundColourARGB);

            }
            else
            {
                this.BackColor = Color.FromArgb(this.Macro.BackgroundColourARGB);
                this.ForeColor = Color.FromArgb(this.Macro.ForegroundColourARGB);

            }
            this.FlatAppearance.MouseOverBackColor = this.BackColor;
            this.FlatAppearance.MouseDownBackColor = this.BackColor;
        }
    }
}
