using Cubase.Macro.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Services.Config
{
    public interface IConfigurationService
    {
        CubaseMacroConfiguration Configuration { get; }

        void ReloadConfiguration();
    }
}
