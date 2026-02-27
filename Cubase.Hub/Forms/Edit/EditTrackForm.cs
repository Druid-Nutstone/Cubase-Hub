using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Services;
using Cubase.Hub.Services.Audio;
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

namespace Cubase.Hub.Forms.Edit
{
    public partial class EditTrackForm : BaseWindows11Form
    {
        private readonly ITrackService trackService;

        private readonly IMessageService messageService;
        
        private MixDown mixDown;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? AudioFile { get; set; }

        public EditTrackForm(ITrackService trackService, IMessageService messageService)
        {
            InitializeComponent();
            this.trackService = trackService;
            this.messageService = messageService;
            ThemeApplier.ApplyDarkTheme(this);
            this.UpdateButton.Click += UpdateButton_Click;
        }

        private void UpdateButton_Click(object? sender, EventArgs e)
        {
            if (this.mixDown != null)
            {
                var msgHandler = this.messageService.OpenMessage($"Updating mix {this.mixDown.Title}", this); 
                this.trackService.SetTagsFromMixDowm(this.mixDown);
                msgHandler.Close();
            }
        }

        public void Initialise(string audioFile)
        {
            this.UpdateButton.Enabled = false;  
            this.AudioFile = audioFile;
            this.mixDown = this.trackService.PopulateTagsFromFile(this.AudioFile);
            this.mixDown.PropertyChanged += MixDown_PropertyChanged;
            this.Text = $"Edit {this.mixDown.Title}";
            this.Title.Bind(nameof(MixDown.Title), this.mixDown);
            this.Album.Bind(nameof(MixDown.Album), this.mixDown);
            this.Artist.Bind(nameof(MixDown.Artist), this.mixDown);
            this.Performers.Bind(nameof(MixDown.Performers), this.mixDown);
            this.Bitrate.Bind(nameof(MixDown.BitRate), this.mixDown);
            this.SampleRate.Bind(nameof(MixDown.SampleRate), this.mixDown);
            this.TrackNo.Bind(nameof(MixDown.TrackNumber), this.mixDown);
            this.Comments.Bind(nameof(MixDown.Comment), this.mixDown);
            this.Length.Bind(nameof(MixDown.Duration), this.mixDown);
            this.Size.Bind(nameof(MixDown.Size), this.mixDown);
            this.AudioType.Bind(nameof(MixDown.AudioType), this.mixDown);
            this.Genre.AutoCompleteCustomSource.Clear();
            this.Genre.AutoCompleteCustomSource.AddRange(CubaseHubConstants.TagGenres);
            this.Genre.Bind(nameof(MixDown.Genre), this.mixDown);
        }

        private void MixDown_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            this.UpdateButton.Enabled = true;
        }
    }
}
