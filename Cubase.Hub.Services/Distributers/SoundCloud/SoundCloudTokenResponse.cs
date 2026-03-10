using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cubase.Hub.Services.Distributers.SoundCloud
{ 
    public class SoundCloudTokenResponse
    {
        private static string SoundCloudTokenLocation => Path.Combine(CubaseHubConstants.UserAppDataFolderPath, "soundcloudtoken.json");
        
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonIgnore]
        public DateTime ExpiresAt { get; set; }

        [JsonIgnore]
        public bool HasExpired => ExpiresAt <= DateTime.UtcNow;

        public void SetExpires() 
        { 
           this.ExpiresAt = DateTime.UtcNow.AddSeconds(ExpiresIn);  
        } 

        public void Save()
        {
            var asText = JsonSerializer.Serialize<SoundCloudTokenResponse>(this);
            File.WriteAllText(SoundCloudTokenLocation, asText);
        }

        public bool Exists => File.Exists(SoundCloudTokenLocation); 

        public static SoundCloudTokenResponse? Load()
        {
            if (File.Exists(SoundCloudTokenLocation))
            {
                return JsonSerializer.Deserialize<SoundCloudTokenResponse>(File.ReadAllText(SoundCloudTokenLocation));
            }
            return null;
        }

        public static void Delete()
        {
            if (File.Exists(SoundCloudTokenLocation))
            {
                File.Delete(SoundCloudTokenLocation);   
            }
        }

        public static bool TokenExists()
        {
            return File.Exists(SoundCloudTokenLocation);
        }

    }
}
