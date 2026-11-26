using Cubase.Macro.Common.Models;

namespace Cubase.Macro.Tests.Fakes
{
    public class FakeTrackCollection : TrackCollection
    {
        public FakeTrackCollection()
        {
        }

        public void AddFakeTracks()
        {
            base.Add(new Track() { Id = "FakeTrack2", Name = "Track 2" });
        }
    }
}
