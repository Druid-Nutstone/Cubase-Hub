using Cubase.Macro.Services.Window;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Services.Monitor
{
    [Obsolete("This class is no longer used and will be removed in a future version.")]
    public class MonitorService : IMonitorService
    {
        private readonly IWindowService windowService;

        private int FormLeft;

        private bool RunningTrue = true;
        
        private readonly ILogger<MonitorService> logger;    
        
        public MonitorService(IWindowService windowService, ILogger<MonitorService> logger)
        {
            this.windowService = windowService;
            this.logger = logger;
        }

        public void Stop()
        {
            this.RunningTrue = false;
        }

        public void PositionCubase(int width)
        {
            this.FormLeft = width;
            int threadSleepTimeWhenCubaseIsRunning = 10000; 
            int threadlSleepTimeWhenCubaseIsNotRunning = 1000;
            int threadSleepTime = threadlSleepTimeWhenCubaseIsNotRunning;
            Task.Run(() =>
            {
                while (RunningTrue)
                {
                    try
                    {
                        if (this.windowService.IsCubaseActive(false))
                        {
                            threadSleepTime = threadSleepTimeWhenCubaseIsRunning;
                            SetCubasePosition();
                        }
                        else
                        {
                            threadSleepTime = threadlSleepTimeWhenCubaseIsNotRunning;
                        }

                    }
                    catch (Exception ex)
                    {
                        this.logger.LogError(ex, "Error positioning Cubase");
                    }
                    Thread.Sleep(threadSleepTime);
                }
                
                void SetCubasePosition()
                {

                        //var hWnd = this.windowService.GetCubaseHandle();
                        //if (hWnd == IntPtr.Zero)
                        //    return;

                        // GetWindowRect(hWnd, out var rect);

                        var rect = this.windowService.GetCubaseBounds();

                        // small tolerance avoids endless repositioning due to tiny diffs
                        const int tolerance = 2;

                        if (Math.Abs(rect.Left - FormLeft) > tolerance)
                        {
                            this.logger.LogInformation("Positioning Cubase to left {FormLeft}", FormLeft);
                            this.windowService.PositionCubase(FormLeft);
                        }
                }
            });
        }


    }
}
