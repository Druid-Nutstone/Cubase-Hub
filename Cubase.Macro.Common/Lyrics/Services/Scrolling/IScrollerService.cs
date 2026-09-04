using Cubase.Macro.Common.Models;

namespace Cubase.Macro.Common.Lyrics.Services.Scrolling
{
    public interface IScrollerService
    {

        void StartDurationTimer(LyricContainer lyricContainer,
                          Action<int> onGotoBar,
                          Action<TimeSpan> onTransportUpdate,
                          int intervalMilliseconds = 50);

        Task StartMidiTimer(LyricContainer lyricContainer,
                  Action<int> onGotoBar,
                  Action<TimeSpan> onTransportUpdate,
                  int intervalMilliseconds = 50);

        void Stop();
    }
}
