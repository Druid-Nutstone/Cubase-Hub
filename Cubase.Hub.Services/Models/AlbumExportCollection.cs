using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.Models
{
    public class AlbumExportCollection : List<AlbumExport>
    {
    }

    public class AlbumExport
    {
        public string Name { get; set; }

        public string Location { get; set; }
    }
}
