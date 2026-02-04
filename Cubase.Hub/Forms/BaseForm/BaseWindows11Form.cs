using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Cubase.Hub.Forms.BaseForm
{
    public partial class BaseWindows11Form : Form
    {
        protected void ApplyWindows11Look()
        {
            if (!OperatingSystem.IsWindowsVersionAtLeast(10, 0, 22000))
                return;

            var hWnd = Handle;

            // 1️⃣ Enable dark title bar
            int dark = 1;
            DwmSetWindowAttribute(
                hWnd,
                DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE,
                ref dark,
                sizeof(int)
            );

            // 2️⃣ Enable Mica
            int backdrop = (int)DWM_SYSTEMBACKDROP_TYPE.DWMSBT_MAINWINDOW;
            DwmSetWindowAttribute(
                hWnd,
                DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
                ref backdrop,
                sizeof(int)
            );

            /*

            if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 22000))
            {
                // Remove border and title color difference
                var hWnd = this.Handle;

                // Enable mica effect (requires Windows 11 22H2+)
                int trueValue = 1;
                DwmSetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE, ref trueValue, sizeof(int));

                int captionColor = (int)ColorTranslator.ToWin32(SystemColors.Control);
                DwmSetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.DWMWA_CAPTION_COLOR, ref captionColor, sizeof(uint));

                // Optional: set text color for better readability
                int textColor = (int)ColorTranslator.ToWin32(SystemColors.ControlText);
                DwmSetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.DWMWA_TEXT_COLOR, ref textColor, sizeof(uint));

                // Optional: match border color too (to remove dark border)
                int borderColor = (int)ColorTranslator.ToWin32(SystemColors.Control);
                DwmSetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.DWMWA_BORDER_COLOR, ref borderColor, sizeof(uint));

                int dark = 1;
                DwmSetWindowAttribute(
                    hWnd,
                    DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE,
                    ref dark,
                    sizeof(int)
                );

                int backdrop = 1;
                DwmSetWindowAttribute(
                    hWnd,
                    DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
                    ref backdrop,
                    sizeof(int)
                );
            
            }
            */
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ApplyWindows11Look();
        }

        private enum DWMWINDOWATTRIBUTE
        {
            DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
            DWMWA_SYSTEMBACKDROP_TYPE = 38,
            DWMWA_CAPTION_COLOR = 35,
            DWMWA_TEXT_COLOR = 36,
            DWMWA_BORDER_COLOR = 34
        }

        enum DWM_SYSTEMBACKDROP_TYPE
        {
            DWMSBT_AUTO = 0,
            DWMSBT_NONE = 1,
            DWMSBT_MAINWINDOW = 2, // ✅ Mica
            DWMSBT_TRANSIENTWINDOW = 3,
            DWMSBT_TABBEDWINDOW = 4
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attribute, ref int pvAttribute, int cbAttribute);
    }

    public static class DarkTheme
    {
        public static Color BackColor = Color.FromArgb(32, 32, 32);
        public static Color PanelColor = Color.FromArgb(37, 37, 38);
        public static Color ControlColor = Color.FromArgb(45, 45, 48);

        public static Color TextColor = Color.FromArgb(220, 220, 220);
        public static Color MutedText = Color.FromArgb(160, 160, 160);
        public static Color BorderColor = Color.FromArgb(60, 60, 60);
    }

    public static class ThemeApplier
    {
        public static void ApplyDarkTheme(Control control)
        {
            control.BackColor = DarkTheme.BackColor;
            control.ForeColor = DarkTheme.TextColor;

            if (control is Panel or TableLayoutPanel)
                control.BackColor = DarkTheme.PanelColor;

            if (control is SplitContainer splitter)
            {
                splitter.BackColor = DarkTheme.ControlColor;
            }

            if (control is TextBox tb)
            {
                tb.BorderStyle = BorderStyle.FixedSingle;
                tb.BackColor = DarkTheme.ControlColor;
                tb.ForeColor = DarkTheme.TextColor;
            }

            if (control is Button btn)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderColor = DarkTheme.BorderColor;
                btn.BackColor = DarkTheme.ControlColor;
                btn.ForeColor = DarkTheme.TextColor;
            }

            if (control is LinkLabel linkLabel)
            {
                linkLabel.LinkBehavior = LinkBehavior.NeverUnderline; // 🔑 disables system style

                linkLabel.LinkColor = DarkTheme.TextColor;
                linkLabel.ActiveLinkColor = DarkTheme.TextColor;
                linkLabel.VisitedLinkColor = DarkTheme.MutedText;
                linkLabel.DisabledLinkColor = DarkTheme.MutedText;

                linkLabel.ForeColor = DarkTheme.TextColor; // still required
            }

            foreach (Control child in control.Controls)
                ApplyDarkTheme(child);
        }
    }

}
