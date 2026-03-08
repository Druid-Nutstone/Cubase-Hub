using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Cubase.Hub.Services.Models
{
    public class MixDownCollection : List<MixDown>
    {
        public MixDownCollection() 
        { 
        
        }

        public MixDownCollection(IEnumerable<MixDown> collection) : base(collection)
        {
        }

        public void SelectDeSelectMixes(bool state)
        {
            foreach (var item in this)
            {
                item.Selected = state;
            }
        }

        public IEnumerable<MixDown> ThatHaveMixDownAsTitle()
        {
            return this.Where(x => x.Title == CubaseHubConstants.MixdownDirectory);
        }

        public void SelectSelectedForDistribution(List<MixDown> selectedTracks)
        {
            var lookup = this.ToDictionary(x => x.FileName);

            foreach (var item in selectedTracks)
            {
                if (lookup.TryGetValue(item.FileName, out var track))
                {
                    track.MarkForDistribution = true;
                }
            }
        }

        public MixDownCollection OrderByTrack()
        {
            return new MixDownCollection(this.OrderBy(x => x.TrackNumber));
        }

        public MixDownCollection OrderByDate()
        {
            return new MixDownCollection(this.OrderBy(x => x.LastModified)); 
        }

        public MixDownCollection OrderByType()
        {
            return new MixDownCollection(this.OrderBy(x => x.AudioType));
        }

        public MixDownCollection OrderByDuration()
        {
            return new MixDownCollection(this.OrderBy(x => x.Duration));
        }

        public MixDownCollection OrderBySize()
        {
            return new MixDownCollection(this.OrderBy(x => x.Size));
        }

        public void SetMixdownExportLocation(string location)
        {
            foreach (var item in this)
            {
                item.ExportLocation = location;
            }
        }

        public MixDownCollection GetSelectedMixes()
        {
            return new MixDownCollection(this.Where(x => x.Selected));
        }

        public void RemoveSelectedMixes()
        {
            for (int i=0; i < this.Count; i++)
            {
                if (this[i].Selected)
                {
                    this.Remove(this[i]);
                }
            }    
        }

        public bool AreAnyMixesSelected()
        {
            return this.Where(x => x.Selected).Any();
        }

        public void CreateFromFiles(string[] files)
        {
            foreach(var file in files)
            {
                this.Add(MixDown.CreateFromFile(file));
            }
        }

        public static MixDownCollection CreateFromSingleMix(MixDown mixDown)
        {
            var tempCollection = new MixDownCollection();
            tempCollection.Add(mixDown);
            return tempCollection;
        }
    }

    public class MixDown : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        private bool _selected = false;
        public bool Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }

        private string _fileName;
        public string FileName
        {
            get => _fileName;
            set => SetProperty(ref _fileName, value);
        }

        private string _parentDirectory;
        public string ParentDirectory
        {
            get => _parentDirectory;
            set => SetProperty(ref _parentDirectory, value);
        }

        // ID tags


        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _album;
        public string Album
        {
            get => _album;
            set => SetProperty(ref _album, value);
        }

        private string _genre;
        public string Genre
        {
            get => _genre;
            set => SetProperty(ref _genre, value);
        }

        private string _duration;
        public string Duration
        {
            get => _duration;
            set => SetProperty(ref _duration, value);
        }

        private string _size;
        public string Size
        {
            get => _size;
            set => SetProperty(ref _size, value);
        }

        private uint _trackNumber;
        public uint TrackNumber
        {
            get => _trackNumber;
            set => SetProperty(ref _trackNumber, value);
        }

        private string _audioType;
        public string AudioType
        {
            get => _audioType;
            set => SetProperty(ref _audioType, value);
        }

        private uint _year;
        public uint Year
        {
            get => _year;
            set => SetProperty(ref _year, value);
        }

        private string _artist;
        public string Artist
        {
            get => _artist;
            set => SetProperty(ref _artist, value);
        }

        private string _performers;
        public string Performers
        {
            get => _performers;
            set => SetProperty(ref _performers, value);
        }

        private string _bitRate;
        public string BitRate
        {
            get => _bitRate;
            set => SetProperty(ref _bitRate, value);
        }

        private string _comment;
        public string Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
        }

        private bool _markForDistribution;
        public bool MarkForDistribution
        {
            get => _markForDistribution;
            set => SetProperty(ref _markForDistribution, value);
        }

        public string ExportLocation { get; set; }
        
        public DateTime LastModified { get; set; }

        public int SampleRate { get; set; }

        public string GetContentType()
        {
            switch (this._audioType.ToLower())
            {
                default:
                case "wav":
                    return "audio/wav";
                case "mp3":
                    return "audio/mpeg";
                case "flac":
                    return "audio/flac";
            }
        }

        public void UpdateFromAnotherMix(MixDown mix)
        {
            this._title = mix.Title;    
            this._album = mix.Album;
            this._artist = mix.Artist;
            this._audioType = mix.AudioType;
            this._bitRate = mix.BitRate;
            this._comment = mix.Comment;
            this._duration = mix.Duration;
            this._genre = mix.Genre;
            this._performers = mix.Performers;
            this._trackNumber = mix.TrackNumber;
            this.ExportLocation = mix.ExportLocation;
            this._markForDistribution = mix.MarkForDistribution;
        }

        public static MixDown CreateFromFile(string file)
        {
            return new MixDown
            {
                FileName = file,
                ParentDirectory = Path.GetFileName(Directory.GetParent(file).FullName)
            };
        }
    }
}
