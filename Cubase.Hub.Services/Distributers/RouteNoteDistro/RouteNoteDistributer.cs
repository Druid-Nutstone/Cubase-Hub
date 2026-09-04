using Cubase.Hub.Services.Models;
using Cubase.Hub.Services.Track;

namespace Cubase.Hub.Services.Distributers.RouteNoteDistro
{
    public class RouteNoteDistributer : IDistributer
    {
        private readonly ITrackService trackService;

        public RouteNoteDistributer(ITrackService trackService)
        {
            this.trackService = trackService;
        }

        public bool Distribute(MixDown mixDown, Action<string> onError)
        {
            if (string.IsNullOrEmpty(mixDown.ExportLocation))
            {
                onError("there is no export folder defined for this track");
                return false;
            }
            this.trackService.ConvertToFlac(mixDown, mixDown.ExportLocation, new FlacConfiguration()
            {
                CompressionLevel = CompressionLevel.High8,
                Depth = BitDepth.Bit16,
                SampleRate = SampleRate.Herts44
            });
            return true;
        }
    }
}
