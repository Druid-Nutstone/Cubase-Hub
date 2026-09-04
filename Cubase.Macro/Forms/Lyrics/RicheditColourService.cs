using Cubase.Macro.Common.Lyrics.Scrolling;

namespace Cubase.Macro.Forms.Lyrics
{
    public class RicheditColourService : IColourService
    {
        public object GetChordsColour()
        {
            return Color.IndianRed;
        }

        public object GetDefaultColour()
        {
            return DarkTheme.TextColor;
        }

        public object GetSectionColour()
        {
            return Color.Yellow;
        }

        public object GetTitleColour()
        {
            return Color.Cyan;
        }
    }
}
