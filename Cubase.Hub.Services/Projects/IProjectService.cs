using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.Projects
{
    public interface IProjectService
    {
        CubaseProjectCollection? LoadProjects(Action<string> OnError);

    }
}
