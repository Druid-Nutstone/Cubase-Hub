using Cubase.Hub.Controls.Menus;
using Cubase.Hub.Services.Audio;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Controls.Album.Manage
{
    public class MixControlContextMenu : DarkContextMenu
    {
        public MixControlContextMenu(MixDown mixDown, IAudioService audioService, IMessageService messageService) : base()
        {
            this.Items.Add(new ConvertToMp3MenuItem(mixDown, audioService, messageService));
            this.Items.Add(new ConvertToFlacMenuItem(mixDown, audioService, messageService));
        }
    }

    public class BaseMixdownMenuItem : ToolStripMenuItem
    {
        protected MixDown MixDown;
        protected IAudioService AudioService;
        protected IMessageService MessageService;
        public BaseMixdownMenuItem(string text, MixDown mixDown, IAudioService audioService, IMessageService messageService) : base(text)
        {
            this.MixDown = mixDown;
            this.AudioService = audioService;
            this.MessageService = messageService;
        }
    }

    public class ConvertToMp3MenuItem : BaseMixdownMenuItem
    {
        public ConvertToMp3MenuItem(MixDown mixDown, IAudioService audioService, IMessageService messageService) : base("Convert to MP3", mixDown, audioService, messageService)
        {

        }

        protected override void OnClick(EventArgs e)
        {

        }
    }

    public class ConvertToFlacMenuItem : BaseMixdownMenuItem
    {
        public ConvertToFlacMenuItem(MixDown mixDown, IAudioService audioService, IMessageService messageService) : base("Convert to Flac", mixDown, audioService, messageService)
        {

        }

        protected override void OnClick(EventArgs e)
        {

        }
    }

}
