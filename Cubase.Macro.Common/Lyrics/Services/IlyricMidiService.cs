using Cubase.Macro.Common.Models;

namespace Cubase.Macro.Common.Lyrics.Services
{
    public interface IlyricMidiService
    {
        TransportLocationCollection GetTransportLocation();

        bool IsMidiAvailable();
    }
}
