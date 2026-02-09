using System;
using System.Collections.Generic;
using System.Text;
using Cubase.Hub.Services.Models;
using NAudio.Wave;
using TagLib;
using FFMpegCore;
using FFMpegCore.Pipes;
using FFMpegCore.Enums;
namespace Cubase.Hub.Services.Audio
{
    public class AudioService : IAudioService
    {
        public IWavePlayer? Player { get; private set; }
        public AudioFileReader? Audio { get; private set; } 

        public AudioService() 
        {
            GlobalFFOptions.Configure(options =>
            {
                options.BinaryFolder = Path.Combine(AppContext.BaseDirectory, "Encoder");
            });

        }
    
        public IWavePlayer Play(string musicfile, Action<StoppedEventArgs> onStopped)
        {
            var fileName = musicfile;
            if (musicfile.IsFlac())
            {
                fileName = this.ConvertFlacToWav(musicfile);
            }
            return internalPlayFile(fileName, onStopped);
        }

        private string ConvertFlacToWav(string flacfile)
        {
            FFMpegArguments
                .FromFileInput(flacfile)
                .OutputToFile(this.GetTempWavFile(), overwrite: true, options => options
                .WithAudioCodec("pcm_s16le"))
                .ProcessSynchronously();
            return this.GetTempWavFile();
        }

        private string GetTempWavFile() 
        {
            return Path.Combine(Path.GetTempPath(), "TempWav.Wav");
        } 

        private IWavePlayer internalPlayFile(string musicfile, Action<StoppedEventArgs> onStopped)
        {
            Player?.Stop();
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
            Player?.Dispose();
            Audio?.Dispose();
            if (System.IO.File.Exists(this.GetTempWavFile()))
            {
                System.IO.File.Delete(this.GetTempWavFile());   
            }
        }


        public void SetTagsFromMixDowm(MixDown mixDown, string? targetFile = null)
        {
            var fileTag = TagLib.File.Create(targetFile ?? mixDown.FileName);
            fileTag.Tag.Title = mixDown.Title;
            fileTag.Tag.Album = mixDown.Album;
            fileTag.Tag.Genres = [mixDown.Genre ?? string.Empty];
            fileTag.Tag.Track = mixDown.TrackNumber;
            fileTag.Tag.Year = mixDown.Year;
            fileTag.Tag.AlbumArtists = [mixDown.Artist ?? string.Empty];
            fileTag.Tag.Performers = mixDown.Performers.Split(";");
            fileTag.Tag.Comment = mixDown.Comment;
            fileTag.Save();
        }

        public void PopulateMixdownFromTags(MixDown mixDown)
        {
            var tags = TagLib.File.Create(mixDown.FileName);

            mixDown.Title = tags.Tag.Title ?? mixDown.ParentDirectory;
            mixDown.Album = tags.Tag.Album;
            mixDown.Genre = tags.Tag.FirstGenre;
            mixDown.Duration = tags.Properties.Duration.ToString(@"mm\:ss");
            var fileInfo = new FileInfo(mixDown.FileName);
            mixDown.Size = ConvertLengthToString(fileInfo.Length);
            mixDown.TrackNumber = tags.Tag.Track;
            mixDown.AudioType = tags.MimeType.Split("/").TakeLast(1).First();
            mixDown.Year = tags.Tag.Year;
            mixDown.Artist = tags.Tag.AlbumArtists?.FirstOrDefault()
                             ?? tags.Tag.Performers?.FirstOrDefault()
                             ?? string.Empty;
            mixDown.Performers = string.Join(";", tags.Tag.Performers ?? []);
            mixDown.Comment = tags.Tag.Comment;
        }

        private string ConvertLengthToString(long length)
        {
            const long KB = 1024;
            const long MB = KB * 1024;
            const long GB = MB * 1024;

            if (length < KB)
                return $"{length} Bytes";
            else if (length < MB)
                return $"{(length / (double)KB):0.##} KB";
            else if (length < GB)
                return $"{(length / (double)MB):0.##} MB";
            else
                return $"{(length / (double)GB):0.##} GB";
        }

        public MixDown PopulateTagsFromFile(string fileName)
        {
            var mixDown = new MixDown() { FileName = fileName };
            PopulateMixdownFromTags(mixDown);
            return mixDown;
        }

        public MixDownCollection PopulateMixDownCollectionFromTags(MixDownCollection mixes)
        {
            foreach (var mix in mixes)
            {
                PopulateMixdownFromTags(mix);
            }
            return new MixDownCollection(
                mixes.OrderBy(x => x.TrackNumber)
            );
        }

        public void ConvertToMp3(MixDown mixDown, string targetDirectory, AudioQuality quality)
        {
            var outFile = this.MapOutputAudioFile(mixDown.FileName, targetDirectory, ".mp3");


            FFMpegArguments
                 .FromFileInput(mixDown.FileName)
                 .OutputToFile(outFile, overwrite: true, options => options
                                   .WithAudioCodec("mp3")
                                   .WithAudioBitrate(quality))
                 .ProcessSynchronously();
            this.SetTagsFromMixDowm(mixDown, outFile);

        }

        public void ConvertToFlac(MixDown mixDown, string targetDirectory, CompressionLevel compressionLevel)
        {
            var outFile = this.MapOutputAudioFile(mixDown.FileName, targetDirectory, ".flac");
            FFMpegArguments
                 .FromFileInput(mixDown.FileName)
                 .OutputToFile(outFile, overwrite: true, options => options
                                   .WithAudioCodec("flac")
                                   .WithCustomArgument($"-compression_level {(int)compressionLevel}"))
                 .ProcessSynchronously();
            this.SetTagsFromMixDowm(mixDown, outFile);
        }

        private string MapOutputAudioFile(string inputFileName, string targetDirectory, string extention)
        {
            var newFileName = Path.GetFileNameWithoutExtension(inputFileName);
            return Path.Combine(targetDirectory, $"{newFileName}{extention}");
        }
    }
}
