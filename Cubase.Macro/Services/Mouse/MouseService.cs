using Cubase.Macro.Services.Window;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Cubase.Macro.Services.Mouse
{
    public class MouseService : IMouseService, IDisposable
    {
        private readonly MainForm mainForm;
        private readonly ILogger<MouseService> log;
        private readonly IWindowService windowService;

        private IntPtr hookId = IntPtr.Zero;
        private NativeMouse.LowLevelMouseProc hookCallback;

        private bool isShowing;

        // ----------------------------
        // TUNING
        // ----------------------------
        private const int EdgeThreshold = 8;        // enter zone
        private const int EdgeExitThreshold = 20;   // exit zone (hysteresis)

        private const int DwellTimeMs = 150;
        private const int TriggerCooldownMs = 1000;

        private DateTime edgeEnterTime = DateTime.MinValue;
        private DateTime lastTriggerTime = DateTime.MinValue;

        private bool inEdgeZone = false;
        private bool wasInEdgeZone = false;

        private Point? lastCursor = null;

        private DateTime lastInsideTime = DateTime.Now;
        private const int ExitDelayMs = 150;

        public MouseService(
            MainForm mainForm,
            ILogger<MouseService> log,
            IWindowService windowService)
        {
            this.mainForm = mainForm;
            this.log = log;
            this.windowService = windowService;
        }

        public void Initialise()
        {
            hookCallback = HookCallback;

            using (var process = Process.GetCurrentProcess())
            using (var module = process.MainModule)
            {
                hookId = NativeMouse.SetWindowsHookEx(
                    NativeMouse.WH_MOUSE_LL,
                    hookCallback,
                    NativeMouse.GetModuleHandle(module.ModuleName),
                    0);
            }

            log.LogInformation("Mouse hook initialised");
        }

        public void Dispose()
        {
            if (hookId != IntPtr.Zero)
            {
                NativeMouse.UnhookWindowsHookEx(hookId);
                hookId = IntPtr.Zero;
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (nCode >= 0 && wParam == (IntPtr)NativeMouse.WM_MOUSEMOVE)
                {
                    var data = Marshal.PtrToStructure<NativeMouse.MSLLHOOKSTRUCT>(lParam);
                    HandleMouseMove(new Point(data.pt.x, data.pt.y));
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Mouse hook error");
            }

            return NativeMouse.CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        private void HandleMouseMove(Point cursor)
        {
            // UI thread marshal
            if (mainForm.IsHandleCreated && mainForm.InvokeRequired)
            {
                mainForm.BeginInvoke(new Action(() => HandleMouseMove(cursor)));
                return;
            }


            if (isShowing)
            {
                if ((DateTime.Now - lastTriggerTime).TotalMilliseconds < 200)
                {
                    lastCursor = cursor;
                    return;
                }

                if (!insideForm())
                {
                    if ((DateTime.Now - lastInsideTime).TotalMilliseconds > ExitDelayMs)
                    {
                        // log.LogInformation("Mouse left MainForm → closing");

                        mainForm.CloseWindow(); // or Hide()
                    }
                }
                else
                {
                    lastInsideTime = DateTime.Now;
                }

                lastCursor = cursor;
                return;
            }

            if (isShowing)
            {
                lastCursor = cursor;
                return;
            }

            if (!windowService.IsCubaseActive(false))
            {
                lastCursor = cursor;
                return;
            }

            var cubaseBounds = windowService.GetCubaseBounds();

            if (cubaseBounds == Rectangle.Empty)
            {
                lastCursor = cursor;
                return;
            }

            // ----------------------------
            // STABLE EDGE ZONE (HYSTERESIS)
            // ----------------------------

            bool enterZone =
                cursor.X <= cubaseBounds.Left + EdgeThreshold &&
                cursor.Y >= cubaseBounds.Top &&
                cursor.Y <= cubaseBounds.Bottom;

            bool exitZone =
                cursor.X > cubaseBounds.Left + EdgeExitThreshold;

            wasInEdgeZone = inEdgeZone;

            if (enterZone)
            {
                inEdgeZone = true;
            }
            else if (exitZone)
            {
                inEdgeZone = false;
            }
            // else: hold state (IMPORTANT)

            //log.LogInformation(
            //    $"Mouse {cursor} | InZone={inEdgeZone} | WasInZone={wasInEdgeZone}");

            // ----------------------------
            // EDGE LOGIC
            // ----------------------------

            if (inEdgeZone)
            {
                // ALWAYS ensure timer starts correctly
                if (edgeEnterTime == DateTime.MinValue)
                {
                    edgeEnterTime = DateTime.Now;
                    // log.LogInformation($"Edge ENTER: {edgeEnterTime:HH:mm:ss.fff}");
                }

                var dwellTime = (DateTime.Now - edgeEnterTime).TotalMilliseconds;

                // log.LogInformation($"Dwell: {dwellTime}ms");

                if (dwellTime < DwellTimeMs)
                {
                    lastCursor = cursor;
                    return;
                }

                if ((DateTime.Now - lastTriggerTime).TotalMilliseconds < TriggerCooldownMs)
                {
                    lastCursor = cursor;
                    return;
                }

                lastTriggerTime = DateTime.Now;

                // log.LogInformation("Triggering form show");
                ShowForm();
            }
            else
            {
                // reset ONLY when truly out of zone
                edgeEnterTime = DateTime.MinValue;
            }

            lastCursor = cursor;

            bool insideForm()
            {
                return cursor.X >= mainForm.Bounds.Left &&
                cursor.X <= mainForm.Bounds.Right &&
                cursor.Y >= mainForm.Bounds.Top &&
                cursor.Y <= mainForm.Bounds.Bottom;
            }
           
        }

        private void ShowForm()
        {
            if (isShowing)
                return;

            isShowing = true;

            lastInsideTime = DateTime.Now;

            mainForm.ActionComplete = () =>
            {
                isShowing = false;
                inEdgeZone = false;
                wasInEdgeZone = false;
                edgeEnterTime = DateTime.MinValue;
            };

            mainForm.ShowMacros();
        }
    }
}