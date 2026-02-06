using Cubase.Hub.Services.Config;
using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.FileAndDirectory
{
    public class DirectoryService : IDirectoryService
    {
        
        public DirectoryService()
        {
        }

        public List<Template> GetCubaseTemplates(List<string> cubaseTemplates)
        {
            var templateCollection = new List<Template>();
            foreach (var dir in cubaseTemplates)
            {
                templateCollection.AddRange(
                    Directory.GetFiles(dir,
                                       $"*{CubaseHubConstants.CubaseFileExtension}",
                                       SearchOption.AllDirectories
                                       ).Select(x => new Template()
                                       {
                                           TemplateLocation = x,
                                           TemplateName = Path.GetFileNameWithoutExtension(x)
                                       })
                );
            }
            return templateCollection;
        }

        public List<AlbumLocation> GetCubaseAlbums(List<string> cubaseDirectories)
        {
            var albumCollection = new List<AlbumLocation>();
            foreach (var dir in cubaseDirectories)
            {
                albumCollection.AddRange(Directory.GetFiles(
                    dir,
                    $"*{CubaseHubConstants.CubaseAlbumFileExtension}",
                    SearchOption.AllDirectories
                ).Select(x => new AlbumLocation() 
                { 
                    AlbumPath = Directory.GetParent(x).FullName, 
                    AlbumName = Path.GetFileName(Path.GetDirectoryName(x))
                }));
            }
            return albumCollection;
        }
        
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

        public MixDownCollection GetMixes(string albumPath)
        {
            var mixDownCollection = new MixDownCollection();
            Directory.GetDirectories(albumPath, "*", SearchOption.AllDirectories)
                     .ToList()
                     .ForEach(dir =>
                     {
                         mixDownCollection.CreateFromFiles(GetMixDownFiles(dir));   
                     });

            string[] GetMixDownFiles(string root)
            {
                if (Path.GetFileName(root) == CubaseHubConstants.MixdownDirectory)
                {
                    return Directory.GetFiles(root);
                }
                return [];  
            }

            return mixDownCollection;
        }
    }
}
