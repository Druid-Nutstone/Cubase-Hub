using Cubase.Hub.Services.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cubase.Hub.Services.Distributers.SoundCloud
{
    public class SoundCloudCache
    {
        private static string SoundCloudCacheLocation = Path.Combine(CubaseHubConstants.CachePath, "SoundCloudCache.json");

        /*
        public SoundCloudTrackCollection Tracks { get; set; }
        */
        public SoundCloudPlaylistCollection Albums { get; set; }

        /*
        [JsonIgnore]
        public List<AlbumLocation> UpdatedAlbums { get; set; } = new List<AlbumLocation>(); 
        */
        public SoundCloudCache() 
        { 
        }

        /*
        public void AddUpdatedAlbum(AlbumLocation? album)
        {
            if (album != null)
            {
                if (!UpdatedAlbums.Any(x => x.AlbumName == album?.AlbumName))
                {
                    UpdatedAlbums.Add(album);
                }
            }
        }
        */
        public bool CreateCache(SoundCloudDistributionProvider soundCloud, Action<string> onError)
        {
            this.Albums = soundCloud.GetPlayLists(onError);
            if (this.Albums == null)
            {
                return false;
            }
            /*
            this.Tracks = soundCloud.GetTracks(onError);
            if (this.Tracks == null)
            {
                return false;
            }
            */
            var thisAsText = JsonSerializer.Serialize(this);
            File.WriteAllText(SoundCloudCacheLocation, thisAsText);
            return true;
        }

        public bool CacheExists()
        {
            return File.Exists(SoundCloudCacheLocation);
        }

        public static void RemoveCache()
        {
            if (File.Exists(SoundCloudCacheLocation))
            {
                File.Delete(SoundCloudCacheLocation);
            }
        } 

        public static SoundCloudCache Create()
        {
            if (File.Exists(SoundCloudCacheLocation))
            {
                var cacheLoc = File.ReadAllText(SoundCloudCacheLocation);
                return JsonSerializer.Deserialize<SoundCloudCache>(cacheLoc);
            }
            else
            {
                return new SoundCloudCache();
            }
        }


    }
}
