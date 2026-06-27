using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Cubase.Macro.Common.Models
{
    public class LyricIndexCollection 
    {
        public List<Album> Albums { get; set; } = new List<Album>();

        public List<SetList> SetLists { get; set; } = new List<SetList>();

        public List<Lyric> Lyrics { get; set; } = new List<Lyric>();
    
        public LyricIndexCollection PopulateLyricFiles(string baseDirectory)
        {
            this.Lyrics.Clear();
            var availableLyrics = Directory.GetFiles(baseDirectory, "*.txt").ToList();
            availableLyrics.ForEach(x => this.Lyrics.Add(new Lyric() 
            { 
               FileName = Path.GetFileName(x),
               TrackName = Path.GetFileNameWithoutExtension(x),
               LastModified = File.GetLastWriteTimeUtc(x)
            }));
            return this;
        }

        public string Serialise()
        {
            var json = JsonSerializer.Serialize(this);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        }

        public void SerialiseToFile(string path)
        {
            File.WriteAllText(path, JsonSerializer.Serialize(this, new JsonSerializerOptions() { WriteIndented = true}));
        }

        public static LyricIndexCollection DeserialiseFromFile(string path)
        {
            if (File.Exists(path))
            {
                return JsonSerializer.Deserialize<LyricIndexCollection>(File.ReadAllText(path));
            }
            else return new LyricIndexCollection();
        }

        public static LyricIndexCollection Deserialise(string message)
        {
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(message));
            return JsonSerializer.Deserialize<LyricIndexCollection>(json);
        }
    }

    public class SetList
    {
        public string Name { get; set; }

        public List<string> Tracks { get; set; } = new List<string>();  
    }

    public class Album
    {
        public string Name { get; set; }
    
        public List<string> Tracks {  get; set; } = new List<string>();
    } 

    public class Lyric
    {
        public string FileName { get; set; }
    
        public string TrackName { get; set; }

        public DateTime LastModified { get; set; }

        public string Serialise()
        {
            var json = JsonSerializer.Serialize(this);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        }

        public static Lyric Deserialise(string message)
        {
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(message));
            return JsonSerializer.Deserialize<Lyric>(json);
        }
    }
}
