using Cubase.Hub.Forms.Albums;
using Cubase.Hub.Forms.CompletedMixes;
using Cubase.Hub.Forms.Config;
using Cubase.Hub.Forms.Tracks;
using Cubase.Hub.Services;
using Cubase.Hub.Services.Config;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Cubase.Hub.Forms.Main.Menu
{
    public class MenuContent
    {
        private readonly IServiceProvider serviceProvider;

        public MenuContent(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider; 
        }
        
        public void Initialise(MenuStrip menuStrip, MainForm mainForm)
        {
            menuStrip.Items.Clear();
            menuStrip.Items.Add(new FileMenu(mainForm, this.serviceProvider));
            menuStrip.Items.Add(new AlbumMenu(mainForm, this.serviceProvider));
            menuStrip.Items.Add(new TrackMenu(mainForm, this.serviceProvider));
            menuStrip.Items.Add(new OptionsMenu(mainForm, this.serviceProvider));
            menuStrip.Items.Add(new PlayMenu(mainForm, this.serviceProvider));
        }
    }

    public class BaseToolStripMenuItem : ToolStripMenuItem
    {
        protected MainForm MainForm; 
        
        protected IServiceProvider ServiceProvider;

        public BaseToolStripMenuItem(MainForm mainForm, IServiceProvider serviceProvider)
        {
            this.MainForm = mainForm;
            this.ServiceProvider = serviceProvider;
        }
    }

    public class OptionsMenu : BaseToolStripMenuItem
    {
        public OptionsMenu(MainForm mainForm, IServiceProvider serviceProvider) : base(mainForm, serviceProvider)
        {
            this.Text = "Options";
            this.DropDownItems.Add(new OptionsMenuEdit(mainForm, this.ServiceProvider));
            this.DropDownItems.Add(new OptionsMenuOpen(mainForm, this.ServiceProvider));
        }
    }

    public class PlayMenu : BaseToolStripMenuItem
    {
        public PlayMenu(MainForm mainForm, IServiceProvider serviceProvider) : base(mainForm, serviceProvider)
        {
            this.Text = "Play and Deploy";
            this.DropDownItems.Add(new PlayMixesMenu(mainForm, this.ServiceProvider));
        }
    }

    public class PlayMixesMenu : BaseToolStripMenuItem
    {
        public PlayMixesMenu(MainForm mainForm, IServiceProvider serviceProvider) : base(mainForm, serviceProvider)
        {
            this.Text = "Mixes";
        }

        protected override void OnClick(EventArgs e)
        {
            var mixesForm = this.ServiceProvider.GetService<CompletedMixesForm>();
            mixesForm.InitialiseMixes();
            mixesForm?.ShowDialog();
        }
    }



    public class OptionsMenuEdit : BaseToolStripMenuItem
    {
        public OptionsMenuEdit(MainForm mainForm, IServiceProvider serviceProvider) : base(mainForm, serviceProvider)
        {
            this.Text = "Edit";
        }

        protected override void OnClick(EventArgs e)
        {
            var configForm = this.ServiceProvider.GetService<ConfigurationForm>();
            configForm?.ShowDialog();
        }
    }

    public class OptionsMenuOpen : BaseToolStripMenuItem
    {
        public OptionsMenuOpen(MainForm mainForm, IServiceProvider serviceProvider) : base(mainForm, serviceProvider)
        {
            this.Text = "Open (File)";
        }

        protected override void OnClick(EventArgs e)
        {
            var p = new Process() { StartInfo = new ProcessStartInfo() { Arguments = CubaseHubConstants.ConfigurationFileName, FileName = "notepad.exe", UseShellExecute = true } };
            p.Start();
        }
    }



    public class AlbumMenu : BaseToolStripMenuItem 
    {
        public AlbumMenu(MainForm mainForm, IServiceProvider serviceProvider) : base(mainForm, serviceProvider)
        {
            this.Text = "Albums";
            this.DropDownItems.Add(new NewAlbumMenu(mainForm, serviceProvider));
            this.DropDownItems.Add(new ManageAlbumMenu(mainForm, serviceProvider));
        }


    }

    public class NewAlbumMenu : BaseToolStripMenuItem
    {
        public NewAlbumMenu(MainForm mainForm, IServiceProvider serviceProvider) : base(mainForm, serviceProvider)
        {
            this.Text = "New Album";
        }

        protected override void OnClick(EventArgs e)
        {
            var newAlbumForm = this.ServiceProvider.GetService<NewAlbumForm>(); 
            newAlbumForm?.ShowDialog();
        }
    }

    public class ManageAlbumMenu : BaseToolStripMenuItem
    {
        public ManageAlbumMenu(MainForm mainForm, IServiceProvider serviceProvider) : base(mainForm, serviceProvider)
        {
            this.Text = "Manage Albums";
        }

        protected override void OnClick(EventArgs e)
        {
            var manageAlbumForm = this.ServiceProvider.GetService<ManageAlbumsForm>();
            manageAlbumForm?.Initialise();
            manageAlbumForm?.ShowDialog();
        }
    }

    public class NewTrackMenu : BaseToolStripMenuItem
    {
        public NewTrackMenu(MainForm mainForm, IServiceProvider serviceProvider) : base(mainForm, serviceProvider)
        {
            this.Text = "New Track";
        }

        protected override void OnClick(EventArgs e)
        {
            var newTrackForm = this.ServiceProvider.GetService<NewTrackForm>();
            newTrackForm?.Initialise();
            newTrackForm?.ShowDialog();
        }
    }

    public class TrackMenu : BaseToolStripMenuItem
    {
        public TrackMenu(MainForm mainForm, IServiceProvider serviceProvider) : base(mainForm, serviceProvider)
        {
            this.Text = "Tracks";
            this.DropDownItems.Add(new NewTrackMenu(mainForm, serviceProvider));

        }
    }

    public class FileMenu : BaseToolStripMenuItem
    {
        public FileMenu(MainForm mainForm, IServiceProvider serviceProvider) : base(mainForm, serviceProvider)
        {
            this.Text = "File"; 
            this.DropDownItems.Add(new FileMenuExit(mainForm, serviceProvider));
        }
    }

    public class FileMenuExit : BaseToolStripMenuItem
    {
        public FileMenuExit(MainForm mainForm, IServiceProvider serviceProvider) : base(mainForm, serviceProvider)
        {
            this.Text = "Exit";
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Application.Exit();
        }
    }
}
