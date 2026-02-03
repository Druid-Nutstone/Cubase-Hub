using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Controls.MainFormControls.ProjectsForm
{
    public class CubaseProjectItemMixesControl : TableLayoutPanel
    {
        public CubaseProjectItemMixesControl()
        {
            this.Dock = System.Windows.Forms.DockStyle.Top;
        }

        public void SetMixes(List<string> mixes)
        {
            this.Controls.Clear();
            this.RowStyles.Clear();
            this.RowCount = 0;

            var nameText = new System.Windows.Forms.Label
            {
                Text = "File Name",
                AutoSize = true,
                Dock = System.Windows.Forms.DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Font = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 10, System.Drawing.FontStyle.Bold),
                Padding = new System.Windows.Forms.Padding(5)
            };
            this.RowStyles.Add(new RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.Controls.Add(nameText, 0, this.RowCount);

            var sizeText = new System.Windows.Forms.Label
            {
                Text = "File Size",
                AutoSize = true,
                Dock = System.Windows.Forms.DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Font = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 10, System.Drawing.FontStyle.Bold),
                Padding = new System.Windows.Forms.Padding(5)
            };
            this.RowStyles.Add(new RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.Controls.Add(sizeText, 1, this.RowCount);

            var actionText = new System.Windows.Forms.Label
            {
                Text = "Action",
                AutoSize = true,
                Dock = System.Windows.Forms.DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Font = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 10, System.Drawing.FontStyle.Bold),
                Padding = new System.Windows.Forms.Padding(5)
            };
            this.RowStyles.Add(new RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.Controls.Add(actionText, 2, this.RowCount);


            this.RowCount++;
            
            foreach (var mix in mixes)
            {

                // add the name of the mix as a label
                var mixLabel = new System.Windows.Forms.Label
                {
                    Text = Path.GetFileName(mix),
                    AutoSize = true,
                    Dock = System.Windows.Forms.DockStyle.Fill,
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                    Padding = new System.Windows.Forms.Padding(5)
                };
                this.RowStyles.Add(new RowStyle(System.Windows.Forms.SizeType.AutoSize));
                this.Controls.Add(mixLabel, 0, this.RowCount);

                // add the size of the mix as a label
                var fileInfo = new System.IO.FileInfo(mix);
                var mixSizeLabel = new System.Windows.Forms.Label
                {
                    Text = $"{fileInfo.Length / 1024} KB",
                    AutoSize = true,
                    Dock = System.Windows.Forms.DockStyle.Fill,
                    TextAlign = System.Drawing.ContentAlignment.MiddleRight,
                    Padding = new System.Windows.Forms.Padding(5)
                };
                this.Controls.Add(mixSizeLabel, 1, this.RowCount);
                
                // add play button 
                var playButton = new System.Windows.Forms.Button
                {
                    Text = "Play",
                    AutoSize = true,
                    Dock = System.Windows.Forms.DockStyle.Fill,
                    Padding = new System.Windows.Forms.Padding(5)
                };
                playButton.Click += (sender, e) => 
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = mix,
                        UseShellExecute = true
                    });
                };
                this.Controls.Add(playButton, 2, this.RowCount);

                this.RowCount++;
            }
        }
    }
}
