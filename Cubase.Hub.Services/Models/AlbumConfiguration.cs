using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cubase.Hub.Services.Models
{
    public class AlbumConfiguration : INotifyPropertyChanged
    {
        private string _title;
        private string _artist;
        private uint _year = (uint)DateTime.Now.Year;
        private string _genre = "Unknown";
        private string _comments;

        private string _sourceLocation = null;

        [JsonIgnore]
        public Action<MixDown>? DistributionChanged { get; set; }

        public void AddForDistribution(MixDown mixDown)
        {
            if (!this.DistributionMixes.Any(x => x.FileName == mixDown.FileName))
            {
                this.DistributionMixes.Add(mixDown);
            }
            else
            {
                this.DistributionMixes[this.DistributionMixes.FindIndex(x => x.FileName == mixDown.FileName)] = mixDown;
            }
            this.SaveToDirectory(this._sourceLocation);
            DistributionChanged?.Invoke(mixDown);
        }

        public MixDown? FindMixDown(MixDown mixDown)
        {
            return this.DistributionMixes.FirstOrDefault(x => x.FileName == mixDown.FileName); 
        } 

        public void UpdateMixDistribution(MixDown mixDown)
        {
            if (this.DistributionMixes.Any(_ => _.FileName == mixDown.FileName && mixDown.MarkForDistribution)) 
            {
                this.AddForDistribution(mixDown);   
            }   
        }

        public void CheckForUpdatedDistributionMixes(List<MixDown> mixDowns)
        {
            foreach (var item in this.DistributionMixes)
            {
                var newMixDown = mixDowns.FirstOrDefault(x => x.FileName == item.FileName);
                if (newMixDown != null)
                {
                    if (newMixDown.LastModified != item.LastModified)
                    {
                        DistributionChanged?.Invoke(newMixDown);
                    }
                }
            }
        }

        public void RemoveFromDistribution(MixDown mixDown) 
        {
            if (this.DistributionMixes.Any(x => x.FileName == mixDown.FileName))
            {
                this.DistributionMixes.RemoveAt(this.DistributionMixes.FindIndex(x => x.FileName == mixDown.FileName));
                this.SaveToDirectory(this._sourceLocation);
            }
        } 

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Artist
        {
            get => _artist;
            set => SetProperty(ref _artist, value);
        }

        public uint Year
        {
            get => _year;
            set => SetProperty(ref _year, value);
        }

        public string Genre
        {
            get => _genre;
            set => SetProperty(ref _genre, value);
        }

        public string Comments
        {
            get => _comments;
            set => SetProperty(ref _comments, value);
        }

        public bool Verify(out string propertyInError, bool verifyTitle = true)
        {
            if (verifyTitle)
            {
                if (string.IsNullOrWhiteSpace(Title))
                {
                    propertyInError = nameof(Title);
                    return false;
                }
            }
            if (string.IsNullOrWhiteSpace(Artist))
            {
                propertyInError = nameof(Artist);
                return false;
            }
            if (Year == 0)
            {
                propertyInError = nameof(Year);
                return false;
            }

            propertyInError = string.Empty;
            return true;
        }

        public string? AlbumArt { get; set; }

        public AlbumConfiguration WithAlbumArt(string albumArtLocation)
        {
            AlbumArt = albumArtLocation;
            return this;
        }

        public bool SaveToDirectory(string directoryPath)
        {
            try
            {
                var targetPath = Path.Combine(directoryPath, CubaseHubConstants.CubaseAlbumConfigurationFileName);
                File.WriteAllText(targetPath, JsonSerializer.Serialize(this, new JsonSerializerOptions() { WriteIndented = true }));
                return true;
            }
            catch (Exception e) 
            {
                return false;
            }
        }

        public static AlbumConfiguration LoadFromFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                var albumConfig = JsonSerializer.Deserialize<AlbumConfiguration>(File.ReadAllText(fileName));
                albumConfig._sourceLocation = Path.GetDirectoryName(fileName);
                return albumConfig;
            }
            return null;
        }

        public List<MixDown> DistributionMixes { get; set; } = new List<MixDown>(); 
    }
}
