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
    
        public WindowSettings? MainWindowLocation {  get; set; } 

        public WindowSettings? AlbumWindowLocation { get; set; }

        public string? LastExportFolderLocation { get; set; }

        public AlbumExportCollection? AlbumExports { get; set; } = new AlbumExportCollection();

        public IEnumerable<string>? RecentProjects { get; set; } = new List<string>();
    }

    public class WindowSettings
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
