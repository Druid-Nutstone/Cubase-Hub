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
