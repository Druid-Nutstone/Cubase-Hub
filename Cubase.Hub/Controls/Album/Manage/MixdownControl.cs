using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Services.Audio;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Models;
using Cubase.Hub.Services.Track;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Hub.Controls.Album.Manage
{
    public class MixdownControl : TableLayoutPanel
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<MixDown, string> OnMixChanged { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<MixDown>? OnPlay { get; set; }

        public MixdownControl()
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

        public void ShowMixes(MixDownCollection mixes, Action<MixDown, string> onMixChanged, Action<MixDown> onPlay, ITrackService trackService, IMessageService messageService, IServiceProvider serviceProvider)
        {
            this.OnMixChanged = onMixChanged;
            this.OnPlay = onPlay;   
            this.Controls.Clear();
            this.RowStyles.Clear();
            this.RowCount = 0;

            this.ColumnCount = 1;
            this.ColumnStyles.Clear();
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            foreach (var mix in mixes)
            {
                var mixDowncontrol = new MixControl(mix, serviceProvider, trackService, messageService)
                {
                    Dock = DockStyle.Fill
                };

                mixDowncontrol.OnPlay += (mix) => { this.OnPlay?.Invoke(mix); };

                mixDowncontrol.OnMixChanged += (m,p) =>
                {

                    this.OnMixChanged?.Invoke(m, p);

                };

                this.Controls.Add(mixDowncontrol, 0, this.RowCount);
                this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                this.RowCount++;
            }
            this.RowStyles.Add(new RowStyle(SizeType.Absolute, 10)); // bottom spacing
            ThemeApplier.ApplyDarkTheme(this);
        }

    }
}