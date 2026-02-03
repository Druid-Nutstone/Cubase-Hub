using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.Models
{
    public class CubaseProjectCollection : List<CubaseProject>
    {
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
            var project = CubaseProject.Create(Path.GetFileNameWithoutExtension(cprFileName), cprFileName, Path.GetFileName(parentDir), mixDownFiles);
            this.Add(project);
            return project;
        }
    }
}
