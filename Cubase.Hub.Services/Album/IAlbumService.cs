using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.Album
{
    public interface IAlbumService
    {
        AlbumConfiguration Configuration { get; }

        void NewAlbum();

        bool VerifyAlbum(Action<string> onError);

        bool SaveAlbum(string targetDirectory, Action<string> onError);

        List<AlbumLocation> GetAlbumList(Action<string> onError);

        AlbumConfiguration GetAlbumConfigurationFromAlbumLocation(AlbumLocation albumLocation);

        MixDownCollection GetMixesForAlbum(AlbumLocation albumLocation);

        string AlbumExportLocation(AlbumLocation albumLocation);

        void InitialiseAlbumArt(string albumExportLocation);

        string InitialiseAlbumExportLocation(AlbumLocation albumLocation, Action<string> onError);
    }
}
