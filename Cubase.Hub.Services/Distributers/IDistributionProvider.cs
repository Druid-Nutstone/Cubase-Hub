using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.Distributers
{
    public interface IDistributionProvider
    {
        void Initialise();

        MixDownCollection? GetTracks(Action<string> onError);
    
    }
}
