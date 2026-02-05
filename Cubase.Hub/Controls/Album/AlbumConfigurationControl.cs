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
        
        public AlbumConfigurationControl()
        {
            InitializeComponent();
        }

        public void Initialise()
        {
            this.AlbumTitle.Bind(nameof(AlbumConfiguration.Title), AlbumConfiguration);
            this.AlbumArtist.Bind(nameof(AlbumConfiguration.Artist), AlbumConfiguration);
            this.AlbumYear.Bind(nameof(AlbumConfiguration.Year), AlbumConfiguration);
            this.AlbumComments.Bind(nameof(AlbumConfiguration.Comments), AlbumConfiguration);
            // Replace this line:
            // this.AlbumGenre.AutoCompleteCustomSource = CubaseHubConstants.TagGenres;

            // With the following code:
            this.AlbumGenre.AutoCompleteCustomSource.Clear();
            this.AlbumGenre.AutoCompleteCustomSource.AddRange(CubaseHubConstants.TagGenres);
            this.AlbumGenre.Bind(nameof(AlbumConfiguration.Genre), AlbumConfiguration);
        }


    }
}
