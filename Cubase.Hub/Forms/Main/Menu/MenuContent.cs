using Cubase.Hub.Forms.Config;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
            menuStrip.Items.Add(new ProjectMenu(mainForm, this.serviceProvider));
            menuStrip.Items.Add(new OptionsMenu(mainForm, this.serviceProvider));
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
            // Add album related menu items here
        }

        protected override void OnClick(EventArgs e)
        {
            var configForm = this.ServiceProvider.GetService<ConfigurationForm>();
            configForm?.ShowDialog();
        }
    }

    public class AlbumMenu : BaseToolStripMenuItem 
    {
        public AlbumMenu(MainForm mainForm, IServiceProvider serviceProvider) : base(mainForm, serviceProvider)
        {
            this.Text = "Album";
            this.DropDownItems.Add(new NewAlbumMenu(mainForm, serviceProvider));
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

    public class ProjectMenu : BaseToolStripMenuItem
    {
        public ProjectMenu(MainForm mainForm, IServiceProvider serviceProvider) : base(mainForm, serviceProvider)
        {
            this.Text = "Project";
            // Add album related menu items here
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
