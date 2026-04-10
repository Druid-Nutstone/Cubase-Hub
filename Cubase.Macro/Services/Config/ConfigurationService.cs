using Cubase.Macro.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Services.Config
{
    public class ConfigurationService : IConfigurationService
    {
        public CubaseMacroConfiguration Configuration { get; private set; } = CubaseMacroConfiguration.Load();

        public ConfigurationService()
        {
            
        }
    }
}
