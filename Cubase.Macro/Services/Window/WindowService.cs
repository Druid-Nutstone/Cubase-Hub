using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Cubase.Macro.Services.Window
{
    public class WindowService : IWindowService
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        public bool IsCubaseActive()
        {
            IntPtr hwnd = GetForegroundWindow();

            if (hwnd == IntPtr.Zero)
                return false;

            GetWindowThreadProcessId(hwnd, out uint pid);

            try
            {
                var process = Process.GetProcessById((int)pid);

                // Debug.WriteLine($"Active process: {process.ProcessName} (PID: {pid})");

                // Check process name (no .exe)
                return process.ProcessName.StartsWith("Cubase", StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex);
                return false;
            }
        }
    }
}
