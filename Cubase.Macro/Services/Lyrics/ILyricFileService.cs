using Cubase.Macro.Common.Models;

namespace Cubase.Macro.Services.Lyrics
{
    public interface ILyricFileService
    {
        LyricResponseModel GetProjectCurrentLyrics();

        LyricResponseModel GetLyric(Lyric fileName);

        LyricIndexCollection GetLyricIndex();

        LyricContent GetLyricContent(Lyric fileName);

    }
}
