using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Cubase.Hub.Services.Distributers.SoundCloud
{
    public class SoundCloudTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
