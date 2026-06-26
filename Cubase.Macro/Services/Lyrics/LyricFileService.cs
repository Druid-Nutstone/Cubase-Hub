using Cubase.Macro.Common.Models;
using Cubase.Macro.Services.Window;
using System;
using System.Collections.Generic;
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

        public LyricResponseModel GetProjectCurrentLyrics()
        {
            var projectFileTitle = this.windowService.GetCubaseProjectTitle();
            if (projectFileTitle != null)
            {
                var dropBoxDir = CubaseMacroConstants.DropBoxBaseDirectory;
                var realProjectName = $"{projectFileTitle}.txt";
                var fullProjectLyricFile = System.IO.Path.Combine(dropBoxDir, realProjectName);
                if (System.IO.File.Exists(fullProjectLyricFile))
                {
                    return LyricResponseModel.Create(projectFileTitle, System.IO.File.ReadAllLines(fullProjectLyricFile));
                }
                else
                {
                    return LyricResponseModel.CreateWithError($"Cannot find lyric file associaed with project {projectFileTitle}");
                }
            }
            else
            {
                return LyricResponseModel.CreateWithError("Cannot get the lyric file associated with the current Project");
            }
        }
    }
}
