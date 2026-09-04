using Cubase.Hub.Services.Models;

namespace Cubase.Hub.Forms.Distributers
{
    public interface IDistributerForm
    {
        void Initialise();

        public string ProviderName { get; }

        UserControl MainControl { get; }

        void SetAlbum(AlbumConfiguration albumConfiguration, MixDownCollection mixDowns);

        UserControl TrackControl { get; }

        void UploadMixes(MixDownCollection mixDowns);
    }
}
