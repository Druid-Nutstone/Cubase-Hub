using Cubase.Hub.Controls.CompletedMixes;
using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Services.Album;
using Cubase.Hub.Services.Config;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Models;
using Cubase.Hub.Services.Track;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Hub.Forms.CompletedMixes
{
    public partial class CompletedMixesForm : BaseWindows11Form
    {
        private readonly ITrackService trackService;
        
        private readonly IConfigurationService configurationService;

        private readonly IAlbumService albumService;

        private readonly IMessageService messageService;

        private readonly IServiceProvider serviceProvider;

        public CompletedMixesForm()
        {
            InitializeComponent();
        }

        public CompletedMixesForm(ITrackService trackService,
                                  IAlbumService albumService,
                                  IMessageService messageService,
                                  IServiceProvider serviceProvider,
                                  IConfigurationService configurationService)
        {
            InitializeComponent();
            this.trackService = trackService;
            this.albumService = albumService;
            this.messageService = messageService;
            this.serviceProvider = serviceProvider;
            this.configurationService = configurationService;
            ThemeApplier.ApplyDarkTheme(this);
        }

        public void InitialiseMixes()
        {
            this.LoadPosition();
            this.LoadCompletedMixes();
            ThemeApplier.ApplyDarkTheme(this);
        }

        private void LoadCompletedMixes()
        {
            this.Cursor = Cursors.WaitCursor;
            var msgHandler = this.messageService.OpenMessage("Loading Albums ..", this);
            var treeMenu = new CompletedMixesMenu(this.serviceProvider);
            treeMenu.OnAlbumSelected = this.LoadAlbumData;
            treeMenu.Dock = DockStyle.Fill;
            this.MenuTreePanel.Controls.Clear();
            this.MenuTreePanel.Controls.Add(treeMenu);
            treeMenu.LoadAlbums();
            msgHandler.Close();
            this.Cursor = Cursors.Default;
        }

        private void LoadAlbumData(AlbumLocation albumLocation)
        {
            this.trackService.StopTrack(); // stop any existing tracks playing !!
            this.DataPanel.Controls.Clear();
            var albumPlayer = new AlbumPlayerControl(this.serviceProvider);
            albumPlayer.Dock = DockStyle.Fill;
            this.DataPanel.Controls.Add(albumPlayer);
            albumPlayer.Play(albumLocation);
        }

        private void LoadPosition()
        {
            if (!this.configurationService.IsLoaded)
            {
                this.configurationService.LoadConfiguration(() => 
                {
                    this.messageService.ShowError("Cannot load configuration");        
                }); 
            } 
            if (this.configurationService.Configuration.PlayWindowLocation != null)
            {
                if (this.configurationService.Configuration.PlayWindowLocation.isMaximised)
                {
                    this.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    StartPosition = FormStartPosition.Manual;
                    Bounds = new Rectangle(
                        this.configurationService.Configuration.PlayWindowLocation.X,
                        this.configurationService.Configuration.PlayWindowLocation.Y,
                        this.configurationService.Configuration.PlayWindowLocation.Width,
                        this.configurationService.Configuration.PlayWindowLocation.Height);
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            var bounds = WindowState == FormWindowState.Normal
            ? Bounds
            : RestoreBounds;

            var settings = new WindowSettings
            {
                X = bounds.X,
                Y = bounds.Y,
                Width = bounds.Width,
                Height = bounds.Height,
            };

            settings.isMaximised = this.WindowState == FormWindowState.Maximized;
            this.configurationService?.Configuration?.PlayWindowLocation = settings;
            this.configurationService?.SaveConfiguration((err) =>
            {
                this.messageService.ShowError($"Could not save configuration file. {err}");
            });
        }



    }
}
