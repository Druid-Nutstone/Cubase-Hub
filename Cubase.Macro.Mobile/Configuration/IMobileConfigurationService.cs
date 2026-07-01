using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Mobile.Configuration
{
    public interface IMobileConfigurationService
    {
        MobileConfiguration Configuration { get; set; }
        Task InitialiseConfiguration();
    }
}
