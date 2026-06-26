using Cubase.Macro.Common.Lyrics.Services;
using Cubase.Macro.Common.Models;
using Cubase.Macro.Common.Socket;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Mobile.Lyrics
{
    public class MobileLyricService : IlyricMidiService
    {
        private readonly CubaseMacroWebSocketClient webSocketClient;

        public MobileLyricService(CubaseMacroWebSocketClient websocketClient) 
        { 
            this.webSocketClient = websocketClient;
        }
        
        public TransportLocationCollection GetTransportLocation()
        {
            throw new NotImplementedException("CANNOT BE USED ON MOBILE APP!");
        }

        public bool IsMidiAvailable()
        {
            throw new NotImplementedException("CANNOT BE USED ON MOBILE APP!");
        }
    }
}
