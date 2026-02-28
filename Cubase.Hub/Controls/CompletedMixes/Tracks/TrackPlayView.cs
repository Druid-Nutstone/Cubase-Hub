using Cubase.Hub.Controls.Album.Manage;
using Cubase.Hub.Controls.Media.Play;
using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Models;
using Cubase.Hub.Services.Track;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Controls.CompletedMixes.Tracks
{
    public class TrackPlayView : TableLayoutPanel
    {
        public TrackPlayView()
        {
            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ColumnCount = 1;
            RowCount = 0;
            GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            this.DoubleBuffered = true;
            // ✅ THIS IS CRITICAL
            ColumnStyles.Clear();
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            Padding = new Padding(10);
        }

        public void ShowMixes(MixDownCollection mixes, IServiceProvider serviceProvider, PlayTrackControl playTrackControl )
        {
            this.Controls.Clear();
            this.RowStyles.Clear();
            this.RowCount = 0;

            this.ColumnCount = 1;
            this.ColumnStyles.Clear();
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            foreach (var mix in mixes)
            {
                var trackControl = new TrackPlayViewControl(mix, serviceProvider, playTrackControl);
                trackControl.Dock = DockStyle.Fill;
                this.Controls.Add(trackControl, 0, this.RowCount);
                this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                this.RowCount++;
            }
            this.RowStyles.Add(new RowStyle(SizeType.Absolute, 10)); // bottom spacing
            ThemeApplier.ApplyDarkTheme(this);
        }
    }
}
