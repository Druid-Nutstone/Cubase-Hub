using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.Audio
{
    public static class AudioExtentions
    {
        public static bool IsFlac(this string fileName)
        {
            return Path.GetExtension(fileName).ToLower() == ".flac";
        }
    }
}
