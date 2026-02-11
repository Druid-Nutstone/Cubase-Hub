using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.Models
{
    public class CubaseProject
    {
        public string FullPath { get; set; }

        public string Name { get; set; }

        public string FolderPath { get; set; }  

        public string Album {  get; set; }

        public string AlbumPath { get; set; }

        public DateTime LastModified { get; set; }

        public List<string> Mixes { get; set; } = new List<string>();

        public static CubaseProject Create(string name, string fullPath, string folderPath, string albumName, string albumPath,  List<string>? mixes = null)
        {
            var lastModified = System.IO.File.GetLastWriteTime(fullPath);

            return new CubaseProject
            {
                Name = name,
                FullPath = fullPath,
                Album = albumName,
                AlbumPath = albumPath,
                FolderPath = folderPath,
                LastModified = lastModified,
                Mixes = mixes ?? new List<string>()
            };
        }

    }
}
