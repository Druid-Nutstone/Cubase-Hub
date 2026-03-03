using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Cubase.Hub.Services.Distributers.SoundCloud
{
    public class UpdatePlaylistRequest
    {
        [JsonPropertyName("playlist")]
        public PlaylistUpdate Playlist { get; set; }
    }

    public class PlaylistUpdate
    {
        [JsonPropertyName("tracks")]
        public List<TrackRef> Tracks { get; set; }
    }

    public class TrackRef
    {
        [JsonPropertyName("urn")]
        public string Urn { get; set; }
    }
}
