using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace Cubase.Macro.Services.Window
{
    public class WindowService : IWindowService
    {
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport("user32.dll")]
        private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessID);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool Process32First(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool Process32Next(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        [DllImport("kernel32.dll")]
        static extern bool CloseHandle(IntPtr hHandle);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("dwmapi.dll")]
        private static extern int DwmGetWindowAttribute(
            IntPtr hwnd,
            int dwAttribute,
            out RECT pvAttribute,
            int cbAttribute);

        private const int DWMWA_EXTENDED_FRAME_BOUNDS = 9;

        const uint TH32CS_SNAPPROCESS = 0x00000002;

        [StructLayout(LayoutKind.Sequential)]
        struct PROCESSENTRY32
        {
            public uint dwSize;
            public uint cntUsage;
            public uint th32ProcessID;
            public IntPtr th32DefaultHeapID;
            public uint th32ModuleID;
            public uint cntThreads;
            public uint th32ParentProcessID;
            public int pcPriClassBase;
            public uint dwFlags;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szExeFile;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        private readonly ILogger<WindowService> log;

        public WindowService(ILogger<WindowService> log)
        {
            this.log = log;
        }

        public Rectangle GetCubaseBounds()
        {
            var handle = GetCubaseHandle();

            if (handle == IntPtr.Zero)
                return Rectangle.Empty;

            if (DwmGetWindowAttribute(handle, DWMWA_EXTENDED_FRAME_BOUNDS, out RECT rect, Marshal.SizeOf<RECT>()) == 0)
            {
                return Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
            }

            return Rectangle.Empty;
        }

        public IntPtr GetCubaseHandle()
        {
            var processes = Process.GetProcesses().Where(p => p.MainWindowTitle.StartsWith("Cubase Pro Project", StringComparison.OrdinalIgnoreCase) ||
                     p.MainWindowTitle.StartsWith("Cubase Artist", StringComparison.OrdinalIgnoreCase) ||
                     p.MainWindowTitle.StartsWith("Cubase LE", StringComparison.OrdinalIgnoreCase) ||
                     p.MainWindowTitle.StartsWith("Cubase Version", StringComparison.OrdinalIgnoreCase) ||
                     p.MainWindowTitle.StartsWith("Cubase Elements", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            if (processes == null)
                return IntPtr.Zero;

            return processes == null ? IntPtr.Zero : processes.MainWindowHandle;
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
                this.log.LogInformation("Waiting for Cubase to become foreground...");
                count++;
                Thread.Sleep(100);
            }

            return count < 100;
        }

        public static IEnumerable<Process> GetChildProcesses(Process parent)
        {
            var result = new List<Process>();

            IntPtr snapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);

            if (snapshot == IntPtr.Zero || snapshot == (IntPtr)(-1))
                return result;

            try
            {
                var entry = new PROCESSENTRY32();
                entry.dwSize = (uint)Marshal.SizeOf(typeof(PROCESSENTRY32));

                if (Process32First(snapshot, ref entry))
                {
                    do
                    {
                        if (entry.th32ParentProcessID == parent.Id)
                        {
                            try
                            {
                                var child = Process.GetProcessById((int)entry.th32ProcessID);
                                result.Add(child);
                            }
                            catch
                            {
                                // Process may have exited
                            }
                        }
                    }
                    while (Process32Next(snapshot, ref entry));
                }
            }
            finally
            {
                CloseHandle(snapshot);
            }

            return result;
        }

        public bool IsCubaseActive(bool logit = true)
        {
            IntPtr hwnd = GetForegroundWindow();

            if (hwnd == IntPtr.Zero)
                return false;

            GetWindowThreadProcessId(hwnd, out uint pid);

            try
            {
                var process = Process.GetProcessById((int)pid);
                if (logit)
                {
                    this.log.LogInformation($"Cubase process?: {process.ProcessName} (PID: {pid})");
                }
                // Debug.WriteLine($"Active process: {process.ProcessName} (PID: {pid})");

                // Check process name (no .exe)
                return process.ProcessName.StartsWith("Cubase", StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                this.log.LogError(ex, $"Error checking active process for Cubase (PID: {pid})");
                return false;
            }
        }
    }
}
