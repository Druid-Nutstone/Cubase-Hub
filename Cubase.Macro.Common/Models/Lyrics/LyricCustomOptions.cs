using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Cubase.Macro.Common.Models.Lyrics
{
    public class LyricCustomOptions : INotifyPropertyChanged
    {
        private string? lyricAudioFile;
        private string? lyricAudioPath;
        private bool playAudioWithScroll = false;
        private int barOffset = 0;
        private bool showChords = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string? LyricAudioFile
        {
            get => lyricAudioFile;
            set
            {
                if (lyricAudioFile != value)
                {
                    lyricAudioFile = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? LyricAudioPath
        {
            get => lyricAudioPath;
            set
            {
                if (lyricAudioPath != value)
                {
                    lyricAudioPath = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool PlayAudioWithScroll
        {
            get => playAudioWithScroll;
            set
            {
                if (playAudioWithScroll != value)
                {
                    playAudioWithScroll = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool ShowChords
        {
            get => showChords;
            set
            {
                if (showChords != value)
                {
                    showChords = value;
                    OnPropertyChanged();
                }
            }
        }

        public int BarOffset
        {
            get => barOffset;
            set
            {
                if (barOffset != value)
                {
                    barOffset = value;
                    OnPropertyChanged();
                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
