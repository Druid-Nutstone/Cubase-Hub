using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Cubase.Hub.Services.Distributers.SoundCloud
{
    public class SoundCloudPlaylistCollection : List<SoundCloudPlaylist>
    {
        public void AddOrUpdatePlayList(SoundCloudPlaylist playlist)
        {
            var index = this.FindIndex(0, (x) => x.Id == playlist.Id);

            if (index < 0)
            {
                this.Add(playlist);
                return;
            }
            this[index] = playlist;
        }
        
        public bool HaveAlbum(string albumName)
        {
            return this.FirstOrDefault(x => x.Title == albumName) != null;
        } 

        public SoundCloudPlaylist GetAlbum(string albumName)
        {
            return this.FirstOrDefault(x => x.Title == albumName);
        }

        public SoundCloudTrack? GetTrack(string trackName)
        {
            return this.SelectMany(x => x.Tracks?.Where(y => y.Title == trackName))?.FirstOrDefault(); 
        }
    }

    public class SoundCloudPlaylist
    {
        [JsonPropertyName("id")]
        public long? Id { get; set; }

        [JsonPropertyName("urn")]
        public string? Urn { get; set; }

        [JsonPropertyName("kind")]
        public string? Kind { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("permalink")]
        public string? Permalink { get; set; }

        [JsonPropertyName("permalink_url")]
        public string? PermalinkUrl { get; set; }

        [JsonPropertyName("uri")]
        public string? Uri { get; set; }

        [JsonPropertyName("duration")]
        public long? Duration { get; set; }

        [JsonPropertyName("genre")]
        public string? Genre { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("tag_list")]
        public string? TagList { get; set; }

        [JsonPropertyName("track_count")]
        public int? TrackCount { get; set; }

        [JsonPropertyName("playlist_type")]
        public string? PlaylistType { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("artwork_url")]
        public string? ArtworkUrl { get; set; }

        [JsonPropertyName("tracks_uri")]
        public string? TracksUri { get; set; }

        [JsonPropertyName("created_at")]
        public string? CreatedAtRaw { get; set; }

        [JsonPropertyName("last_modified")]
        public string? LastModifiedRaw { get; set; }

        [JsonPropertyName("release_year")]
        public int? ReleaseYear { get; set; }

        [JsonPropertyName("release_month")]
        public int? ReleaseMonth { get; set; }

        [JsonPropertyName("release_day")]
        public int? ReleaseDay { get; set; }

        [JsonPropertyName("user")]
        public SoundCloudUser? User { get; set; }

        [JsonPropertyName("tracks")]
        public List<SoundCloudTrack>? Tracks { get; set; }
   
    }
}
