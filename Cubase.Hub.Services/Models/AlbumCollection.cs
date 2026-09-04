namespace Cubase.Hub.Services.Models
{
    public class AlbumCollection : List<AlbumLocation>
    {

    }

    public class AlbumLocation
    {
        public string AlbumName { get; set; }

        public string AlbumPath { get; set; }
    }
}
