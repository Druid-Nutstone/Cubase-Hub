using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Forms.Mixes;
using Cubase.Hub.Services;
using Cubase.Hub.Services.Audio;
using Cubase.Hub.Services.Config;
using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Models;
using Cubase.Hub.Services.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Hub.Forms.Albums
{
    public partial class ManageAlbumsForm : BaseWindows11Form
    {

        private readonly IConfigurationService configurationService;

        private readonly IProjectService projectService;

        private readonly IDirectoryService directoryService;

        private readonly IAudioService audioService;

        private readonly IMessageService messageService;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AlbumLocation CurrentAlbum { get; set; }

        private MixDownCollection CurrentMixes;

        private AlbumConfiguration CurrentAlbumConfiguration;

        private ManageMixesForm mixesForm;

        public ManageAlbumsForm(IConfigurationService configurationService,
                                IDirectoryService directoryService,
                                IAudioService audioService,
                                IMessageService messageService,
                                ManageMixesForm manageMixesForm,
                                IProjectService projectService)
        {
            InitializeComponent();
            this.configurationService = configurationService;
            this.directoryService = directoryService;
            this.audioService = audioService;
            this.projectService = projectService;
            this.messageService = messageService;
            this.mixesForm = manageMixesForm;
            ThemeApplier.ApplyDarkTheme(this);
            this.SelectedAlbum.SelectedIndexChanged += SelectedAlbum_SelectedIndexChanged;
            this.RereshFromAblumButton.Click += RereshFromAblumButton_Click;
            this.OpenAlbumDirectory.Click += OpenAlbumDirectory_Click;
            this.DeleteSelectedButton.BackColor = Color.FromKnownColor(KnownColor.IndianRed);
            this.DeleteSelectedButton.Click += DeleteSelectedButton_Click;
            this.DisableButtonsThatNeedASelection();
            this.ManageMixesButton.Click += ManageMixesButton_Click;
            this.SelectDeselectAllMixes.Text = "Select All";
            this.SelectDeselectAllMixes.CheckedChanged += SelectDeselectAllMixes_CheckedChanged;
            this.SetSelectedTracksTitleButton.Click += SetSelectedTracksTitleButton_Click;
            this.AutoScaleMode = AutoScaleMode.Dpi;
        }

        private void ManageMixesButton_Click(object? sender, EventArgs e)
        {
            this.mixesForm.Initialise(this.CurrentMixes.GetSelectedMixes());
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
                    this.audioService.SetTagsFromMixDowm(mix);
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
                    this.audioService.SetTagsFromMixDowm(mix);
                }
                this.LoadTracks(this.CurrentAlbum.AlbumPath);
            }
        }

        private void OpenAlbumDirectory_Click(object? sender, EventArgs e)
        {
            if (this.CurrentAlbum != null)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "explorer.exe",
                    Arguments = this.CurrentAlbum.AlbumPath,
                    UseShellExecute = true
                });
            }
        }

        private void SelectedAlbum_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (this.SelectedAlbum.SelectedIndex > -1)
            {
                this.CurrentAlbum = this.SelectedAlbum.SelectedItem as AlbumLocation;
                // get album
                this.CurrentAlbumConfiguration = AlbumConfiguration.LoadFromFile(Path.Combine(this.CurrentAlbum.AlbumPath, CubaseHubConstants.CubaseAlbumConfigurationFileName));
                this.AlbumConfigurationControl.AlbumConfiguration = this.CurrentAlbumConfiguration;
                this.AlbumConfigurationControl.Initialise(this.OnAlbumChanged);
                // get album mixes 
                var msgDialog = this.messageService.OpenMessage("Loading Tracks..", this);
                this.LoadTracks(this.CurrentAlbum.AlbumPath);
                msgDialog.Close();
            }
            else
            {
                this.AlbumConfigurationControl.AlbumConfiguration = new AlbumConfiguration();
                this.AlbumConfigurationControl.Initialise();
                this.mixdownControl.ShowMixes(new MixDownCollection(), this.OnMixChanged, this.audioService, this.messageService);
            }
        }

        private void LoadTracks(string albumPath)
        {
            var mixes = this.directoryService.GetMixes(albumPath);
            this.CurrentMixes = this.audioService.PopulateMixDownCollectionFromTags(mixes);
            this.ShowMixes();
        }

        private void ShowMixes()
        {
            this.mixdownControl.ShowMixes(this.CurrentMixes, this.OnMixChanged, this.audioService, this.messageService);
        }

        private void SetSelectionButtonsState()
        {
            this.DisableButtonsThatNeedASelection();
            if (this.CurrentMixes.AreAnyMixesSelected())
            {
                this.EnableButtonsThatNeedASelection();
            }
        }



        private void OnMixChanged(MixDown mixDown, string propertyName)
        {
            if (propertyName == nameof(MixDown.Selected))
            {
                this.SetSelectionButtonsState();
            }
            else
            {
                this.audioService.SetTagsFromMixDowm(mixDown);
                if (propertyName == nameof(MixDown.TrackNumber))
                {
                    this.LoadTracks(this.CurrentAlbum.AlbumPath);
                }
            }
        }

        private void OnAlbumChanged(AlbumConfiguration albumConfiguration, string propertyName)
        {

            albumConfiguration.SaveToDirectory(this.CurrentAlbum.AlbumPath);
            foreach (var mix in this.CurrentMixes)
            {
                mix.Album = albumConfiguration.Title;
                mix.Year = albumConfiguration.Year;
                mix.Artist = albumConfiguration.Artist;
                mix.Genre = albumConfiguration.Genre;
                this.audioService.SetTagsFromMixDowm(mix);
            }
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
            this.SelectedAlbum.Items.AddRange(this.directoryService.GetCubaseAlbums(this.configurationService.Configuration.SourceCubaseFolders).ToArray());
            this.SelectedAlbum.DisplayMember = nameof(AlbumLocation.AlbumName);
        }
    }
}
