using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.Models
{
    public class CubaseProjectCollection : List<CubaseProject>
    {
        public CubaseProjectCollection()
        {

        }
        
        public CubaseProjectCollection(IEnumerable<CubaseProject> projects)
        {
            this.AddRange(projects);
        }

        public IEnumerable<AlbumLocation> GetAlbums()
        {
            var albums =  this.Where(a => a.Album != null)
                .Select(x => new AlbumLocation() 
                { 
                    AlbumName= x.Album, 
                    AlbumPath = x.AlbumPath  
                });
            return albums;
        } 

        public CubaseProject AddProject(string cprFileName)
        {
            string parentDir = Path.GetDirectoryName(cprFileName); // immediate parent
            string mixDownPath = Path.Combine(parentDir, CubaseHubConstants.MixdownDirectory);
            var mixDownFiles = new List<string>();
            if (Directory.Exists(mixDownPath))
            {
                mixDownFiles = Directory.GetFiles(mixDownPath, "*.*", SearchOption.TopDirectoryOnly)
                                        .ToList();
            }

            string album = null;
            var albumPath = Directory.GetParent(
                      Path.GetDirectoryName(cprFileName)
                      )?.FullName;
            if (File.Exists(Path.Combine(albumPath, CubaseHubConstants.CubaseAlbumConfigurationFileName)))
            {
                album = Path.GetFileName(albumPath);
            }
            else
            {
                albumPath = null;
            }
            var project = CubaseProject.Create(Path.GetFileNameWithoutExtension(cprFileName), cprFileName, Path.GetFileName(parentDir), album, albumPath, mixDownFiles);
            this.Add(project);
            return project;
        }

        public CubaseProjectCollection FilteredCollection(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return this;
            }
            // todo filter by fields 
            var result = this.Search(text);
            return new CubaseProjectCollection(result);
        }

        private IEnumerable<CubaseProject> Search(string text) 
        {
            var temp = new List<CubaseProject>();   
            foreach (var project in this)
            {
                if (project.Name.Contains(text, StringComparison.OrdinalIgnoreCase) ||
                    project.FolderPath.Contains(text, StringComparison.OrdinalIgnoreCase))
                {
                    temp.Add(project);
                }
            }
            return temp;
        }
    }
}
