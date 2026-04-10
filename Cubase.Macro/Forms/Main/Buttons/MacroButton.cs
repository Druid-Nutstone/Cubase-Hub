using Cubase.Macro.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Forms.Main.Buttons
{
    public class MacroButton : Button
    {
        private Action<CubaseMacro, MacroButton> OnMacroClicked;

        private CubaseMacro Macro;

        public MacroButton(CubaseMacro macro, Action<CubaseMacro, MacroButton> OnMacroClicked) : base()
        {
            this.Macro = macro;
            this.OnMacroClicked = OnMacroClicked;
            this.Text = macro.Title;
            this.Dock = DockStyle.Fill;
            this.SetColoursAndTitle();
            this.Font = new Font(this.Font.FontFamily, this.Font.Size+2, FontStyle.Bold);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.SetColoursAndTitle();
            this.OnMacroClicked?.Invoke(this.Macro, this);
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;

        }

        public void SetColoursAndTitle()
        {
            if (this.Macro.ToggleState == CubaseMacroToggleState.On)
            {
                this.BackColor = Color.FromArgb(this.Macro.ToggleBackgroundColourARGB);
                this.ForeColor = Color.FromArgb(this.Macro.ToggleForegroundColourARGB);
                this.Text = this.Macro.TitleToggle;
            }
            else
            {
                this.BackColor = Color.FromArgb(this.Macro.BackgroundColourARGB);
                this.ForeColor = Color.FromArgb(this.Macro.ForegroundColourARGB);
                this.Text = this.Macro.Title;
            }
            this.FlatAppearance.MouseOverBackColor = this.BackColor;
            this.FlatAppearance.MouseDownBackColor = this.BackColor;
        }
    }
}
