using Cubase.Hub.Controls.CompletedMixes.Tracks;
using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Forms.Distributers
{
    public interface IDistributerTrackControl
    {
        void SetMix(MixDown mixDown, TrackPlayViewControl trackPlayViewControl);
    }
}
