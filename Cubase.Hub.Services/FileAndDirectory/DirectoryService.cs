using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.FileAndDirectory
{
    public class DirectoryService : IDirectoryService
    {
        public List<string> GetCubaseProjects(string sourceFolderPath)
        {
            var files = Directory.GetFiles(
                sourceFolderPath,
                $"*{CubaseHubConstants.CubaseFileExtension}",
                SearchOption.AllDirectories
            );

            return files
                .Select(f => new FileInfo(f))               // create FileInfo once
                .OrderByDescending(fi => fi.LastWriteTime)  // order by last modified
                .Select(fi => fi.FullName)                  // back to string paths
                .ToList();
        }

        public bool MakeSureDirectoryExists(string directoryPath)
        {
            var dirPath = directoryPath;
            if (Path.HasExtension(directoryPath))
            {
                dirPath = Path.GetDirectoryName(directoryPath) ?? string.Empty;
            }
            if (!Directory.Exists(dirPath))
            {
                try
                {
                    Directory.CreateDirectory(dirPath);
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
    }
}
