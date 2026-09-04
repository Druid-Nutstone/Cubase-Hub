using Cubase.Hub.Services.Models;

namespace Cubase.Hub.Services.Distributers
{
    public interface IDistributer
    {
        bool Distribute(MixDown mixDown, Action<string> onError);

    }
}
