using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.Audio
{
    public interface IAudioService
    {
        IWavePlayer? Player { get;  }

        AudioFileReader? Audio { get; }

        IWavePlayer Play(string musicfile, Action<StoppedEventArgs> onStopped);

        void Stop();
    }
}
