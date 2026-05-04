using Cubase.Macro.Common.Models;
using Cubase.Macro.Forms.Configuration.ColourPicker;
using Cubase.Macro.Models;
using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.Text;
using Timer = System.Windows.Forms.Timer;

namespace Cubase.Macro.Forms.Main.Buttons
{
    public class MacroButton : Button
    {
        private Action<CubaseMacro, MacroButton> OnMacroClicked;

        private CubaseMacro Macro;

        private System.Windows.Forms.Timer animate;

        private Color[] borderColours = new Color[] {Color.DarkGreen, Color.White};

        public MacroButton(CubaseMacro macro, Action<CubaseMacro, MacroButton> OnMacroClicked) : base()
        {
            this.Macro = macro;
            this.OnMacroClicked = OnMacroClicked;
            this.Text = macro.Title;
            this.Dock = DockStyle.Fill;
            this.SetColoursAndTitle();
            this.Font = new Font(this.Font.FontFamily, this.Font.Size, FontStyle.Bold);
            this.Cursor = Cursors.Hand;
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 1;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.OnMacroClicked?.Invoke(this.Macro, this);
            this.FlatStyle = FlatStyle.Flat;
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

        public void SetColoursAndTitle()
        {
            if (this.Macro.ToggleState == CubaseMacroToggleState.On)
            {
                this.BackColor = Color.FromArgb(this.Macro.ToggleBackgroundColourARGB);
                this.ForeColor = Color.FromArgb(this.Macro.ToggleForegroundColourARGB);
                this.Text = this.Macro.TitleToggle;
                this.animate = new Timer();
                this.animate.Interval = 500;
                var colourIndex = 0;
                this.animate.Tick += (s, e) =>
                {
                    this.FlatAppearance.BorderColor = this.borderColours[colourIndex];
                    this.Update();
                    colourIndex = colourIndex == 0 ? 1 : 0;
                };
                animate.Start();
            }
            else
            {
                this.BackColor = Color.FromArgb(this.Macro.BackgroundColourARGB);
                this.ForeColor = Color.FromArgb(this.Macro.ForegroundColourARGB);
                this.Text = this.Macro.Title;
                this.animate?.Stop();
                this.animate?.Dispose();
            }
            this.FlatAppearance.MouseOverBackColor = this.BackColor;
            this.FlatAppearance.MouseDownBackColor = this.BackColor;
            this.FlatAppearance.BorderColor = this.BackColor;
        }
    }
}
