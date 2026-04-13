using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices.ObjectiveC;

namespace Cubase.Macro.Services.Window
{
    public class WindowService : IWindowService
    {
        #region Win32

        [DllImport("user32.dll")] private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")] private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")] private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")] private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        [DllImport("kernel32.dll")] private static extern uint GetCurrentThreadId();
        [DllImport("user32.dll")] private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("user32.dll")] private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")] private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(
            IntPtr hWnd,
            IntPtr hWndInsertAfter,
            int X,
            int Y,
            int cx,
            int cy,
            uint uFlags);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("dwmapi.dll")]
        private static extern int DwmGetWindowAttribute(
            IntPtr hwnd,
            int dwAttribute,
            out RECT pvAttribute,
            int cbAttribute);

        #endregion

        #region Constants

        private const int DWMWA_EXTENDED_FRAME_BOUNDS = 9;

        private const int GWL_STYLE = -16;

        private const int WS_CAPTION = 0x00C00000;
        private const int WS_THICKFRAME = 0x00040000;
        private const int WS_MINIMIZE = 0x20000000;
        private const int WS_MAXIMIZEBOX = 0x00010000;
        private const int WS_SYSMENU = 0x00080000;

        private const uint SWP_FRAMECHANGED = 0x0020;
        private const uint SWP_NOACTIVATE = 0x0010;
        private const uint SWP_SHOWWINDOW = 0x0040;

        private static readonly IntPtr HWND_TOP = IntPtr.Zero;
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

        #endregion

        #region Structs

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        private class WindowState
        {
            public int Style;
            public RECT Rect;
        }

        #endregion

        private readonly ILogger<WindowService> log;

        private static readonly Dictionary<IntPtr, WindowState> _savedStates = new();

        public WindowService(ILogger<WindowService> log)
        {
            this.log = log;
        }

        public void PositionCubase(int leftOffset)
        {
            var hWnd = GetCubaseHandle();
            if (hWnd == IntPtr.Zero)
                return;

            var screen = Screen.FromHandle(hWnd).WorkingArea;

            int width = screen.Right - leftOffset;   // ✅ correct width
            int height = screen.Bottom - screen.Top; // ✅ correct height

            var tolerence = SystemInformation.BorderSize.Width * 9;

            SetWindowPos(
                hWnd,
                HWND_TOP,
                leftOffset-tolerence,
                screen.Top,
                width+(tolerence*2),
                height+tolerence,
                SWP_SHOWWINDOW
            );
        }


        public Rectangle GetCubaseBounds()
        {
            var handle = GetCubaseHandle();

            if (handle == IntPtr.Zero)
                return Rectangle.Empty;

            GetWindowRect(handle, out var rect);

            return Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        public IntPtr GetCubaseHandle()
        {
            var process = Process.GetProcesses()
                .Where(p =>
                    p.MainWindowTitle.StartsWith("Cubase Pro Project", StringComparison.OrdinalIgnoreCase) ||
                    p.MainWindowTitle.StartsWith("Cubase Artist", StringComparison.OrdinalIgnoreCase) ||
                    p.MainWindowTitle.StartsWith("Cubase LE", StringComparison.OrdinalIgnoreCase) ||
                    p.MainWindowTitle.StartsWith("Cubase Version", StringComparison.OrdinalIgnoreCase) ||
                    p.MainWindowTitle.StartsWith("Cubase Elements", StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            return process?.MainWindowHandle ?? IntPtr.Zero;
        }

        public bool BringCubaseToFront()
        {
            var hWnd = this.GetCubaseHandle();

            if (hWnd == IntPtr.Zero)
                return false;

            SetForegroundWindow(hWnd);

            var count = 0;
            while (GetForegroundWindow() != hWnd && count < 100)
            {
                log.LogInformation("Waiting for Cubase to become foreground...");
                count++;
                Thread.Sleep(100);
            }

            return count < 100;
        }

        public bool IsCubaseActive(bool logit = true)
        {
            return this.GetCubaseHandle() != IntPtr.Zero;   
        }

        public bool IsCubaseFullscreen(int left)
        {
            return GetCubaseBounds().Left == left;  
        }

        public void MaximiseCubase()
        {
            var ptr = this.GetCubaseHandle();
            if (ptr != IntPtr.Zero)
            {
                ShowWindow(ptr, 9); // SW_Restore
                ShowWindow(ptr, 3); // SW_MAXIMIZE
            }
        }
    }
}