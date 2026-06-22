using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Cubase.Macro.Common.Models
{
    public class TransportLocationCollection 
    {
        public TransportLocationCollection()
        {
            this.TransportType = TransportType.NotSpecified;
        }

        public TransportLocationCollection(TransportLocaton transportLocation)
        {
            switch (transportLocation.LocationType)
            {
                case TransportType.Seconds:
                    this.TransportType = TransportType.Seconds;
                    this.SecondsTime = transportLocation.Location;
                    break;
                case TransportType.BarsBeats:
                    this.TransportType = TransportType.BarsBeats;
                    this.BarBeatTime = int.Parse(transportLocation.Location);
                    break;
                default:
                    this.TransportType = TransportType.Samples;
                    this.SampleTime = transportLocation.Location;
                    break;
            }
        }
        public TransportType TransportType { get; set; }
    
        public string SecondsTime {  get; set; }

        public int BarBeatTime { get; set; }

        public string SampleTime { get; set; }

        public static TransportLocationCollection Deserialise(string message)
        {
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(message));
            return JsonSerializer.Deserialize<TransportLocationCollection>(json);
        }

        public string Serialise()
        {
            var json = JsonSerializer.Serialize(this);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        }

    }
}
