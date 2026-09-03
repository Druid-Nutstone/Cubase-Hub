using Cubase.Macro.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Common.Lyrics.Services.Scrolling
{
    public interface IScrollerService
    {

        void StartDurationTimer(LyricContainer lyricContainer,
                          Action<int> onGotoBar,
                          Action<TimeSpan> onTransportUpdate,
                          int intervalMilliseconds = 50);

        void Stop();
    }
}
