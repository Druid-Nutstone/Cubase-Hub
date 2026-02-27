using Cubase.Hub.Services.Audio;
using Cubase.Hub.Services.Config;
using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.Models;
using FFMpegCore.Enums;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.Track
{
    public class TrackService : ITrackService
    {
        private readonly IAudioService audioService;

        private readonly IConfigurationService configurationService;

        private readonly IDirectoryService directoryService;

        private IWavePlayer player;

        private AudioFileReader audio;

        public IWavePlayer? Player => this.player;

        public AudioFileReader? Audio => this.audio;

        public TrackService(IConfigurationService configurationService,
                            IDirectoryService directoryService,  
                            IAudioService audioService) 
        { 
            this.audioService = audioService;
            this.directoryService = directoryService;
            this.configurationService = configurationService;
            this.player = this.audioService.Player;
            this.audio = this.audioService.Audio;
        }

        public void StopTrack()
        {
            this.audioService.Stop();
        }

        public IWavePlayer PlayTrack(MixDown mixDown, Action<StoppedEventArgs> onStopped)
        {
            return this.PlayTrack(mixDown.FileName, onStopped);
        }

        public IWavePlayer PlayTrack(string musicfile, Action<StoppedEventArgs> onStopped)
        {
            var waveplayer = this.audioService.Play(musicfile, onStopped);
            this.player = this.audioService.Player;
            this.audio = this.audioService.Audio;
            return waveplayer;
        }

        public void SetTagsFromMixDown(MixDown mixDown, string? fileName = null)
        {
            this.audioService.AudioSetTagsFromMixDowm(mixDown, fileName);
        }

        public void PopulateMixdownFromTags(MixDown mixDown)
        {
            this.audioService.AudioPopulateMixdownFromTags(mixDown);
        }

        public MixDownCollection PopulateMixDownCollectionFromTags(MixDownCollection mixes)
        {
            return this.audioService.AudioPopulateMixDownCollectionFromTags(mixes);
        }

        public void SetTagsFromMixDowm(MixDown mixDown, string? targetFile = null)
        {
            this.audioService.AudioSetTagsFromMixDowm(mixDown, targetFile);
        }

        public void ConvertToMp3(MixDown mixDown, string targetDirectory, AudioQuality quality)
        {
            this.audioService.AudioConvertToMp3(mixDown, targetDirectory, quality);
        }

        public void ConvertToFlac(MixDown mixDown, string targetDirectory, FlacConfiguration configuration)
        {
            this.audioService.AudioConvertToFlac(mixDown, targetDirectory, configuration);
        }

        public MixDown PopulateTagsFromFile(string fileName)
        {
            return this.audioService.AudioPopulateTagsFromFile(fileName);
        }

        public MixDownCollection GetMixesForAlbum(AlbumLocation albumLocation)
        {
            var mixes = this.directoryService.GetMixes(albumLocation.AlbumPath);
            return this.PopulateMixDownCollectionFromTags(mixes);
        }
    }
}
