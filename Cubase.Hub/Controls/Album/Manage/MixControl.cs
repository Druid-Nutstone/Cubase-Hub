using Cubase.Hub.Controls.MainFormControls.ProjectsControl.PlayControls;
using Cubase.Hub.Services.Audio;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Hub.Controls.Album.Manage
{
    public partial class MixControl : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<MixDown, string> OnMixChanged { get; set; }

        private MixDown Mix;

        private IMessageService messageService;

        public MixControl()
        {
            InitializeComponent();
        }

        public MixControl(MixDown mixDown, IAudioService audioService, IMessageService messageService)
        {
            InitializeComponent();
            this.Initialise(mixDown, audioService);
            this.messageService = messageService;
            if (mixDown.AudioType.ToLower() == "wav")
            {
                this.ContextMenuStrip = new MixControlContextMenu(mixDown, audioService, messageService);
            }
            
        }

        public void Initialise(MixDown mixDown, IAudioService audioService)
        {
            this.Mix = mixDown;
            mixDown.PropertyChanged += MixDown_PropertyChanged;
            this.MixTitle.Bind(nameof(MixDown.Title), mixDown);
            this.MixTrackNo.Bind(nameof(MixDown.TrackNumber), mixDown);
            this.MixPerformers.Bind(nameof(MixDown.Performers), mixDown);
            this.MixDuration.Bind(nameof(MixDown.Duration), mixDown);
            this.PlayerPanel.Controls.Clear();
            var playerControl = new PlayControl(audioService);
            playerControl.MusicFile = mixDown.FileName;
            playerControl.Dock = DockStyle.Fill;    
            this.PlayerPanel.Controls.Add(playerControl);
            this.MixComments.Bind(nameof(MixDown.Comment), mixDown);
            this.MixExtra.Text = $"({mixDown.Size},{mixDown.AudioType},{mixDown.Artist},{mixDown.Album})";
            this.MixSelected.Bind(nameof(MixDown.Selected),mixDown);
        }

        private void MixDown_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            this.OnMixChanged?.Invoke(this.Mix, e.PropertyName);
        }
    }
}
