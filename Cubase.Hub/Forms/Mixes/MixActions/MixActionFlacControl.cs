using Cubase.Hub.Services.Audio;
using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Models;
using FFMpegCore.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Hub.Forms.Mixes.MixActions
{
    public partial class MixActionFlacControl : UserControl, IMixActionControl
    {
        private CompressionLevel CompressionLevel = CompressionLevel.Default;
        
        public MixActionFlacControl()
        {
            InitializeComponent();
            this.CompressionComboBox.Items.Clear();
            this.CompressionComboBox.Items.AddRange(Enum.GetNames(typeof(CompressionLevel)));
            this.CompressionComboBox.SelectedIndexChanged += CompressionComboBox_SelectedIndexChanged;
            this.CompressionComboBox.SelectedItem = this.CompressionLevel.ToString();
        }

        private void CompressionComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            this.CompressionLevel = Enum.Parse<CompressionLevel>(this.CompressionComboBox.SelectedItem as string, true);
        }

        public void RunAction(MixDownCollection mixDowns, 
                              string targetDirectory,
                              IAudioService audioService, 
                              IMessageService messageService, 
                              IDirectoryService directoryService,
                              Func<int, string, bool> onProgress)
        {
            for (var i = 0; i < mixDowns.Count; i++)
            {
                var state = onProgress.Invoke(i + 1, Path.GetFileNameWithoutExtension(mixDowns[i].FileName));
                audioService.ConvertToFlac(mixDowns[i], targetDirectory, this.CompressionLevel);
            }
            onProgress.Invoke(0, "All Done");
        }
    }
}
