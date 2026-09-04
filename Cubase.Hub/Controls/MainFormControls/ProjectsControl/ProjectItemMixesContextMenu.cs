using Cubase.Hub.Controls.Menus;
using Cubase.Hub.Services.Audio;
using Cubase.Hub.Services.Messages;

namespace Cubase.Hub.Controls.MainFormControls.ProjectsControl
{
    public class ProjectItemMixesContextMenu : DarkContextMenu
    {
        private readonly IAudioService audioService;

        private readonly IMessageService messageService;

        public ProjectItemMixesContextMenu(IAudioService audioService, IMessageService messageService) : base()
        {
            this.audioService = audioService;
        }
    }
}
