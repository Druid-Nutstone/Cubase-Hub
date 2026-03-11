using Cubase.Hub.Services.Models;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

namespace Cubase.Hub.Services.Distributers.SoundCloud
{
    public class UpdateTrackMetaDataRequest
    {
        [JsonPropertyName("track")]
        public UpdateTrackMetaDataRequestTrack Track { get; set; }
    
        public static UpdateTrackMetaDataRequest CreateFromMixdown(MixDown mixDown)
        {
            var year = mixDown.Year == 0 ? DateTime.Now.Year : (int)mixDown.Year;

            return new UpdateTrackMetaDataRequest()
            {
                Track = new UpdateTrackMetaDataRequestTrack()
                 {
                     Title = mixDown.Title,
                     Description = BuildDescription(),
                     Genre = mixDown.Genre ?? "Not Specified",
                     ReleaseDate = new DateOnly(year, 1, 1).ToString("yyyy-MM-dd"),
                     TagList = $"{mixDown.Artist} {mixDown.Genre} {mixDown.Year}",
                     LabelName = mixDown.Artist ?? string.Empty
                 }
            };

            string BuildDescription()
            {
                if (!string.IsNullOrEmpty(mixDown.Comment))
                {
                    return mixDown.Comment;
                }
                var performers = mixDown.Artist;
                if (!string.IsNullOrEmpty(mixDown.Performers))
                {
                    performers = string.Join(" ", mixDown.Performers.Split(";")).Trim();
                }
                var buildPerformers = $"performed by: {performers};";
                return buildPerformers;
            }
        }

     }


    public class UpdateTrackMetaDataRequestTrack
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("genre")]
        public string? Genre { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("tag_list")]
        public string? TagList { get; set; }

        [JsonPropertyName("release_date")]
        public string? ReleaseDate { get; set; }

        [JsonPropertyName("label_name")]
        public string? LabelName { get; set; }

    }
}
