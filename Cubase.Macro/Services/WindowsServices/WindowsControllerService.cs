using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceProcess;

namespace Cubase.Macro.Services.WindowsServices
{
    public class WindowsControllerService : IWindowsControllerService
    {
        public void StopMidiWindowsService()
        {
            this.StopService("midisrv"); // service name
        }

        public void StartMidiWindowsService()
        {
            this.StartService("midisrv"); // service name
        }

        private void StopService(string serviceName)
        {
            ServiceController sc = new ServiceController(serviceName);
            if (sc.Status == ServiceControllerStatus.Running)
            {
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
            }
        }

        private void StartService(string serviceName)
        {
            ServiceController sc = new ServiceController(serviceName);
            if (sc.Status == ServiceControllerStatus.Stopped)
            {
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
            }
        }
    }
}
