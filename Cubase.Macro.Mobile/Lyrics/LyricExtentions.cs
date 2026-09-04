namespace Cubase.Macro.Mobile.Lyrics
{
    public static class LyricExtentions
    {
        public static string LyricFullPath(this string fileName)
        {
            return Path.Combine(CubaseMacroMobileConstants.LyricSourceFolder, fileName);
        }
    }
}
