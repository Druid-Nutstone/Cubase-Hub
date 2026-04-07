using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Cubase.Hub.Services.Distributers.SoundCloud
{
    public class CreatePlaylistRequest
    {
        [JsonPropertyName("playlist")]
        public PlaylistCreateData Playlist { get; set; }
    
        public static CreatePlaylistRequest CreateFromAlbum(AlbumConfiguration albumConfiguration, string description, PlayListType playListType = PlayListType.album)
        {
            return new CreatePlaylistRequest()
            {
                Playlist = new PlaylistCreateData()
                {
                    Title = albumConfiguration.Title,
                    Description = description,
                    ReleaseDate = $"01/01/{ albumConfiguration.Year }",
                    Genre = albumConfiguration.Genre,
                    LabelName = albumConfiguration.Label ?? albumConfiguration.Artist,
                    TagList = albumConfiguration.Artist,
                    SetType = playListType.ToString()
                }
            };
        }
    }

    public enum PlayListType
    {
        album = 0,
        playlist = 1
    }

    public class PlaylistCreateData
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("sharing")]
        public string? Sharing { get; set; } = "public";

        [JsonPropertyName("set_type")]
        public string? SetType { get; set; } = "album";

        [JsonPropertyName("genre")]
        public string? Genre { get; set; }

        [JsonPropertyName("label_name")]
        public string? LabelName { get; set; }

        [JsonPropertyName("release_date")]
        public string? ReleaseDate { get; set; }

        [JsonPropertyName("tag_list")]
        public string? TagList { get; set; }



    }
}
