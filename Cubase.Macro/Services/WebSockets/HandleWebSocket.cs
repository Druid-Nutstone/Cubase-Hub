using Cubase.Macro.Common.Models;
using Cubase.Macro.Services.Config;
using Cubase.Macro.Services.Midi;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace Cubase.Macro.Services.WebSockets
{

    public static class CubaseSockets
    {
        private static Microsoft.Extensions.Logging.ILogger Log;

        private static IConfigurationService ConfigurationService;

        public static async Task HandleWebSocket(WebSocket socket, IMidiService midiService, Microsoft.Extensions.Logging.ILogger logger, IConfigurationService configurationService)
        {
            Log = logger;
            ConfigurationService = configurationService;
            var buffer = new byte[1024 * 10];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer, CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    break;
                }

                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                var socketRequest = WebSocketMidiCommandMessage.CreateFromRequest(message);

                WebSocketMidiCommandMessage response = WebSocketMidiCommandMessage.CreateError("Unknown Command"); 

                switch (socketRequest.Command)
                {
                    case WebSocketMidiCommand.MidiCommand:
                        response = RunMidiCommand(socketRequest.GetCommand());
                        break;
                    case WebSocketMidiCommand.MidiCommandList:
                        response = GetMidiCommandList(); 
                        break;
                }

                // Example: trigger MIDI
                // midiService.Send(message);

                var responseEncoded = Encoding.UTF8.GetBytes(response.Serialise());
                await socket.SendAsync(responseEncoded, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        static WebSocketMidiCommandMessage GetMidiCommandList()
        {
            Log.LogInformation("Received command to get Midi Commands");
            var macroCollection = CubaseMacroCollection.Load();
            return WebSocketMidiCommandMessage.CreateFromMacroCollection(macroCollection);
        }

        static WebSocketMidiCommandMessage RunMidiCommand(CubaseKeyCommand cubaseKeyCommand)
        {
            return WebSocketMidiCommandMessage.CreateError("Not implemented! - yet");
        }
    }
}
