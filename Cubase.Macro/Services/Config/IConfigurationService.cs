using Cubase.Macro.Models;

namespace Cubase.Macro.Services.Config
{
    public interface IConfigurationService
    {
        CubaseMacroConfiguration Configuration { get; }

        void ReloadConfiguration();
    }
}
