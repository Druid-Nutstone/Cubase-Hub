using Cubase.Macro.Common.Models.Lyrics;

namespace Cubase.Macro.Mobile.Services.Lyrics
{
    public interface ILyricService
    {
        LyricContainer CurrentLyric { get; set; }

        Task CheckForFileUpdates(Action<string> onError);

        Task<string?> LoadProjectLyricIfAvailable(string projectName);

        Task StartAudio(LyricContainer lyricContainer);

        Task StopAudio();

        Task<List<string>> GetLyricFiles();

        Task<List<string>> GetSetlists();

        Task<FileResult?> PickAudioFileAsync();

        Task<LyricContainer> LoadSetListLyric(SetListSong setListSong);

        void SaveLyric(LyricContainer lyricContainer);
    }
}
