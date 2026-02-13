using Cubase.Hub.Services;
using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Hub.Controls.Album
{
    public partial class AlbumConfigurationControl : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AlbumConfiguration AlbumConfiguration { get; set; } = new AlbumConfiguration();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<AlbumConfiguration, string>? OnAlbumChanged { get; set; }

        public AlbumConfigurationControl()
        {
            InitializeComponent();
        }

        public void DisableTitle()
        {
            this.AlbumTitle.Enabled = false;
        }

        public void EnableTitle()
        {
            this.AlbumTitle.Enabled = true;
        }

        public void Initialise(Action<AlbumConfiguration, string>? onAlbumChanged = null)
        {
            this.AlbumTitle.Bind(nameof(AlbumConfiguration.Title), AlbumConfiguration);
            this.AlbumArtist.Bind(nameof(AlbumConfiguration.Artist), AlbumConfiguration);
            this.AlbumYear.Bind(nameof(AlbumConfiguration.Year), AlbumConfiguration);
            this.AlbumComments.Bind(nameof(AlbumConfiguration.Comments), AlbumConfiguration);
            this.AlbumGenre.AutoCompleteCustomSource.Clear();
            this.AlbumGenre.AutoCompleteCustomSource.AddRange(CubaseHubConstants.TagGenres);
            this.AlbumGenre.Bind(nameof(AlbumConfiguration.Genre), AlbumConfiguration);
            this.AlbumConfiguration.PropertyChanged += AlbumConfiguration_PropertyChanged;
            this.OnAlbumChanged = onAlbumChanged;
        }

        private void AlbumConfiguration_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            this.OnAlbumChanged?.Invoke(this.AlbumConfiguration, e.PropertyName);
        }


    }
}
