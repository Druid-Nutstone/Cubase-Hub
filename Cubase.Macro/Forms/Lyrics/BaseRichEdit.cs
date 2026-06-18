using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Cubase.Macro.Forms.Lyrics
{
    public class BaseRichEdit : RichTextBox
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
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 12);
            this.BorderStyle = BorderStyle.None;
        }
    }
}
