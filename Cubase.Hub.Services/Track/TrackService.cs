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

        public MixDownCollection GetFinalMixesForAlbum(AlbumLocation albumLocation)
        {
            var finalMixes = this.configurationService.GetFinalMixLocationFromAlbumName(albumLocation.AlbumName);
            if (finalMixes != null)
            {
                List<MixDown> mixes = new List<MixDown>();
                // loop through any versions in quality order (wav is best then flac then mp3 
                // so any wav's will replace others 
                foreach (var validExt in CubaseHubConstants.ValidAudioExtensions)
                {
                    var audioFiles = Directory.GetFiles(finalMixes, $"*{validExt}");
                    foreach (var audioFile in audioFiles)
                    {
                        var mixdown = this.audioService.AudioPopulateTagsFromFile(audioFile);
                        // replace any existing mixes 
                        var existingTitle = mixes.FirstOrDefault(x => x.Title == mixdown.Title);
                        if (existingTitle != null)
                        {
                            mixes.Remove(existingTitle);
                        } 
                        mixes.Add(mixdown);
                    }
                }
                return new MixDownCollection(mixes.OrderBy(x => x.TrackNumber));
            }
            else
            {
                return new MixDownCollection();
            }
        }

        public string? GetTrackCoverArt(MixDown mixDown)
        {
            var root = this.configurationService.GetFinalMixLocationFromAlbumName(mixDown.Album);
            if (string.IsNullOrEmpty(root))
            {
                return null;
            }

            var trackArtLocation = Path.Combine(root, CubaseHubConstants.TrackArt);

            var allTrackArt = Directory.GetFiles(trackArtLocation, $"{mixDown.Title}.*");
             
            if (allTrackArt.Length == 0)
            {
                return null; 
            }

            return allTrackArt.First();
        }

        public bool CopyTrackArt(MixDown mixDown, string targetart)
        {
            var root = this.configurationService.GetFinalMixLocationFromAlbumName(mixDown.Album);
            if (string.IsNullOrEmpty(root))
            {
                return false;
            }

            var trackArtLocation = Path.Combine(root, CubaseHubConstants.TrackArt);

            var existingArt = Directory.GetFiles(trackArtLocation, $"{mixDown.Title}.*");
            foreach ( var delFile in existingArt )
            {
                File.Delete(delFile); 
            }

            File.Copy(targetart, Path.Combine(trackArtLocation, $"{ mixDown.Title}{Path.GetExtension(targetart)}"), true);

            return true;
        }
    }
}
