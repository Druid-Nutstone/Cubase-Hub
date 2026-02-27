using Cubase.Hub.Services.Models;
using FFMpegCore.Enums;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.Track
{
    public interface ITrackService
    {
        IWavePlayer? Player { get; }
        AudioFileReader? Audio { get; }

        void StopTrack();

        IWavePlayer PlayTrack(MixDown mixDown, Action<StoppedEventArgs> onStopped);

        IWavePlayer PlayTrack(string musicfile, Action<StoppedEventArgs> onStopped);

        MixDownCollection PopulateMixDownCollectionFromTags(MixDownCollection mixes);

        void SetTagsFromMixDowm(MixDown mixDown, string? targetFile = null);

        void ConvertToMp3(MixDown mixDown, string targetDirectory, AudioQuality quality);

        void ConvertToFlac(MixDown mixDown, string targetDirectory, FlacConfiguration configuration);

        void SetTagsFromMixDown(MixDown mixDown, string? fileName = null);

        void PopulateMixdownFromTags(MixDown mixDown);

        MixDown PopulateTagsFromFile(string fileName);

        MixDownCollection GetMixesForAlbum(AlbumLocation albumLocation);

    }
}
