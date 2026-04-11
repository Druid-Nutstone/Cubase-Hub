using Cubase.Macro.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Cubase.Macro.Services
{
    public sealed class StaticConfig
    {
        private static StaticConfig _instance;

        private CubaseMacroConfiguration cubaseMacroConfiguration;

        public CubaseMacroConfiguration Config => cubaseMacroConfiguration;

        public static StaticConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StaticConfig();
                }
                return _instance;

            }
        }

        public void SetConfiguration(CubaseMacroConfiguration configuration)
        {
            this.cubaseMacroConfiguration = configuration;  
        }

    }
}
