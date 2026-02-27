using Cubase.Hub.Services.Models;
using FFMpegCore.Enums;
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

        void FastForward(TimeSpan timeSpan);

        void FastBackward(TimeSpan timeSpan);

        void Stop();

        MixDownCollection AudioPopulateMixDownCollectionFromTags(MixDownCollection mixes);

        void AudioSetTagsFromMixDowm(MixDown mixDown, string? targetFile = null);

        void AudioConvertToMp3(MixDown mixDown, string targetDirectory, AudioQuality quality);

        void AudioConvertToFlac(MixDown mixDown, string targetDirectory, FlacConfiguration configuration);

        void AudioPopulateMixdownFromTags(MixDown mixDown);

        MixDown AudioPopulateTagsFromFile(string fileName);

    }
}
