using Cubase.Hub.Controls.Menus;
using Cubase.Hub.Forms.Edit;
using Cubase.Hub.Services.Audio;
using Cubase.Hub.Services.Distributers;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Models;
using Cubase.Hub.Services.Track;
using Microsoft.Extensions.DependencyInjection;
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
        private IServiceProvider ServiceProvider; 


        public MixControlContextMenu(MixDown mixDown, IServiceProvider serviceProvider) : base()
        {
            this.MixDown = mixDown; 
            this.ServiceProvider = serviceProvider;
            // hack 
            this.Items.Add(new ToolStripMenuItem("Loading...") { Enabled = false });    
        }

        protected override void OnOpening(CancelEventArgs e)
        {
            base.OnOpening(e);
            this.Items.Clear();
            this.Items.Add(new EditMix(MixDown, this.ServiceProvider));
            if (MixDown.AudioType.ToLower() == "wav")
            {
                this.Items.Add(new DistributerMenu(MixDown, this.ServiceProvider));
                this.Items.Add(new ConvertToMp3MenuItem(MixDown, this.ServiceProvider));
                this.Items.Add(new ConvertToFlacMenuItem(MixDown, this.ServiceProvider));
            }
            this.Items.Add(new SetTitleFromFileName(MixDown, this.ServiceProvider));
            if (!string.IsNullOrEmpty(MixDown.ExportLocation))
            {
                this.Items.Add(new CopyToExportDirectory(MixDown, this.ServiceProvider));
            }
            this.Items.Add(new DeleteMix(MixDown, this.ServiceProvider));
        }
    }

    public class BaseMixdownMenuItem : ToolStripMenuItem
    {
        protected MixDown MixDown;
        protected IServiceProvider ServiceProvider;
        public BaseMixdownMenuItem(string text, MixDown mixDown, IServiceProvider serviceProvider) : base(text)
        {
            this.MixDown = mixDown;
            this.ServiceProvider = serviceProvider;
        }

        public override Size GetPreferredSize(Size constrainingSize)
        {
            var size = base.GetPreferredSize(constrainingSize);
            size.Height = 50; // your desired row height
            return size;
        }

        protected ITrackService TrackService => this.ServiceProvider.GetService<ITrackService>();

        protected IMessageService MessageService => this.ServiceProvider.GetService<IMessageService>();

        protected EditTrackForm EditTrack => this.ServiceProvider.GetService<EditTrackForm>();
    
    
    }

    public class DistributerMenu : BaseMixdownMenuItem
    {
        public DistributerMenu(MixDown mixDown, IServiceProvider serviceProvider) : base($"Convert For Distributer", mixDown, serviceProvider)
        {
            foreach (var distro in Distributers.DistroNames)
            {
                this.DropDownItems.Add(new DistroMenu(distro, mixDown, serviceProvider));
            }
        }
    }

    public class DistroMenu : BaseMixdownMenuItem
    {
        public DistroMenu(string name, MixDown mixdown, IServiceProvider serviceProvider) : base(name, mixdown, serviceProvider)
        {

        }

        protected override void OnClick(EventArgs e)
        {
            var distroProvider  = this.ServiceProvider.GetKeyedService<IDistributer>(this.Text);
            var msgHandler = this.MessageService.OpenMessage($"Creating distribution release for {this.Text} for audio file {Path.GetFileName(this.MixDown.FileName)}", this.Parent);
            var state = distroProvider?.Distribute(this.MixDown, (err) => 
            {
                msgHandler.Close();
                this.MessageService.ShowError($"Could not create a distribution release {err}");
            });
            if (state.Value)
            {
                msgHandler.Close();
            }
        }
    }

    public class DeleteMix : BaseMixdownMenuItem
    {
        public DeleteMix(MixDown mixDown, IServiceProvider serviceProvider) : base($"Delete {Path.GetFileNameWithoutExtension(mixDown.FileName)}", mixDown, serviceProvider)
        {

        }

        protected override void OnClick(EventArgs e)
        {
            File.Delete(this.MixDown.FileName);
            AlbumCommands.Instance.RefreshTracks();
        }
    }

    public class EditMix : BaseMixdownMenuItem
    {
        public EditMix(MixDown mixDown, IServiceProvider serviceProvider) : base($"Edit Mix", mixDown, serviceProvider)
        {

        }

        protected override void OnClick(EventArgs e)
        {
            this.EditTrack.Initialise(this.MixDown.FileName);
            this.EditTrack.ShowDialog();
        }
    }

    public class SetTitleFromFileName : BaseMixdownMenuItem
    {
        public SetTitleFromFileName(MixDown mixDown, IServiceProvider serviceProvider) : base($"Set title to {Path.GetFileNameWithoutExtension(mixDown.FileName)}", mixDown, serviceProvider)
        {

        }

        protected override void OnClick(EventArgs e)
        {
            this.MixDown.Title = Path.GetFileNameWithoutExtension(this.MixDown.FileName);
            this.TrackService.PopulateMixdownFromTags(this.MixDown);
            AlbumCommands.Instance.RefreshTracks();
        }
    }

    public class CopyToExportDirectory : BaseMixdownMenuItem
    {
        public CopyToExportDirectory(MixDown mixDown, IServiceProvider serviceProvider) : base($"Copy to export directory", mixDown, serviceProvider)
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
        public ConvertToMp3MenuItem(MixDown mixDown, IServiceProvider serviceProvider) : base("Convert to MP3", mixDown, serviceProvider)
        {

        }

        protected override void OnClick(EventArgs e)
        {
            var msgBox = this.MessageService.OpenMessage($"Converting {this.MixDown.Title} to MP3...", this.Parent);
            this.TrackService.ConvertToMp3(this.MixDown, Path.GetDirectoryName(this.MixDown.FileName), FFMpegCore.Enums.AudioQuality.Ultra);
            AlbumCommands.Instance.RefreshTracks();
            msgBox.Close();
        }
    }

    public class ConvertToFlacMenuItem : BaseMixdownMenuItem
    {
        public ConvertToFlacMenuItem(MixDown mixDown, IServiceProvider serviceProvider) : base("Convert to Flac", mixDown, serviceProvider)
        {

        }

        protected override void OnClick(EventArgs e)
        {
            var msgBox = this.MessageService.OpenMessage($"Converting {this.MixDown.Title} to FLAC...", this.Parent);
            this.TrackService.ConvertToFlac(this.MixDown, Path.GetDirectoryName(this.MixDown.FileName), new FlacConfiguration());
            AlbumCommands.Instance.RefreshTracks();
            msgBox.Close();
        }
    }

}
