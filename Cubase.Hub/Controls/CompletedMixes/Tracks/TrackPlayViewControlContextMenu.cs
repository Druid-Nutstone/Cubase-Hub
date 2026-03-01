using Cubase.Hub.Controls.Album.Manage;
using Cubase.Hub.Controls.Menus;
using Cubase.Hub.Forms.Edit;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Models;
using Cubase.Hub.Services.Track;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Cubase.Hub.Controls.CompletedMixes.Tracks
{
    public class TrackPlayViewControlContextMenu : DarkContextMenu
    {
        private MixDown MixDown;
        private readonly IServiceProvider ServiceProvider;
        private TrackPlayViewControl Parent;

        public TrackPlayViewControlContextMenu(TrackPlayViewControl parent,MixDown mixDown, IServiceProvider serviceProvider) : base()
        {
            this.MixDown = mixDown;
            this.Parent = parent;
            this.ServiceProvider = serviceProvider;
            // hack 
            this.Items.Add(new ToolStripMenuItem("Loading...") { Enabled = false });
        }

        protected override void OnOpening(CancelEventArgs e)
        {
            base.OnOpening(e);
            this.Items.Clear();
            this.Items.Add(new EditMix(this.Parent, this.MixDown, this.ServiceProvider));
            this.Items.Add(new DeleteMix(this.Parent, this.MixDown, this.ServiceProvider));
        }
    }

    public class BaseMixdownMenuItem : ToolStripMenuItem
    {
        protected MixDown MixDown;
        protected IServiceProvider ServiceProvider;
        protected TrackPlayViewControl Parent;
        public BaseMixdownMenuItem(string text, TrackPlayViewControl parent, MixDown mixDown, IServiceProvider serviceProvider) : base(text)
        {
            this.MixDown = mixDown;
            this.Parent = parent;
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

    public class DeleteMix : BaseMixdownMenuItem
    {
        public DeleteMix(TrackPlayViewControl parent, MixDown mixDown, IServiceProvider serviceProvider) : base($"Delete {Path.GetFileNameWithoutExtension(mixDown.FileName)}", parent, mixDown, serviceProvider)
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
        public EditMix(TrackPlayViewControl parent, MixDown mixDown, IServiceProvider serviceProvider) : base($"Edit Mix", parent, mixDown, serviceProvider)
        {

        }

        protected override void OnClick(EventArgs e)
        {
            this.EditTrack.Initialise(this.MixDown.FileName);
            this.EditTrack.ShowDialog();
            this.Parent.Reload();
        }
    }
}
