using Cubase.Macro.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Forms.Main.Buttons
{
    public class MacroCommonButton : MacroButton
    {
        private Action<CubaseMacro, MacroButton> OnMacroClicked;

        private CubaseMacro Macro;

        private System.Windows.Forms.Timer animate;

        private Color[] borderColours = new Color[] { Color.DarkGreen, Color.White };

        public MacroCommonButton(CubaseMacro macro, Action<CubaseMacro, MacroButton> OnMacroClicked) : base(macro, OnMacroClicked)
        {
            this.Macro = macro;
            this.OnMacroClicked = OnMacroClicked;
            this.Text = macro.Title;
            this.Dock = DockStyle.Fill;
            this.SetColoursAndTitle();
            this.Font = new Font(this.Font.FontFamily, this.Font.Size - 2, FontStyle.Bold);
            this.Cursor = Cursors.Hand;
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 1;
            this.Width = 60;
            this.Height = 120;
        }
    }
}
