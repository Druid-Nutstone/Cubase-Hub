using Cubase.Hub.Controls.Album.Manage;
using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Forms.CompletedMixes;
using Cubase.Hub.Forms.Message;
using Cubase.Hub.Forms.Mixes;
using Cubase.Hub.Services;
using Cubase.Hub.Services.Album;
using Cubase.Hub.Services.Audio;
using Cubase.Hub.Services.Config;
using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Models;
using Cubase.Hub.Services.Projects;
using Cubase.Hub.Services.Track;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Hub.Forms.Albums
{
    public partial class ManageAlbumsForm : BaseWindows11Form
    {

        private readonly IConfigurationService configurationService;

        private readonly IProjectService projectService;

        private readonly IDirectoryService directoryService;

        private readonly ITrackService trackService;

        private readonly IMessageService messageService;

        private readonly IServiceProvider serviceProvider;

        private readonly IAlbumService albumService;

        private readonly CompletedMixesForm completedMixesForm;


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AlbumLocation CurrentAlbum { get; set; }

        private MixDownCollection CurrentMixes;

        private AlbumConfiguration CurrentAlbumConfiguration;

        private ManageMixesForm mixesForm;

        private NonBlockingMessage? nonBlockingMessage;

        public ManageAlbumsForm(IConfigurationService configurationService,
                                IDirectoryService directoryService,
                                ITrackService trackService,
                                IAlbumService albumService,
                                IMessageService messageService,
                                IServiceProvider serviceProvider,
                                CompletedMixesForm completedMixesForm,
                                ManageMixesForm manageMixesForm,
                                IProjectService projectService)
        {
            InitializeComponent();
            this.configurationService = configurationService;
            this.directoryService = directoryService;
            this.trackService = trackService;
            this.albumService = albumService;
            this.completedMixesForm = completedMixesForm;
            this.projectService = projectService;
            this.messageService = messageService;
            this.mixesForm = manageMixesForm;
            this.serviceProvider = serviceProvider;
            ThemeApplier.ApplyDarkTheme(this);
            this.SelectedAlbum.SelectedIndexChanged += SelectedAlbum_SelectedIndexChanged;
            this.RereshFromAblumButton.Click += RereshFromAblumButton_Click;
            this.OpenMixes.Click += OpenMixes_Click;
            this.OpenAlbumDirectory.Click += OpenAlbumDirectory_Click;
            this.DeleteSelectedButton.BackColor = Color.FromKnownColor(KnownColor.IndianRed);
            this.DeleteSelectedButton.Click += DeleteSelectedButton_Click;
            this.DisableButtonsThatNeedASelection();
            this.ManageMixesButton.Click += ManageMixesButton_Click;
            this.SelectDeselectAllMixes.Text = "Select All";
            this.SelectDeselectAllMixes.CheckedChanged += SelectDeselectAllMixes_CheckedChanged;
            this.SetSelectedTracksTitleButton.Click += SetSelectedTracksTitleButton_Click;
            this.BrowseExportLocationButton.Click += BrowseExportLocationButton_Click;
            this.OpenExportDirectory.Click += OpenExportDirectory_Click;
            this.RefreshTracksButton.Click += RefreshTracksButton_Click;
            this.OrderBy.Items.Clear();
            this.OrderBy.Items.AddRange(["Track", "Date", "Type", "Duration", "Size"]);
            this.OrderBy.SelectedIndex = 0;
            this.OrderBy.SelectedIndexChanged += OrderBy_SelectedIndexChanged;
            this.AutoScaleMode = AutoScaleMode.Dpi;
            // register static event class and wait for commands comming in 
            AlbumCommands.Instance.RegisterForAlbumCommand(this.OnAlbumCommandReceived);
            this.PlayTrack.TrackService = this.trackService;
            this.PlayTrack.ShowPlay = false;
        }

        private void OpenMixes_Click(object? sender, EventArgs e)
        {
            this.completedMixesForm.InitialiseMixes(this.CurrentAlbum);
            this.completedMixesForm.ShowDialog();
        }

        private void OrderBy_SelectedIndexChanged(object? sender, EventArgs e)
        {
            switch (this.OrderBy.SelectedItem.ToString())
            {
                case "Track":
                    this.CurrentMixes = this.CurrentMixes.OrderByTrack();
                    break;
                case "Date":
                    this.CurrentMixes = this.CurrentMixes.OrderByDate();
                    break;
                case "Type":
                    this.CurrentMixes = this.CurrentMixes.OrderByType();
                    break;
                case "Duration":
                    this.CurrentMixes = this.CurrentMixes.OrderByDuration();
                    break;
                case "Size":
                    this.CurrentMixes = this.CurrentMixes.OrderBySize();
                    break;

            }
            this.ShowMixes();
        }

        private void RefreshTracksButton_Click(object? sender, EventArgs e)
        {
            this.LoadTracks();
        }

        private void OpenExportDirectory_Click(object? sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.AlbumExportLocation.Text))
            {
                this.directoryService.OpenExplorer(this.AlbumExportLocation.Text);
            }
        }

        private void BrowseExportLocationButton_Click(object? sender, EventArgs e)
        {
            var folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = "Select Album Export Location";
            folderBrowser.UseDescriptionForTitle = true;
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                this.AlbumExportLocation.Text = folderBrowser.SelectedPath;
                var albumExportConfig = this.configurationService?.Configuration?.AlbumExports?.FirstOrDefault(x => x.Name.Equals(this.CurrentAlbum.AlbumName));
                if (albumExportConfig == null)
                {
                    albumExportConfig = new AlbumExport { Name = this.CurrentAlbum.AlbumName, Location = folderBrowser.SelectedPath };
                    this.configurationService.Configuration.AlbumExports.Add(albumExportConfig);
                }
                else
                {
                    albumExportConfig.Location = folderBrowser.SelectedPath;
                }
                this.configurationService.SaveConfiguration((err) =>
                {
                    this.messageService.ShowError($"Could not save configuration file. {err}");
                });
                this.SetMixExportLocation(this.AlbumExportLocation.Text);
                this.albumService.InitialiseAlbumArt(this.AlbumExportLocation.Text);
            }
        }

        private void OnAlbumCommandReceived(AlbumCommandType command)
        {
            switch (command)
            {
                case AlbumCommandType.RefreshTracks:
                    if (this.CurrentAlbum != null)
                    {
                        this.LoadTracks();
                    }
                    break;
            }
        }

        private void ManageMixesButton_Click(object? sender, EventArgs e)
        {
            this.mixesForm.Initialise(this.CurrentMixes.GetSelectedMixes(), this.CurrentAlbum);
            this.mixesForm.ShowDialog();
        }

        private void DeleteSelectedButton_Click(object? sender, EventArgs e)
        {
            if (this.CurrentMixes != null)
            {
                var mixesToDelete = this.CurrentMixes.GetSelectedMixes();
                var yesNo = this.messageService.AskMessage("Are you sure you want to delete the selected mixes?");
                if (yesNo == DialogResult.Yes)
                {
                    foreach (var mix in mixesToDelete)
                    {
                        try
                        {
                            File.Delete(mix.FileName);
                        }
                        catch (Exception ex)
                        {
                            this.messageService.ShowError($"Could not delete {mix.FileName}{Environment.NewLine} {ex.Message}");
                            return;
                        }
                    }
                }
                this.CurrentMixes.RemoveSelectedMixes();
                this.ShowMixes();
                this.SetSelectionButtonsState();
            }
        }

        private void SetSelectedTracksTitleButton_Click(object? sender, EventArgs e)
        {
            if (this.CurrentMixes != null)
            {
                foreach (var mix in this.CurrentMixes)
                {
                    mix.Title = Path.GetFileNameWithoutExtension(mix.FileName);
                    this.trackService.SetTagsFromMixDown(mix);
                }
                this.ShowMixes();
            }
        }

        private void SelectDeselectAllMixes_CheckedChanged(object? sender, EventArgs e)
        {
            if (this.CurrentMixes != null)
            {
                this.CurrentMixes.SelectDeSelectMixes(this.SelectDeselectAllMixes.Checked);
            }
        }

        private void DisableButtonsThatNeedASelection()
        {
            this.ManageMixesButton.Enabled = false;
            this.DeleteSelectedButton.Enabled = false;
            this.SetSelectedTracksTitleButton.Enabled = false;
        }

        private void EnableButtonsThatNeedASelection()
        {
            this.ManageMixesButton.Enabled = true;
            this.DeleteSelectedButton.Enabled = true;
            this.SetSelectedTracksTitleButton.Enabled = true;
        }

        private void RereshFromAblumButton_Click(object? sender, EventArgs e)
        {
            if (this.CurrentAlbumConfiguration != null && this.CurrentMixes != null)
            {
                foreach (var mix in this.CurrentMixes)
                {
                    mix.Album = this.CurrentAlbumConfiguration.Title;
                    mix.Year = this.CurrentAlbumConfiguration.Year;
                    mix.Artist = CurrentAlbumConfiguration.Artist;
                    mix.Genre = CurrentAlbumConfiguration.Genre;
                    this.trackService.SetTagsFromMixDown(mix);
                }
                this.LoadTracks();
            }
        }

        private void OpenAlbumDirectory_Click(object? sender, EventArgs e)
        {
            if (this.CurrentAlbum != null)
            {
                this.directoryService.OpenExplorer(this.CurrentAlbum.AlbumPath);
            }
        }

        private void SelectedAlbum_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (this.SelectedAlbum.SelectedIndex > -1)
            {
                this.CurrentAlbum = this.SelectedAlbum.SelectedItem as AlbumLocation;
                // get album
                this.CurrentAlbumConfiguration = this.albumService.GetAlbumConfigurationFromAlbumLocation(this.CurrentAlbum);
                this.CurrentAlbumConfiguration.DistributionChanged = this.DistributionChanged;
                this.AlbumConfigurationControl.AlbumConfiguration = this.CurrentAlbumConfiguration;
                this.AlbumConfigurationControl.Initialise(this.OnAlbumChanged);
                // get album mixes 
                if (this.nonBlockingMessage != null)
                {
                    this.nonBlockingMessage = this.messageService.OpenMessage("Loading Tracks..", this);
                }
                this.LoadTracks();
                this.nonBlockingMessage?.Close();
                this.nonBlockingMessage = null;

            }
            else
            {
                this.AlbumConfigurationControl.AlbumConfiguration = new AlbumConfiguration();
                this.AlbumConfigurationControl.Initialise();
                this.mixdownControl.ShowMixes(new MixDownCollection(), this.OnMixChanged, this.OnPlayMix, this.trackService, this.messageService, this.serviceProvider);
            }
        }

        private void DistributionChanged(MixDown mixDown)
        {
            if (this.nonBlockingMessage == null)
            {
                this.nonBlockingMessage = this.messageService.OpenMessage($"Copying {mixDown.Title} to distribution directory ", this);
            }
            File.Copy(mixDown.FileName, Path.Combine(this.AlbumExportLocation.Text, Path.GetFileName(mixDown.FileName)), true);
            this.nonBlockingMessage.Close();
        }



        private void InitialiseAlbumExportLocation()
        {
            var albumLocation = this.albumService.AlbumExportLocation(this.CurrentAlbum);
            if (string.IsNullOrEmpty(albumLocation))
            {
                albumLocation = this.albumService.InitialiseAlbumExportLocation(this.CurrentAlbum, (err) => { });
                if (albumLocation != null)
                {
                    this.SetMixExportLocation(albumLocation);
                }
            }
            this.AlbumExportLocation.Text = albumLocation ?? null;
        }

        private void LoadTracks()
        {
            this.CurrentMixes = this.albumService.GetMixesForAlbum(this.CurrentAlbum);
            // if not set .. set the album defaults on each track 
            this.CurrentMixes.EnsureAlbumEntries(this.CurrentAlbumConfiguration, this.trackService);
            this.InitialiseAlbumExportLocation();
            // check for any mixes that have the cubase default 'Mixdown' as the title ... 
            foreach (var item in CurrentMixes.ThatHaveMixDownAsTitle())
            {
                var haveSavedMixdown = this.CurrentAlbumConfiguration.FindMixDown(item);
                if (haveSavedMixdown != null)
                {
                    // reset a selected distribution mix to the saved info 
                    // it has been refreshed by cubase as a new mixdown 
                    item.SetAlbumInformation(this.CurrentAlbumConfiguration);
                    item.UpdateFromAnotherMix(haveSavedMixdown);

                }
                else // not saved but set some defaults !
                {
                    item.Title = item.Title = Path.GetFileNameWithoutExtension(item.FileName);
                    item.SetAlbumInformation(this.CurrentAlbumConfiguration);
                }
                this.trackService.SetTagsFromMixDowm(item);
            }

            // mark any mixes as marked for distribution here ... 
            this.CurrentMixes.SelectSelectedForDistribution(this.CurrentAlbumConfiguration.DistributionMixes);

            // if any mixes have been updated - then re-copythem to the export directory 
            // ready for upload so the distributer 
            this.CurrentAlbumConfiguration.CheckForUpdatedDistributionMixes(this.CurrentMixes);

            this.ShowMixes();
        }

        private void SetMixExportLocation(string albumLocation)
        {
            if (!string.IsNullOrEmpty(albumLocation) && this.CurrentMixes != null)
            {
                this.CurrentMixes.SetMixdownExportLocation(albumLocation);
            }
        }

        private void ShowMixes()
        {
            this.SetMixExportLocation(this.AlbumExportLocation.Text);
            this.mixdownControl.ShowMixes(this.CurrentMixes, this.OnMixChanged, this.OnPlayMix, this.trackService, this.messageService, this.serviceProvider);
        }

        private void SetSelectionButtonsState()
        {
            this.DisableButtonsThatNeedASelection();
            if (this.CurrentMixes.AreAnyMixesSelected())
            {
                this.EnableButtonsThatNeedASelection();
            }
        }

        private void OnPlayMix(MixDown mixDown)
        {
            this.PlayTrack.PlayTrack(mixDown);
        }

        private void OnMixChanged(MixDown mixDown, string propertyName)
        {
            switch (propertyName)
            {
                case nameof(MixDown.Selected):
                    this.SetSelectionButtonsState();
                    break;
                case nameof(MixDown.MarkForDistribution):
                    if (mixDown.MarkForDistribution)
                    {
                        this.CurrentAlbumConfiguration.AddForDistribution(mixDown);
                    }
                    else
                    {
                        this.CurrentAlbumConfiguration.RemoveFromDistribution(mixDown);
                    }
                    break;
                default:
                    this.trackService.SetTagsFromMixDown(mixDown);
                    this.CurrentAlbumConfiguration.UpdateMixDistribution(mixDown);
                    break;
            }
        }

        private void OnAlbumChanged(AlbumConfiguration albumConfiguration, string propertyName)
        {
            albumConfiguration.SaveToDirectory(this.CurrentAlbum.AlbumPath);
            foreach (var mix in this.CurrentMixes)
            {
                this.SetMixFromAlbum(albumConfiguration, mix);
            }
        }

        private void SetMixFromAlbum(AlbumConfiguration albumConfiguration, MixDown mix)
        {
            mix.Album = albumConfiguration.Title;
            mix.Year = albumConfiguration.Year;
            mix.Artist = albumConfiguration.Artist;
            mix.Genre = albumConfiguration.Genre;
            this.trackService.SetTagsFromMixDown(mix);
        }

        public void Initialise(AlbumLocation? albumLocation = null)
        {
            this.InitialiseAlbumDropDown();
            if (albumLocation != null)
            {
                for (int i = 0; i < this.SelectedAlbum.Items.Count; i++)
                {
                    var album = this.SelectedAlbum.Items[i] as AlbumLocation;
                    if (album.AlbumPath == albumLocation.AlbumPath)
                    {
                        this.SelectedAlbum.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void InitialiseAlbumDropDown()
        {
            if (this.SelectedAlbum.SelectedIndex > -1)
            {
                this.SelectedAlbum.SelectedIndex = -1;
            }

            this.SelectedAlbum.Items.Clear();
            var albumList = this.albumService.GetAlbumList(this.messageService.ShowError);
            this.SelectedAlbum.Items.AddRange(albumList.ToArray());
            this.SelectedAlbum.DisplayMember = nameof(AlbumLocation.AlbumName);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (this.configurationService.Configuration == null)
            {
                this.configurationService.LoadConfiguration(() =>
                {
                    this.messageService.ShowError("Could not load configuration. Please check if the configuration file is correct.");
                });
            }
            if (this.configurationService?.Configuration?.AlbumWindowLocation != null)
            {
                if (this.configurationService.Configuration.AlbumWindowLocation.isMaximised)
                {
                    this.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    StartPosition = FormStartPosition.Manual;
                    Bounds = new Rectangle(
                        this.configurationService.Configuration.AlbumWindowLocation.X,
                        this.configurationService.Configuration.AlbumWindowLocation.Y,
                        this.configurationService.Configuration.AlbumWindowLocation.Width,
                        this.configurationService.Configuration.AlbumWindowLocation.Height);
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            //this.Bounds = new Rectangle() {  } 
            // this.configurationService.Configuration.
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
            this.configurationService?.Configuration?.AlbumWindowLocation = settings;
            this.configurationService?.SaveConfiguration((err) =>
            {
                this.messageService.ShowError($"Could not save configuration file. {err}");
            });
        }
    }
}
