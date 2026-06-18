using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Common.Lyrics
{
    public class LyricBuffer : List<string>
    {
        public void AddBlank(int count = 1)
        {
            for (int i=0; i < count; i++)
            {
                this.Add(string.Empty);
            }
        }

        public string ToText()
        {
            return string.Join(Environment.NewLine, this.ToArray());
        }
    }
}
