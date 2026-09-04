using Cubase.Hub.Services.Models;

namespace Cubase.Hub.Services.Projects
{
    public interface IProjectService
    {
        CubaseProjectCollection? LoadProjects(Action<string> OnError);

        CubaseProjectCollection Projects { get; }

    }
}
