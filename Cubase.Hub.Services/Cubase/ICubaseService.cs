using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.Cubase
{
    public interface ICubaseService
    {
        void OpenCubaseProject(string projectPath);
    }
}
