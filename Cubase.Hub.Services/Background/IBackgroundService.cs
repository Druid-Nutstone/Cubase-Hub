using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.Background
{
    public interface IBackgroundService
    {
        void Start();

        void Pause();

        void Stop();

        void Resume(); 
    }
}
 