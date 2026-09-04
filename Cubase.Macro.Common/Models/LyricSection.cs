using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Cubase.Macro.Common.Models
{
    public class LyricSection
    {
        private static readonly Regex ChordRegex = new Regex(@"\[(.*?)\]", RegexOptions.Compiled);

        public string Name { get; set; }
        public string Comments { get; set; }
        public int Bar { get; set; } = -1;

        public int FontSize { get; set; } = 12;

        // Keeps track of global character indexes relative to the entire multi-line block
        public List<ChordSection> Chords { get; set; } = new List<ChordSection>();

        [JsonIgnore]
        public string Lyrics { get; set; }

        public string EncodedLyrics
        {
            get => Convert.ToBase64String(Encoding.UTF8.GetBytes(Lyrics ?? ""));
            set => Lyrics = Encoding.UTF8.GetString(Convert.FromBase64String(value ?? ""));
        }

        /// <summary>
        /// Safely processes a single raw input line containing embedded chords like "[C]hello"
        /// Strips chords, updates local Chords list using correct absolute string positioning,
        /// and appends the pristine text cleanly.
        /// </summary>
        public void AddLyric(string rawLyricLine)
        {
            if (rawLyricLine == null) return;

            // Calculate our global offset starting point before appending this line
            int currentGlobalOffset = string.IsNullOrEmpty(Lyrics) ? 0 : Lyrics.Length + Environment.NewLine.Length;

            var cleanLineBuilder = new StringBuilder();
            int lastIndex = 0;

            foreach (Match match in ChordRegex.Matches(rawLyricLine))
            {
                // Pull out the pure text before the bracketed chord
                string leadingText = rawLyricLine.Substring(lastIndex, match.Index - lastIndex);
                cleanLineBuilder.Append(leadingText);

                // Absolute position = historical block size + progress made inside current clean line
                int chordPosition = currentGlobalOffset + cleanLineBuilder.Length;
                string chordName = match.Groups[1].Value;

                Chords.Add(new ChordSection
                {
                    Chord = chordName,
                    CharacterIndex = chordPosition
                });

                lastIndex = match.Index + match.Length;
            }

            if (lastIndex < rawLyricLine.Length)
            {
                cleanLineBuilder.Append(rawLyricLine.Substring(lastIndex));
            }

            // Append pristine line back into main state container
            if (string.IsNullOrWhiteSpace(Lyrics))
            {
                Lyrics = cleanLineBuilder.ToString();
            }
            else
            {
                Lyrics += Environment.NewLine + cleanLineBuilder.ToString();
            }
        }

        public void AddLyrics(IEnumerable<string> rawLyricsLines)
        {
            if (rawLyricsLines == null) return;
            this.Lyrics = null; // Reset existing lyrics to avoid duplication
            this.Chords.Clear(); // Reset existing chords to avoid duplication
            foreach (var line in rawLyricsLines)
            {
                AddLyric(line);
            }
        }

        /// <summary>
        /// Re-inserts chords wrapped in square brackets back into their exact line text positions.
        /// Useful for saving or serializing back to your hybrid ChordPro file format.
        /// </summary>
        public string[] GetLinesWithChords()
        {
            if (string.IsNullOrEmpty(Lyrics)) return Array.Empty<string>();

            string[] cleanLines = GetRootLines();
            var resultLines = new List<string>();
            int runningGlobalCharCount = 0;

            foreach (var line in cleanLines)
            {
                int lineStartGlobalIndex = runningGlobalCharCount;
                int lineEndGlobalIndex = lineStartGlobalIndex + line.Length;

                // Fetch chords belonging to this line, ordered from right to left (backwards)
                // Processing backwards allows us to insert text tags without breaking subsequent indices!
                var lineChords = Chords
                    .Where(c => c.CharacterIndex >= lineStartGlobalIndex && c.CharacterIndex <= lineEndGlobalIndex)
                    .OrderByDescending(c => c.CharacterIndex)
                    .ToList();

                if (lineChords.Count == 0)
                {
                    resultLines.Add(line);
                }
                else
                {
                    var workingLineBuilder = new StringBuilder(line);

                    foreach (var chordItem in lineChords)
                    {
                        // Convert absolute global position back to a localized index on this specific line
                        int localInsertIndex = chordItem.CharacterIndex - lineStartGlobalIndex;

                        // Format the string bracket injection
                        string bracketedChord = $"[{chordItem.Chord}]";

                        // Safe-bound clamping just in case an index accidentally hits the line end line limit
                        if (localInsertIndex >= 0 && localInsertIndex <= workingLineBuilder.Length)
                        {
                            workingLineBuilder.Insert(localInsertIndex, bracketedChord);
                        }
                    }

                    resultLines.Add(workingLineBuilder.ToString());
                }

                // Increment the global character tracker, including the newline separator size
                runningGlobalCharCount += line.Length + Environment.NewLine.Length;
            }

            return resultLines.ToArray();
        }


        /// <summary>
        /// Return lines Wihout chords 
        /// </summary>
        /// <returns></returns>
        public string[] GetRootLines()
        {
            return Lyrics?.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }
        /// <summary>
        /// Slices your clean lyrics back into structural chunks ready for your MAUI UI binding layout loop
        /// </summary>
        public List<List<VisualSegment>> GetLyricsAndChords()
        {
            var multiLineResult = new List<List<VisualSegment>>();
            if (string.IsNullOrEmpty(Lyrics)) return multiLineResult;

            // Split into structural lines safely
            string[] lines = this.GetRootLines();
            int runningGlobalCharCount = 0;

            foreach (var line in lines)
            {
                var lineSegments = new List<VisualSegment>();
                int lineStartGlobalIndex = runningGlobalCharCount;
                int lineEndGlobalIndex = lineStartGlobalIndex + line.Length;

                // Grab chords that belong purely to this line's window space
                var lineChords = Chords
                    .Where(c => c.CharacterIndex >= lineStartGlobalIndex && c.CharacterIndex <= lineEndGlobalIndex)
                    .OrderBy(c => c.CharacterIndex)
                    .ToList();

                if (lineChords.Count == 0)
                {
                    lineSegments.Add(new VisualSegment { Chord = "", Text = line });
                }
                else
                {
                    // Prefix text before the first chord on this specific line
                    int initialLocalOffset = lineChords[0].CharacterIndex - lineStartGlobalIndex;
                    if (initialLocalOffset > 0)
                    {
                        lineSegments.Add(new VisualSegment
                        {
                            Chord = "",
                            Bar = this.Bar,
                            FontSize = this.FontSize,
                            Text = line.Substring(0, initialLocalOffset)
                        });
                    }

                    // Loop through mid-line segments
                    for (int i = 0; i < lineChords.Count; i++)
                    {
                        int currentLocalStart = lineChords[i].CharacterIndex - lineStartGlobalIndex;
                        int nextLocalStart = (i + 1 < lineChords.Count)
                            ? lineChords[i + 1].CharacterIndex - lineStartGlobalIndex
                            : line.Length;

                        lineSegments.Add(new VisualSegment
                        {
                            Chord = lineChords[i].Chord,
                            Bar = this.Bar,
                            FontSize = this.FontSize,
                            Text = line.Substring(currentLocalStart, nextLocalStart - currentLocalStart)
                        });
                    }
                }

                multiLineResult.Add(lineSegments);

                // Track global progression (include size of environment line ending)
                runningGlobalCharCount += line.Length + Environment.NewLine.Length;
            }

            return multiLineResult;
        }

        public static LyricSection Create(string name, int bar, string comments)
        {
            return new LyricSection
            {
                Name = name,
                Bar = bar,
                Comments = comments
            };
        }
    }

    public class ChordSection
    {
        public string Chord { get; set; }
        public int CharacterIndex { get; set; }
    }

    public class VisualSegment
    {
        public string Chord { get; set; }
        public string Text { get; set; }

        public int FontSize { get; set; } = 12;

        public int Bar { get; set; } = -1;
    }
}
