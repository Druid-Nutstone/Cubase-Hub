using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Cubase.Hub.Forms.BaseForm;
using Cubase.Hub.Services.Audio;

namespace Cubase.Hub.Controls.Album.Manage
{
    public class MixdownControl : TableLayoutPanel
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<MixDown, string> OnMixChanged { get; set; }   
        
        public MixdownControl()
        {
            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ColumnCount = 1;
            RowCount = 0;
            GrowStyle = TableLayoutPanelGrowStyle.AddRows;

            // ✅ THIS IS CRITICAL
            ColumnStyles.Clear();
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            Padding = new Padding(10);
        }

        public void ShowMixes(MixDownCollection mixes, Action<MixDown, string> onMixChanged, IAudioService audioService)
        {
            this.OnMixChanged = onMixChanged;

            this.Controls.Clear();
            this.RowStyles.Clear();
            this.RowCount = 0;

            this.ColumnCount = 1;
            this.ColumnStyles.Clear();
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            foreach (var mix in mixes)
            {
                var mixDowncontrol = new MixControl(mix, audioService)
                {
                    Dock = DockStyle.Fill
                };

                mixDowncontrol.OnMixChanged += (m,p) =>
                {
                    this.OnMixChanged?.Invoke(m,p);
                };

                this.Controls.Add(mixDowncontrol, 0, this.RowCount);
                this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                this.RowCount++;
            }

            ThemeApplier.ApplyDarkTheme(this);
        }

    }
}