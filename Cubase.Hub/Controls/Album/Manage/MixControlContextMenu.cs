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

        public override Size GetPreferredSize(Size constrainingSize)
        {
            var size = base.GetPreferredSize(constrainingSize);
            size.Height = 50; // your desired row height
            return size;
        }
    }

    public class ConvertToMp3MenuItem : BaseMixdownMenuItem
    {
        public ConvertToMp3MenuItem(MixDown mixDown, IAudioService audioService, IMessageService messageService) : base("Convert to MP3", mixDown, audioService, messageService)
        {

        }

        protected override void OnClick(EventArgs e)
        {
            var msgBox = this.MessageService.OpenMessage($"Converting {this.MixDown.Title} to MP3...", this.Parent);
            this.AudioService.ConvertToMp3(this.MixDown, Path.GetDirectoryName(this.MixDown.FileName), FFMpegCore.Enums.AudioQuality.Ultra);
            AlbumCommands.Instance.RefreshTracks();
            msgBox.Close();
        }
    }

    public class ConvertToFlacMenuItem : BaseMixdownMenuItem
    {
        public ConvertToFlacMenuItem(MixDown mixDown, IAudioService audioService, IMessageService messageService) : base("Convert to Flac", mixDown, audioService, messageService)
        {

        }

        protected override void OnClick(EventArgs e)
        {
            var msgBox = this.MessageService.OpenMessage($"Converting {this.MixDown.Title} to FLAC...", this.Parent);
            this.AudioService.ConvertToFlac(this.MixDown, Path.GetDirectoryName(this.MixDown.FileName), CompressionLevel.Default);
            AlbumCommands.Instance.RefreshTracks();
            msgBox.Close();
        }
    }

}
