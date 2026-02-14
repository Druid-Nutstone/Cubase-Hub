using Cubase.Hub.Controls.MainFormControls.ProjectsControl;
using Cubase.Hub.Controls.MainFormControls.ProjectsControl.PlayControls;
using Cubase.Hub.Services.Audio;
using Cubase.Hub.Services.Messages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Controls.MainFormControls.ProjectsForm
{
    public class CubaseProjectItemMixesControl : TableLayoutPanel
    {
        private readonly IAudioService audioService;

        private readonly IServiceProvider serviceProvider;

        private readonly IMessageService messageService;    

        private List<string> mixes;

        private ProjectItemMixesContextMenu contextMenu; 


        
        public CubaseProjectItemMixesControl(IAudioService audioService, 
                                             IMessageService messageService,
                                             IServiceProvider serviceProvider)
        {
            this.audioService = audioService;   
            this.serviceProvider = serviceProvider;
            this.messageService = messageService;
            this.Dock = System.Windows.Forms.DockStyle.Top;
            this.AutoSize = true;
            this.DoubleBuffered = true; 
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.contextMenu = new ProjectItemMixesContextMenu(audioService, this.messageService);   
            // this.Padding = new System.Windows.Forms.Padding(5);
        }

        public void SetMixes(List<string> mixes)
        {
            this.mixes = mixes;
            this.Controls.Clear();
            this.RowStyles.Clear();
            this.RowCount = 0;
           
            foreach (var mix in mixes)
            {
                // todo change this to use IAudioservice (see manage albums form#)
                
                var tags = TagLib.File.Create(mix);
                // add the name of the mix as a label
                this.AddLabel(tags.Tag.Title ?? Path.GetFileName(mix), 0);
                // add the size of the mix as a label
                var fileInfo = new System.IO.FileInfo(mix);
                this.AddLabel($"{fileInfo.Length / 1024} KB", 1);
                this.AddLabel(tags.Properties.Duration.ToString(@"mm\:ss"), 2);
                this.AddLabel($"#{tags.Tag.Track.ToString() ?? "Unknown number"}", 3);
                this.AddLabel(Path.GetExtension(mix)?.Replace('.',' ').Trim().ToUpper() ?? "Unknown type", 4);
                // add play button 

                var playButton = this.serviceProvider.GetService<PlayControl>();
                playButton.MusicFile = mix;
                this.Controls.Add(playButton, 5, this.RowCount);
                this.Controls.Add(playButton, 5, this.RowCount);
                this.RowCount++;
            }
        }
    
        public void AddLabel(string text, int column)
        {
            this.RowStyles.Add(new RowStyle(System.Windows.Forms.SizeType.AutoSize));
            var label = new System.Windows.Forms.Label
            {
                Text = text,
                AutoSize = true,
                Dock = System.Windows.Forms.DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Padding = new System.Windows.Forms.Padding(5)
            };
            this.Controls.Add(label, column, this.RowCount); 
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Right)
                return;

            int y = 0;
            var heights = this.GetRowHeights();

            var rowIndex = -1;
            for (int i = 0; i < heights.Length; i++)
            {
                if (e.Location.Y >= y && e.Location.Y < y + heights[i])
                {
                    rowIndex = i;
                    break;
                }

                y += heights[i];
            }

            if (rowIndex >= 0 && rowIndex < mixes.Count)
            {
                if (Path.GetExtension(this.mixes[rowIndex]).ToLower() == ".wav")
                {
                    contextMenu.Show(this, e.Location);
                }
            }
        }

    }
}
