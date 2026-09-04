namespace Cubase.Macro.Mobile.Lyrics
{
    [Obsolete]
    public class MobileLyricCollection : List<MobileLyric>
    {
        public MobileLyricCollection() { }


        public int MaxBar()
        {
            return this.Max(x => x.Bar);
        }

        public MobileLyric? GetBar(int bar)
        {
            return this.Where(l => l.Bar <= bar)
                        .OrderByDescending(l => l.Bar)
                        .FirstOrDefault();

            // return this.FirstOrDefault(x => x.Bar == bar);
        }

        public int GetIndex(MobileLyric mobileLyric)
        {
            return this.IndexOf(mobileLyric);
        }

    }

    public class MobileLyric
    {
        public string Lyric { get; set; }

        public int Bar { get; set; } = -1;

        public Color ForegoundColour { get; set; }

    }
}
