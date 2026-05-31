using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Cubase.Macro.Common.Models
{
    public class TrackCollection : List<Track>
    {
        public void UpdateTrack(Track track)
        {
            var trackToUpdate = this.FirstOrDefault(x => x.Id == track.Id);
            if (trackToUpdate == null)
            {
                trackToUpdate = new Track() { Id = track.Id, Mute = track.Mute, Name = track.Name, Solo = track.Solo, TrackType = track.TrackType, Volume = track.Volume };
                return;
            }
            else
            {
                this[this.FindIndex(x => x.Id == trackToUpdate.Id)] = track;
            }
        }
    }

    public class Track
    {
        public string Id { get; set; }
        
        public string Name { get; set; }

        public string TrackType { get; set; }

        public bool Mute { get; set; }

        public bool Solo { get; set; }

        public bool RecordEnable { get; set; }

        public bool Selected {  get; set; } 

        public double Volume { get; set; } 

    }
}
