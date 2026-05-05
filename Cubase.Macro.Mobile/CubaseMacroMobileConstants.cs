using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Mobile
{
    public static class CubaseMacroMobileConstants
    {
#if DEBUG
        public static string TargetIPAddress => "localhost";
        // Place sensitive logging or diagnostic tools here

#else
       public static string TargetIPAddress => "192.168.4.9"; 
#endif

    }
}
