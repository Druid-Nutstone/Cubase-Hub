using Cubase.Macro.Models;

namespace Cubase.Macro.Services.Config
{
    public class ConfigurationService : IConfigurationService
    {
        public CubaseMacroConfiguration Configuration { get; private set; } = CubaseMacroConfiguration.Load();

        public ConfigurationService()
        {

        }

        public void ReloadConfiguration()
        {
            this.Configuration = CubaseMacroConfiguration.Load();
        }
    }
}
