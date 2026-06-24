using Cubase.Macro.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Common.Lyrics.Services
{
    public interface IlyricMidiService
    {
        TransportLocationCollection GetTransportLocation();

        bool IsMidiAvailable();
    }
}
