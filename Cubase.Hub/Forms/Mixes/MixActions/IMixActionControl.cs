using Cubase.Hub.Services.Audio;
using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Forms.Mixes.MixActions
{
    public interface IMixActionControl
    {
        void RunAction(MixDownCollection mixDowns,
                       string targetDirectory, 
                       IAudioService audioService,
                       IMessageService messageService,
                       IDirectoryService directoryService,
                       Func<int, string, bool> onProgress);
    }
}
