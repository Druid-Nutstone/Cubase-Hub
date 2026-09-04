namespace Cubase.Macro.Mobile.Lyrics
{
    public class LyricLabel : Label
    {
        public LyricLabel() : base()
        {
            this.FontFamily = "CustomMono";
        }

        public LineType Type { get; set; }

        public int Bar { get; set; } = -1;
    }

    public enum LineType
    {
        Lyric = 0,
        Chord = 1,
        Header = 2,
        SectionHeader = 3,
        Comment = 4,
        EmptyLine = 5
    }
}
