using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.Distributers
{
    public interface IDistributer
    {
        bool Distribute(MixDown mixDown, Action<string> onError);

    }
}
