using Cubase.Macro.Common.Models.Lyrics;

namespace Cubase.Macro.Tests.Lyrics
{
    [TestClass]
    public class LyricSectionTests
    {
        [TestMethod]
        public void Can_Create_LyricSection()
        {
            var lyricSection = LyricSection.Create("Verse 1", 1, "Text comments");
            var lyrics = new List<string>()
            {
                "this is [c#maj]line 1 of the lyric",
                "this [Amaj]is line 2 of the lyric"
            };
            lyricSection.AddLyrics(lyrics);
            var visualElements = lyricSection.GetLyricsAndChords();
            foreach (var lyricLine in visualElements)
            {
                foreach (var segment in lyricLine)
                {
                }
            }

        }

        [TestMethod]
        public void Can_Save_And_load_LyricContainer()
        {
            var lyrics = new LyricContainer()
            {
                Album = "Test Album",
                Bpm = 120,
                Duration = TimeSpan.FromMinutes(3),
                Title = "Song Title",
                TimeSignature = 3.4,
            };
            var lyricSection = LyricSection.Create("Verse 1", 1, "Text comments");
            var lyricText = new List<string>()
            {
                "this is [c#maj]line 1 of the lyric",
                "this [Amaj]is line 2 of the lyric"
            };
            lyricSection.AddLyrics(lyricText);
            lyrics.Sections.Add(lyricSection);
            var savedFilePath = lyrics.Save("C:\\deleteme\\", (error) => { Assert.Fail(error); });

            if (savedFilePath != null)
            {
                var loadedLyrics = LyricContainer.Load(savedFilePath, (error) => { Assert.Fail(error); });
                Assert.IsNotNull(loadedLyrics);
                Assert.AreEqual(lyrics.Title, loadedLyrics.Title);
                Assert.AreEqual(lyrics.Album, loadedLyrics.Album);
                Assert.AreEqual(lyrics.Bpm, loadedLyrics.Bpm);
                Assert.AreEqual(lyrics.Duration, loadedLyrics.Duration);
                Assert.AreEqual(lyrics.TimeSignature, loadedLyrics.TimeSignature);
                Assert.AreEqual(lyrics.Sections.Count, loadedLyrics.Sections.Count);
            }
            else
            {
                Assert.Fail("Failed to save the lyric container.");
            }

        }
    }
}
