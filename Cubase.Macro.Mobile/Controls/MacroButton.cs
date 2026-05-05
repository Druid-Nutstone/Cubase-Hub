using Cubase.Macro.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Mobile.Controls
{
    public class MacroButton : Button
    {
        private CubaseMacro macro;
        
        public bool Toggled {  get; set; } = false;

        public Action<CubaseMacro, bool>? OnMacroClicked { get; set; }

        public MacroButton(CubaseMacro macro) : base()
        {
            this.macro = macro;
            this.Margin = new Thickness(5);
            this.Clicked += MacroButton_Clicked;
            WidthRequest = 120;
            HeightRequest = 50;
            this.Initialise();
        }

        private void MacroButton_Clicked(object? sender, EventArgs e)
        {
            if (this.macro.ButtonType == CubaseMacroButtonType.Toggle)
            {
                this.Toggled = !this.Toggled;
            }
            OnMacroClicked?.Invoke(macro, Toggled);
            if (this.Toggled)
            {
                this.Text = macro.TitleToggle;
            }
            else
            {
                this.Text = macro.Title;
            }
            this.SetButtonColours();
        }

        private void Initialise()
        {
            this.Text = this.macro.Title;
            this.SetButtonColours();
        }

        private void SetButtonColours()
        {
            if (!this.Toggled)
            {
                this.BackgroundColor = macro.BackgroundColourARGB.ToMauiColor();
                this.TextColor = macro.ForegroundColourARGB.ToMauiColor(); 
            }
            else
            {
                this.BackgroundColor = macro.ToggleBackgroundColourARGB.ToMauiColor();
                this.TextColor = macro.ToggleForegroundColourARGB.ToMauiColor();
            }
        }

    }

    public static class ColorExtensions
    {
        public static Color ToMauiColor(this int argb)
        {
            int a = (argb >> 24) & 0xFF;
            int r = (argb >> 16) & 0xFF;
            int g = (argb >> 8) & 0xFF;
            int b = argb & 0xFF;
            return Color.FromRgba(r, g, b, a);
        }
    }
}
