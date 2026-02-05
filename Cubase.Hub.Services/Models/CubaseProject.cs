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

        public List<string> Mixes { get; set; } = new List<string>();

        public static CubaseProject Create(string name, string fullPath, string folderPath, string albumName, string albumPath,  List<string>? mixes = null)
        {
            return new CubaseProject
            {
                Name = name,
                FullPath = fullPath,
                Album = albumName,
                AlbumPath = albumPath,
                FolderPath = folderPath,
                Mixes = mixes ?? new List<string>()
            };
        }

    }
}
