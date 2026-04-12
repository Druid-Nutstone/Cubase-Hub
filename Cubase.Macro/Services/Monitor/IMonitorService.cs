using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Services.Monitor
{
    [Obsolete("This class is no longer used and will be removed in a future version.")]
    public interface IMonitorService
    {
        public void PositionCubase(int width);

        public void Stop();
    }
}
