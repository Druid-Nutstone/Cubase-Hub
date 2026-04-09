using Cubase.Macro.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Forms.Main.Buttons
{
    public class MacroButtonPanel : Panel
    {
        public MacroButtonPanel(CubaseMacro macro, Action<CubaseMacro, MacroButton> OnMacroClicked) : base()
        {

            // apply theme now so buttons can override colour
            ThemeApplier.ApplyDarkTheme(this);
            this.Dock = DockStyle.Fill; // 👈 key

            var button = new MacroButton(macro, OnMacroClicked);
            button.Dock = DockStyle.Fill; // 👈 key

            this.Controls.Add(button);
        }

    }
}
