using Cubase.Hub.Services.Audio;
using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Hub.Forms.Mixes.MixActions
{
    public partial class MixActionCopyControl : UserControl, IMixActionControl
    {
        public MixActionCopyControl()
        {
            InitializeComponent();
        }

        public void RunAction(MixDownCollection mixDowns, 
                              string targetDirectory,
                              IAudioService audioService, 
                              IMessageService messageService, 
                              IDirectoryService directoryService,
                              Func<int, string, bool> onProgress)
        {
            for (var i = 0; i < mixDowns.Count; i++)
            {
                var state = onProgress.Invoke(i + 1, Path.GetFileNameWithoutExtension(mixDowns[i].FileName));
                var targetFile = Path.Combine(targetDirectory, Path.GetFileName(mixDowns[i].FileName));
                File.Copy(mixDowns[i].FileName, targetFile, true);
            }
            onProgress.Invoke(0, "All Done");
        }
    }
}
