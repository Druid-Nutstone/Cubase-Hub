using Cubase.Hub.Controls.Menus;
using Cubase.Hub.Services.Audio;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Cubase.Hub.Controls.Album.Manage
{
    public class MixControlContextMenu : DarkContextMenu
    {
        private MixDown MixDown;
        private IAudioService AudioService;
        private IMessageService MessageService;


        public MixControlContextMenu(MixDown mixDown, IAudioService audioService, IMessageService messageService) : base()
        {
            this.MixDown = mixDown; 
            this.AudioService = audioService;
            this.MessageService = messageService;
            // hack 
            this.Items.Add(new ToolStripMenuItem("Loading...") { Enabled = false });    
        }

        protected override void OnOpening(CancelEventArgs e)
        {
            base.OnOpening(e);
            this.Items.Clear();
            if (MixDown.AudioType.ToLower() == "wav")
            {
                this.Items.Add(new ConvertToMp3MenuItem(MixDown, AudioService, MessageService));
                this.Items.Add(new ConvertToFlacMenuItem(MixDown, AudioService, MessageService));
            }
            this.Items.Add(new SetTitleFromFileName(MixDown, AudioService, MessageService));
            if (!string.IsNullOrEmpty(MixDown.ExportLocation))
            {
                this.Items.Add(new CopyToExportDirectory(MixDown, AudioService, MessageService));
            }
            this.Items.Add(new DeleteMix(MixDown, AudioService, MessageService));
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

    public class DeleteMix : BaseMixdownMenuItem
    {
        public DeleteMix(MixDown mixDown, IAudioService audioService, IMessageService messageService) : base($"Delete {Path.GetFileNameWithoutExtension(mixDown.FileName)}", mixDown, audioService, messageService)
        {

        }

        protected override void OnClick(EventArgs e)
        {
            File.Delete(this.MixDown.FileName);
            AlbumCommands.Instance.RefreshTracks();
        }
    }

    public class SetTitleFromFileName : BaseMixdownMenuItem
    {
        public SetTitleFromFileName(MixDown mixDown, IAudioService audioService, IMessageService messageService) : base($"Set title to {Path.GetFileNameWithoutExtension(mixDown.FileName)}", mixDown, audioService, messageService)
        {

        }

        protected override void OnClick(EventArgs e)
        {
            this.MixDown.Title = Path.GetFileNameWithoutExtension(this.MixDown.FileName);
            this.AudioService.PopulateMixdownFromTags(this.MixDown);
            AlbumCommands.Instance.RefreshTracks();
        }
    }

    public class CopyToExportDirectory : BaseMixdownMenuItem
    {
        public CopyToExportDirectory(MixDown mixDown, IAudioService audioService, IMessageService messageService) : base($"Copy to export directory", mixDown, audioService, messageService)
        {

        }

        protected override void OnClick(EventArgs e)
        {
            var msg = this.MessageService.OpenMessage($"Copying {this.MixDown.Title} to export directory...", this.Parent);
            var target = Path.Combine(this.MixDown.ExportLocation,Path.GetFileName(this.MixDown.FileName)); 
            File.Copy(this.MixDown.FileName, target, true); 
            msg.Close();
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
