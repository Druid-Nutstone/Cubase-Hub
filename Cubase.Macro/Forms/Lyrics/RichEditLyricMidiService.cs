using Cubase.Macro.Common.Lyrics.Services;
using Cubase.Macro.Common.Models;
using Cubase.Macro.Services.Midi;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Forms.Lyrics
{
    public class RichEditLyricMidiService : IlyricMidiService
    {
        private bool midiReady = false;

        private readonly IMidiService midiService;

        public RichEditLyricMidiService(IMidiService midiService)
        {
            this.midiService = midiService;
        }

        public TransportLocationCollection GetTransportLocation()
        {
            if (this.midiReady)
            {
                return this.midiService.TransportLocation;
            }
            return new TransportLocationCollection(); 
        }

        public bool IsMidiAvailable()
        {
            this.midiReady = this.midiService.Initialised;
            return this.midiReady;
        }
    }
}
