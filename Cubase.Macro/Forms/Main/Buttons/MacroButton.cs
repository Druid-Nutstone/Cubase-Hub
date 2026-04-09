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
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.OnMacroClicked?.Invoke(this.Macro);
        }
    }
}
