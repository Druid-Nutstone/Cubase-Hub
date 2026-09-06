using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cubase.Macro.Common.Models.Lyrics
{
    public class SetListContainer
    {
        public string Title { get; set; }

        public string Notes { get; set; }

        public List<SetListSong> Songs { get; set; } = new List<SetListSong>();

        [JsonIgnore]
        public int CurrentSong { get; set; } = 0;

        public void Save(string fileName)
        {
            File.WriteAllText(fileName,
                              JsonSerializer.Serialize(this, new JsonSerializerOptions() { WriteIndented = true }));
        }

        public static SetListContainer Load(string fileName)
        {
            return JsonSerializer.Deserialize<SetListContainer>(File.ReadAllText(fileName));
        }
    }

    public class SetListSong
    {
        public int Index { get; set; }

        public string FileName { get; set; }

        public string Title { get; set; }

        public string Notes { get; set; }
    }
}
