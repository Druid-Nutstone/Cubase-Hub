using Cubase.Macro.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Services.Lyrics
{
    public interface ILyricFileService
    {
        LyricResponseModel GetProjectCurrentLyrics();
    }
}
