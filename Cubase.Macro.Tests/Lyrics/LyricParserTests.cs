using Cubase.Macro.Common.Lyrics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Tests.Lyrics
{
    [TestClass]
    public class LyricParserTests
    {
        private string testFile = @"C:\DeleteMe\angie baby.txt";

        private LyricChordParser sut = new LyricChordParser();

        [TestMethod]
        public void Can_Parse_Lyrics()
        {
            var lyricCollection = sut.Parse(testFile);
            var output = lyricCollection?.RenderToText();
            File.WriteAllLines("C:\\Deleteme\\lyricparsertest.txt", output);
        }
    
    }
}
