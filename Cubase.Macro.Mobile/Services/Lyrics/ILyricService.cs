using Cubase.Macro.Common.Models.Lyrics;

namespace Cubase.Macro.Mobile.Services.Lyrics
{
    public interface ILyricService
    {
        Task CheckForFileUpdates(Action<string> onError);

        Task<string?> LoadProjectLyricIfAvailable(string projectName);

        Task<List<string>> GetLyricFiles();

        Task<List<string>> GetSetlists();

        Task<LyricContainer> LoadSetListLyric(SetListSong setListSong);
    }
}
