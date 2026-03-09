using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Cubase.Hub.Services.Distributers.SoundCloud
{

    public class SoundCloudTrackCollection : List<SoundCloudTrack>
    {
        public SoundCloudTrack? GetTrackByTitle(string title)
        {
            return this.FirstOrDefault(x => x.Title == title);
        }

        public void Save(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);  
            }
            var asText = JsonSerializer.Serialize(this);    
            File.WriteAllText(fileName, asText);    
        }

        public static SoundCloudTrackCollection LoadFrom(string fileName)
        {
            var scText = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<SoundCloudTrackCollection>(scText);
        }
    }

    public class SoundCloudTrack
    {
        [JsonPropertyName("kind")]
        public string? Kind { get; set; }

        [JsonPropertyName("id")]
        public long? Id { get; set; }

        [JsonPropertyName("urn")]
        public string? Urn { get; set; }

        [JsonPropertyName("created_at")]
        public string? CreatedAt { get; set; }

        [JsonPropertyName("duration")]
        public int? Duration { get; set; }

        [JsonPropertyName("commentable")]
        public bool? Commentable { get; set; }

        [JsonPropertyName("comment_count")]
        public int? CommentCount { get; set; }

        [JsonPropertyName("sharing")]
        public string? Sharing { get; set; }

        [JsonPropertyName("tag_list")]
        public string? TagList { get; set; }

        [JsonPropertyName("streamable")]
        public bool? Streamable { get; set; }

        [JsonPropertyName("embeddable_by")]
        public string? EmbeddableBy { get; set; }

        [JsonPropertyName("purchase_url")]
        public string? PurchaseUrl { get; set; }

        [JsonPropertyName("purchase_title")]
        public string? PurchaseTitle { get; set; }

        [JsonPropertyName("genre")]
        public string? Genre { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("label_name")]
        public string? LabelName { get; set; }

        [JsonPropertyName("release")]
        public string? Release { get; set; }

        [JsonPropertyName("key_signature")]
        public string? KeySignature { get; set; }

        [JsonPropertyName("isrc")]
        public string? Isrc { get; set; }

        [JsonPropertyName("bpm")]
        public int? Bpm { get; set; }

        [JsonPropertyName("release_year")]
        public int? ReleaseYear { get; set; }

        [JsonPropertyName("release_month")]
        public int? ReleaseMonth { get; set; }

        [JsonPropertyName("release_day")]
        public int? ReleaseDay { get; set; }

        [JsonPropertyName("license")]
        public string? License { get; set; }

        [JsonPropertyName("uri")]
        public string? Uri { get; set; }

        [JsonPropertyName("user")]
        public SoundCloudUser? User { get; set; }

        [JsonPropertyName("permalink_url")]
        public string? PermalinkUrl { get; set; }

        [JsonPropertyName("artwork_url")]
        public string? ArtworkUrl { get; set; }

        [JsonPropertyName("stream_url")]
        public string? StreamUrl { get; set; }

        [JsonPropertyName("download_url")]
        public string? DownloadUrl { get; set; }

        [JsonPropertyName("waveform_url")]
        public string? WaveformUrl { get; set; }

        [JsonPropertyName("available_country_codes")]
        public string? AvailableCountryCodes { get; set; }

        [JsonPropertyName("secret_uri")]
        public string? SecretUri { get; set; }

        [JsonPropertyName("user_favorite")]
        public bool? UserFavorite { get; set; }

        [JsonPropertyName("user_playback_count")]
        public int? UserPlaybackCount { get; set; }

        [JsonPropertyName("playback_count")]
        public int? PlaybackCount { get; set; }

        [JsonPropertyName("download_count")]
        public int? DownloadCount { get; set; }

        [JsonPropertyName("favoritings_count")]
        public int? FavoritingsCount { get; set; }

        [JsonPropertyName("reposts_count")]
        public int? RepostsCount { get; set; }

        [JsonPropertyName("downloadable")]
        public bool? Downloadable { get; set; }

        [JsonPropertyName("access")]
        public string? Access { get; set; }

        [JsonPropertyName("policy")]
        public string? Policy { get; set; }

        [JsonPropertyName("monetization_model")]
        public string? MonetizationModel { get; set; }

        [JsonPropertyName("metadata_artist")]
        public string? MetadataArtist { get; set; }

        public bool isUploadDateOlderThan(DateTime targetDate)
        {
            if (!string.IsNullOrEmpty(this.CreatedAt))
            {
                DateTimeOffset parsedDate = DateTimeOffset.ParseExact(
                      this.CreatedAt,
                      "yyyy/MM/dd HH:mm:ss zzz",
                      CultureInfo.InvariantCulture
                );
                DateTime compareDate = targetDate;

                return parsedDate.UtcDateTime < compareDate;
            }
            else
            {
                return true; // no date so must be older 
            }
        }

        public class SoundCloudUser
        {
            [JsonPropertyName("avatar_url")]
            public string? AvatarUrl { get; set; }

            [JsonPropertyName("id")]
            public long? Id { get; set; }

            [JsonPropertyName("urn")]
            public string? Urn { get; set; }

            [JsonPropertyName("kind")]
            public string? Kind { get; set; }

            [JsonPropertyName("permalink_url")]
            public string? PermalinkUrl { get; set; }

            [JsonPropertyName("uri")]
            public string? Uri { get; set; }

            [JsonPropertyName("username")]
            public string? Username { get; set; }

            [JsonPropertyName("permalink")]
            public string? Permalink { get; set; }

            [JsonPropertyName("created_at")]
            public string? CreatedAt { get; set; }

            [JsonPropertyName("last_modified")]
            public string? LastModified { get; set; }

            [JsonPropertyName("first_name")]
            public string? FirstName { get; set; }

            [JsonPropertyName("last_name")]
            public string? LastName { get; set; }

            [JsonPropertyName("full_name")]
            public string? FullName { get; set; }

            [JsonPropertyName("city")]
            public string? City { get; set; }

            [JsonPropertyName("description")]
            public string? Description { get; set; }

            [JsonPropertyName("country")]
            public string? Country { get; set; }

            [JsonPropertyName("track_count")]
            public int? TrackCount { get; set; }

            [JsonPropertyName("public_favorites_count")]
            public int? PublicFavoritesCount { get; set; }

            [JsonPropertyName("reposts_count")]
            public int? RepostsCount { get; set; }

            [JsonPropertyName("followers_count")]
            public int? FollowersCount { get; set; }

            [JsonPropertyName("followings_count")]
            public int? FollowingsCount { get; set; }

            [JsonPropertyName("plan")]
            public string? Plan { get; set; }

            [JsonPropertyName("myspace_name")]
            public string? MyspaceName { get; set; }

            [JsonPropertyName("discogs_name")]
            public string? DiscogsName { get; set; }

            [JsonPropertyName("website_title")]
            public string? WebsiteTitle { get; set; }

            [JsonPropertyName("website")]
            public string? Website { get; set; }

            [JsonPropertyName("comments_count")]
            public int? CommentsCount { get; set; }

            [JsonPropertyName("online")]
            public bool? Online { get; set; }

            [JsonPropertyName("likes_count")]
            public int? LikesCount { get; set; }

            [JsonPropertyName("playlist_count")]
            public int? PlaylistCount { get; set; }

            [JsonPropertyName("subscriptions")]
            public List<SoundCloudSubscription>? Subscriptions { get; set; }
        }

        public class SoundCloudSubscription
        {
            [JsonPropertyName("product")]
            public SoundCloudProduct? Product { get; set; }
        }

        public class SoundCloudProduct
        {
            [JsonPropertyName("id")]
            public string? Id { get; set; }

            [JsonPropertyName("name")]
            public string? Name { get; set; }
        }
    }
}
