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

        MixDown PopulateTagsFromFile(string fileName);

        void PopulateMixdownFromTags(MixDown mixDown);

        MixDownCollection PopulateMixDownCollectionFromTags(MixDownCollection mixes);

        void SetTagsFromMixDowm(MixDown mixDown, string? targetFile = null);

        void ConvertToMp3(MixDown mixDown, string targetDirectory, AudioQuality quality);

        void ConvertToFlac(MixDown mixDown, string targetDirectory, CompressionLevel compressionLevel);
    }
}
