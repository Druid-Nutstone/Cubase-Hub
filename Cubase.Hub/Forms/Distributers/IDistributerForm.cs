using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Forms.Distributers
{
    public interface IDistributerForm
    {
        void Initialise();

        UserControl MainControl { get; }

        void SetAlbum(AlbumConfiguration albumConfiguration, MixDownCollection mixDowns); 
    }
}
