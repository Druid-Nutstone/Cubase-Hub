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

        private string _comment;
        public string Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
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
