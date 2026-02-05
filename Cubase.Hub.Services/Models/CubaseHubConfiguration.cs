using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.Models
{
    public class CubaseHubConfiguration
    {
        public List<string> SourceCubaseFolders { get; set; } = new List<string>();

        public string? CubaseExeLocation { get; set; }

        public string? CubaseUserTemplateLocation { get; set; }

        public string? CubaseTemplateLocation { get; set; }
    }
}
