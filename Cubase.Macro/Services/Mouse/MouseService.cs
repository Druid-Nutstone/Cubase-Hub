using Cubase.Macro.Services.Window;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Cubase.Macro.Services.Mouse
{
    public class MouseService : IMouseService
    {
        private readonly MainForm mainForm;
        private readonly ILogger<MouseService> log;
        private readonly IWindowService windowService;

        private readonly System.Windows.Forms.Timer mouseTimer;
        private bool isShowing;

        private DateTime lastInsideTime = DateTime.Now;
        private const int CloseMargin = 20; // pixels
        private const int CloseDelayMs = 300;

        public MouseService(
            MainForm mainForm,
            ILogger<MouseService> log,
            IWindowService windowService)
        {
            this.mainForm = mainForm;
            this.log = log;
            this.windowService = windowService;

            mouseTimer = new System.Windows.Forms.Timer
            {
                Interval = 500 // ms (adjust as needed)
            };

            mouseTimer.Tick += MouseTimer_Tick;
        }

        public void Initialise()
        {
            mouseTimer.Start();
        }

        private void MouseTimer_Tick(object sender, EventArgs e)
        {
            var cursor = Control.MousePosition;

            if (isShowing)
            {
                // Get proper screen bounds of the form
                var bounds = mainForm.RectangleToScreen(mainForm.ClientRectangle);

                // Add grace margin
                bounds.Inflate(CloseMargin, CloseMargin);

               
                if (bounds.Contains(cursor))
                {
                    // Cursor still near the form → reset timer
                    lastInsideTime = DateTime.Now;
                    return;
                }

                // Only close if cursor has been away long enough
                if ((DateTime.Now - lastInsideTime).TotalMilliseconds > CloseDelayMs)
                {
                    if (!mainForm.HaveError)
                    {
                        mainForm.CloseWindow();
                        isShowing = false;
                    }
                    return;
                }
                return;
            }
            if (!windowService.IsCubaseActive(false))
                return;

            // Point cursor = Cursor.Position;
            Rectangle screen = Screen.PrimaryScreen.Bounds;


            // Check far-left edge (with tolerance)
            if (cursor.X <= screen.Left + 1)
            {
                ShowForm();
            }
        }

        private void ShowForm()
        {
            isShowing = true;

            // mouseTimer.Stop();

            // Set callback BEFORE showing form
            mainForm.ActionComplete = () =>
            {
                isShowing = false;
                mouseTimer.Start();
            };

            mainForm.ShowMacros();
        }
    }
}