using System.Text.Json;

namespace Cubase.Macro.Common.Models
{
    public class LyricContainer
    {
        public string Title { get; set; }

        public TimeSpan Duration { get; set; } = TimeSpan.Zero;

        public string Album { get; set; }

        public int Bpm { get; set; } = 120;

        public double TimeSignature { get; set; } = 4.0; // 4/4 time signature

        public int FontSize { get; set; } = 12;

        public List<LyricSection> Sections { get; set; } = new List<LyricSection>();

        public void SetSectionFontSize(int? fontSize = -1)
        {
            this.FontSize = fontSize > 0 ? fontSize.Value : this.FontSize;
            foreach (var section in this.Sections)
            {
                section.FontSize = this.FontSize;
            }
        }

        public IEnumerable<int> GetOrderedBars()
        {
            return this.Sections.Select(x => x.Bar).Order().ToArray();
        }

        public string? Save(string targetPath, Action<string> onError)
        {
            try
            {
                // do some checking ...
                if (string.IsNullOrEmpty(this.Title) || this.Duration == TimeSpan.Zero)
                {
                    onError?.Invoke("Title and duration are required");
                    return null;
                }

                var asText = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                var targetFile = Path.Combine(targetPath, $"{this.Title.Trim()}{CubaseMacroConstants.NutstoneLyricNotation}");
                File.WriteAllText(targetFile, asText);
                return targetFile;
            }
            catch (Exception ex)
            {
                onError?.Invoke($"Failed to save LyricContainer: {ex.Message}");
                return null;
            }
        }

        public static LyricContainer? Load(string filePath, Action<string> onError)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    onError?.Invoke($"File not found: {filePath}");
                    return null;
                }
                var asText = File.ReadAllText(filePath);
                var lyricContainer = JsonSerializer.Deserialize<LyricContainer>(asText);
                lyricContainer.SetSectionFontSize(lyricContainer.FontSize);
                return lyricContainer;
            }
            catch (Exception ex)
            {
                onError?.Invoke($"Failed to load LyricContainer: {ex.Message}");
                return null;
            }
        }

    }
}
