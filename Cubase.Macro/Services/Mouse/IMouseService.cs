using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Services.Mouse
{
    [Obsolete("This screws up the pointer..")]
    public interface IMouseService
    {
        void Initialise();
        void Dispose();
    }
}
