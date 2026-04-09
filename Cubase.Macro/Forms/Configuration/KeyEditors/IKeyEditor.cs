using Cubase.Macro.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Forms.Configuration.KeyEditors
{
    public interface IKeyEditor
    {
        CubaseMacro Macro { get; set; }
    }
}
