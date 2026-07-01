using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Cubase.Macro.Common.Models
{
    public class CubaseMidiProjectStatus
    {
        public string ProjectName { get; set; }

        public CubaseMidiProjectStatusType ProjectStatus { get; set; } = CubaseMidiProjectStatusType.Unknown;

        public string Message { get; set; }

        public string Serialise()
        {
            var json = JsonSerializer.Serialize(this);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        }

        public static CubaseMidiProjectStatus Deserialise(string message)
        {
            try
            {
                var json = Encoding.UTF8.GetString(Convert.FromBase64String(message));
                return JsonSerializer.Deserialize<CubaseMidiProjectStatus>(json);
            }
            catch (Exception ex)
            {
                return new CubaseMidiProjectStatus() { ProjectStatus = CubaseMidiProjectStatusType.Unknown};
            }
        }
    }

    public enum CubaseMidiProjectStatusType
    {
        Active = 0,
        NotActive = 1,
        Unknown = 2
    }
}
