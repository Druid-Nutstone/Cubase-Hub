using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Cubase.Macro.Forms.Lyrics
{
    public class BaseRichEdit : RichTextBox, ILyricEditor
    {
        public const int EM_GETSCROLLPOS = 0x0400 + 221;
        public const int EM_SETSCROLLPOS = 0x0400 + 222;

        [StructLayout(LayoutKind.Sequential)]
        protected struct POINT
        {
            public int X;
            public int Y;
        }


        public const int WM_SETREDRAW = 0x000B;

        [DllImport("user32.dll")]
        protected static extern IntPtr SendMessage(
            IntPtr hWnd,
            int msg,
            IntPtr wParam,
            ref POINT lParam);


        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(
            IntPtr hWnd,
            int msg,
            bool wParam,
            IntPtr lParam);

        
        public BaseRichEdit() : base() 
        {
            ThemeApplier.ApplyDarkTheme(this);
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 12);
            this.BorderStyle = BorderStyle.None;

        }

        protected virtual void RefreshContent()
        {
            throw new NotImplementedException("RefreshContent must be overridien");
        }

        public void IncreaseFont()
        {
            this.Font = new Font(this.Font.FontFamily, this.Font.Size+1);
            this.RefreshContent();
        }

        public void DecreaseFont()
        {
            this.Font = new Font(this.Font.FontFamily, this.Font.Size-1);
            this.RefreshContent();
        }

        public void SetFontSize(int fontSize)
        {
            this.Font = new Font(this.Font.FontFamily, fontSize);
            this.RefreshContent();
        }
    }
}
