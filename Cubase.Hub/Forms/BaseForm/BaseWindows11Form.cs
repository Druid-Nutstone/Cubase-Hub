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

                // Optional: dark title bar if app uses dark theme
                //int dark = 1;
                //DwmSetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref dark, sizeof(int));
            }
        }

        private enum DWMWINDOWATTRIBUTE
        {
            DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
            DWMWA_SYSTEMBACKDROP_TYPE = 38,
            DWMWA_CAPTION_COLOR = 35,
            DWMWA_TEXT_COLOR = 36,
            DWMWA_BORDER_COLOR = 34
        }


        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attribute, ref int pvAttribute, int cbAttribute);
    }
}
