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
