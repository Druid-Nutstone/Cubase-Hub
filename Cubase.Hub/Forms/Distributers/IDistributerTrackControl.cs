using Cubase.Hub.Controls.CompletedMixes.Tracks;
using Cubase.Hub.Services.Models;

namespace Cubase.Hub.Forms.Distributers
{
    public interface IDistributerTrackControl
    {
        void SetMix(MixDown mixDown, TrackPlayViewControl trackPlayViewControl);
    }
}
