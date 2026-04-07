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
                Interval = 100 // ms (adjust as needed)
            };

            mouseTimer.Tick += MouseTimer_Tick;
        }

        public void Initialise()
        {
            mouseTimer.Start();
        }

        private void MouseTimer_Tick(object sender, EventArgs e)
        {
            if (isShowing)
                return;

            if (!windowService.IsCubaseActive())
                return;

            Point cursor = Cursor.Position;
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

            mouseTimer.Stop();

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