using System;
using System.Collections.Generic;
using System.Text;
using NAudio.Wave;
using TagLib;
namespace Cubase.Hub.Services.Audio
{
    public class AudioService : IAudioService
    {
        public IWavePlayer? Player { get; private set; }
        public AudioFileReader? Audio { get; private set; } 

        public AudioService() { }
    
        public IWavePlayer Play(string musicfile, Action<StoppedEventArgs> onStopped)
        {
            Player?.Stop();
            Player?.Dispose();
            Player?.Dispose();
            Audio = new AudioFileReader(musicfile); // supports MP3, WAV, FLAC, AIFF
            Player = new WaveOutEvent();
            Player.PlaybackStopped += (o, e) => { onStopped?.Invoke(e); };
            Player.Init(Audio);
            Player.Play();
            return Player;
        }

        public void Stop()
        {
            Player?.Stop();
        }
    }
}
