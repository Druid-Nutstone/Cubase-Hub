using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Services.WindowsServices
{
    public interface IWindowsControllerService
    {

        void StopMidiWindowsService();

        void StartMidiWindowsService();  

    }
}
