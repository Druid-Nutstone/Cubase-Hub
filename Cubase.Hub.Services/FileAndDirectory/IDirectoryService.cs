using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.FileAndDirectory
{
    public interface IDirectoryService
    {
        bool MakeSureDirectoryExists(string directoryPath);

        List<string> GetCubaseProjects(string sourceFolderPath);
    }
}
