using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.Models
{
    public class MixDownCollection : List<MixDown>
    {
        public void CreateFromFiles(string[] files)
        {
            foreach(var file in files)
            {
                this.Add(MixDown.CreateFromFile(file));
            }
        }
    }

    public class MixDown()
    {
        public string FileName { get; set; }    

        public string ParentDirectory { get; set; }   
    
        // ID tags 
        
        public string Title { get; set; }   

        public string Album { get; set; } 
        
        public string Genre { get; set; }   

        public string Duration { get; set; }

        public string Size { get; set; }

        public uint TrackNumber { get; set; }

        public string AudioType { get; set; }

        public uint Year { get; set; }   

        public string Artist { get; set; }  

        public string Performers { get; set; }  

        public string Comment { get; set; }

        public static MixDown CreateFromFile(string file)
        {
            return new MixDown() { FileName = file, ParentDirectory = Path.GetFileName(Directory.GetParent(file).FullName) };
        }
    }
}
