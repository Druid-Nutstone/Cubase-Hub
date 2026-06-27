using Cubase.Macro.Common.Models;
using Cubase.Macro.Services.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cubase.Macro.Services.Lyrics
{
    public class LyricFileService : ILyricFileService
    {
        private IWindowService windowService;

        public LyricFileService(IWindowService windowService)
        {
            this.windowService = windowService;
        }

        public LyricResponseModel GetLyric(Lyric fileName)
        {
            var targetFile = Path.Combine(CubaseMacroConstants.DropBoxBaseDirectory, fileName.FileName);
            
            if (File.Exists(targetFile)) 
            {
                return this.GetLyricResponseModelFromFile(targetFile);
            }
            return new LyricResponseModel();
        }

        public LyricContent GetLyricContent(Lyric fileName)
        {
            var lyricContent = new LyricContent()
            {
                FileName = fileName.FileName
            };
            
            var targetFile = Path.Combine(CubaseMacroConstants.DropBoxBaseDirectory, fileName.FileName);

            if (File.Exists(targetFile))
            {
                lyricContent.Content = File.ReadAllLines(targetFile);
            }
            return lyricContent;
        }

        public LyricIndexCollection GetLyricIndex()
        {
            var lyricCollection = LyricIndexCollection.DeserialiseFromFile(CubaseMacroConstants.LyricIndexFile);
            return lyricCollection.PopulateLyricFiles(CubaseMacroConstants.DropBoxBaseDirectory);    
        }

        public LyricResponseModel GetProjectCurrentLyrics()
        {
            var projectFileTitle = this.windowService.GetCubaseProjectTitle();
            if (projectFileTitle != null)
            {
                var dropBoxDir = CubaseMacroConstants.DropBoxBaseDirectory;
                var realProjectName = $"{projectFileTitle}.txt";
                var fullProjectLyricFile = System.IO.Path.Combine(dropBoxDir, realProjectName);
                return this.GetLyricResponseModelFromFile(fullProjectLyricFile);
            }
            else
            {
                return LyricResponseModel.CreateWithError("Cannot get the lyric file associated with the current Project");
            }
        }

        private LyricResponseModel GetLyricResponseModelFromFile(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                return LyricResponseModel.Create(Path.GetFileNameWithoutExtension(fileName), System.IO.File.ReadAllLines(fileName));
            }
            else
            {
                return LyricResponseModel.CreateWithError($"Cannot find lyric file associaed with project {fileName}");
            }
        }
    }
}
