using System;
using System.Collections.Generic;
using System.Text;
using Cubase.Hub.Services.Models;
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


        public void SetTagsFromMixDowm(MixDown mixDown)
        {
            var fileTag = TagLib.File.Create(mixDown.FileName);
            fileTag.Tag.Title = mixDown.Title;
            fileTag.Tag.Album = mixDown.Album;
            fileTag.Tag.Genres = [mixDown.Genre];
            fileTag.Tag.Track = mixDown.TrackNumber;
            fileTag.Tag.Year = mixDown.Year;
            fileTag.Tag.AlbumArtists = mixDown.Performers.Split(";");
            fileTag.Tag.Performers = [mixDown.Artist]; 
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
            mixDown.AudioType = Path.GetExtension(mixDown.FileName)?.Replace('.', ' ').Trim();
            mixDown.Year = tags.Tag.Year;
            mixDown.Artist = tags.Tag.FirstPerformerSort;
            mixDown.Performers = string.Join(";", tags.Tag.AlbumArtists);
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

        public void PopulateMixDownCollectionFromTags(MixDownCollection mixes)
        {
            foreach (var mix in mixes)
            {
                PopulateMixdownFromTags(mix);
            }
        }

    }
}
