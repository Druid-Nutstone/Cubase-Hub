using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Cubase.Macro.Common.Models
{
    public class LyricResponseModel
    {
        public string TrackName { get; set; }

        public IEnumerable<string> LyricContent { get; set; }

        public bool IsSuccess { get; set; } = false;

        public string ErrorMessage { get; set; }

        public static LyricResponseModel Deserialise(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return CreateWithError("Message is null or empty");
            };

            var json = Encoding.UTF8.GetString(Convert.FromBase64String(message));
            try
            {
                return JsonSerializer.Deserialize<LyricResponseModel>(json);
            }
            catch (Exception ex)
            {
                return CreateWithError($"Failed to deserialise LyricResponseModel: {ex.Message}");
            }
        }

        public string Serialise()
        {
            var json = JsonSerializer.Serialize(this);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        }

        public static LyricResponseModel CreateWithError(string errorMessage)
        {
            return new LyricResponseModel() { IsSuccess = false, ErrorMessage = errorMessage };
        }

        public static LyricResponseModel Create(string trackName, IEnumerable<string> lyricContent)
        {
            return new LyricResponseModel() { TrackName = trackName, LyricContent = lyricContent, IsSuccess = true };    
        }

    }
}
