namespace Cubase.Macro.Common.Models
{
    public class UpdateTrack
    {
        public string Id { get; set; }

        public int Value { get; set; }

        public TrackUpdateValueType TrackValueType { get; set; }
    }

    public enum TrackUpdateValueType
    {
        Record = 0,
        Solo = 1,
        Mute = 2,
        Selected = 3,
        Unknown = 99
    }
}
