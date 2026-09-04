using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Models;
using Cubase.Hub.Services.Track;

namespace Cubase.Hub.Forms.Mixes.MixActions
{
    public interface IMixActionControl
    {
        void RunAction(MixDownCollection mixDowns,
                       string targetDirectory,
                       ITrackService trackService,
                       IMessageService messageService,
                       IDirectoryService directoryService,
                       Func<int, string, bool> onProgress);
    }
}
