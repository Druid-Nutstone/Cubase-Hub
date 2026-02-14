using Cubase.Hub.Controls.Menus;
using Cubase.Hub.Services.Audio;
using Cubase.Hub.Services.Messages;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

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
