using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Mobile.Configuration
{
    public class MobileConfigurationService : IMobileConfigurationService
    {
        public MobileConfiguration Configuration { get; set; }
    
        public MobileConfigurationService()
        {

        }

        public async Task InitialiseConfiguration()
        {
            this.Configuration = MobileConfiguration.LoadFromFile();
            if (!File.Exists(CubaseMacroMobileConstants.ConfigurationFile))
            {
                this.Configuration.Save();
            }
        }
    
    }
}
