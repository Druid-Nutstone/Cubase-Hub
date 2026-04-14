using Cubase.Macro.Services.Config;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Cubase.Macro.Services.Window
{
    public enum ExternalWindowState
    {
        Hide = 0,
        Normal = 1,
        Minimized = 2,
        Maximized = 3,
        Show = 5,
        Restore = 9,
        Unknown = 99
    }

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

        [DllImport("user32.dll")]
        private static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

        delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")] 
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool BringWindowToTop(IntPtr hWnd);

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

        [DllImport("user32.dll")]
        private static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

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

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public Rect rcNormalPosition;
        }

        #endregion

        private readonly ILogger<WindowService> log;

        private static readonly Dictionary<IntPtr, WindowState> _savedStates = new();

        private readonly IConfigurationService configurationService;

        public WindowService(ILogger<WindowService> log, IConfigurationService configurationService)
        {
            this.log = log;
            this.configurationService = configurationService;
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


        private IntPtr FindCubaseMainWindow()
        {
            IntPtr result = IntPtr.Zero;

            EnumWindows((hWnd, lParam) =>
            {
                GetWindowThreadProcessId(hWnd, out uint pid);

                try
                {
                    var proc = Process.GetProcessById((int)pid);
                    if (proc.ProcessName.Equals(this.configurationService.Configuration.CubaseExecutable, StringComparison.OrdinalIgnoreCase))
                    {
                        result = hWnd;
                        return false; // stop enumeration
                    }
                }
                catch { }

                return true;
            }, IntPtr.Zero);

            return result;
        }


        public IntPtr GetCubaseHandle()
        {
            var cubaseHandle = this.FindCubaseMainWindow();
            if (cubaseHandle == IntPtr.Zero) return IntPtr.Zero;
            var hwnd = Process.GetProcesses().FirstOrDefault(p => p.MainWindowTitle.StartsWith(this.configurationService.Configuration.CubaseProjectWindowName, StringComparison.OrdinalIgnoreCase) && p.MainWindowHandle == cubaseHandle)?.MainWindowHandle;
            return hwnd ?? cubaseHandle;

        }

        public bool BringCubaseToFront()
        {
            var hWnd = this.GetCubaseHandle();

            if (hWnd == IntPtr.Zero)
                return false;


            uint targetThreadId = GetWindowThreadProcessId(hWnd, out uint processId);
            uint currentThreadId = GetCurrentThreadId();

            try
            {
                // Attach input threads
                if (currentThreadId != targetThreadId)
                {
                    AttachThreadInput(currentThreadId, targetThreadId, true);
                }

                // Bring to front
                BringWindowToTop(hWnd);
                SetForegroundWindow(hWnd);
            }
            finally
            {
                // Always detach!
                if (currentThreadId != targetThreadId)
                {
                    AttachThreadInput(currentThreadId, targetThreadId, false);
                }
            }

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

        public bool IsCubaseMainWindowActive()
        {
            var cubaseHandle = this.FindCubaseMainWindow();
            if (cubaseHandle == IntPtr.Zero) return false;
            var hwnd = Process.GetProcesses().FirstOrDefault(p => p.MainWindowTitle.StartsWith(this.configurationService.Configuration.CubaseProjectWindowName, StringComparison.OrdinalIgnoreCase))?.MainWindowHandle;
            return hwnd != null;
        }

        public bool IsCubaseRunning()
        {
            var process = Process.GetProcesses().FirstOrDefault(x => x.ProcessName.Equals(this.configurationService.Configuration.CubaseExecutable, StringComparison.OrdinalIgnoreCase));
            return process != null;
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

        public ExternalWindowState GetCubaseWindowState()
        {
            var ptr = this.GetCubaseHandle();
            if (ptr != IntPtr.Zero)
            {
                return GetWindowState(ptr);
            }
            return ExternalWindowState.Unknown;
        }

        private ExternalWindowState GetWindowState(IntPtr hWnd)
        {
            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
            placement.length = Marshal.SizeOf(placement);
            GetWindowPlacement(hWnd, ref placement);
            return Enum.GetValues<ExternalWindowState>().Contains((ExternalWindowState)placement.showCmd) ? (ExternalWindowState)placement.showCmd : ExternalWindowState.Unknown;
        }
    }
}