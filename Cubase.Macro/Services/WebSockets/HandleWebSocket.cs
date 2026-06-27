using Cubase.Macro.Common.Lyrics.Services;
using Cubase.Macro.Common.Models;
using Cubase.Macro.Services.Config;
using Cubase.Macro.Services.Lyrics;
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
        private static Serilog.ILogger Log;

        private static IConfigurationService ConfigurationService;

        private static IMidiService MidiService;

        private static ILyricFileService LyricFileService;

        public static async Task HandleWebSocket(WebSocket socket, 
                                                 IMidiService midiService, 
                                                 Serilog.ILogger logger,
                                                 ILyricFileService lyricFileService,
                                                 IConfigurationService configurationService)
        {
            Log = logger;
            ConfigurationService = configurationService;
            MidiService = midiService;
            LyricFileService = lyricFileService;
            var buffer = new byte[1024 * 10];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer, CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    Log.Information("Closing WebSocket Connection");
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
                    case WebSocketMidiCommand.MidiTransportLocation:
                        response = GetMidiTransportLocation(); 
                        break;
                    case WebSocketMidiCommand.MidiLyricCurrentProject:
                        response = GetMidiLyricCurrentProject();
                        break;
                    case WebSocketMidiCommand.MidiLyricStartTransportMonitoring:
                        midiService.StartTransportMonitoring();
                        response = WebSocketMidiCommandMessage.CreateFromCommand(WebSocketMidiCommand.MidiLyricStartTransportMonitoring);
                        break;
                    case WebSocketMidiCommand.MidiLyricStopTransportMonitoring:
                        midiService.StopTransportMonitoring();
                        response = WebSocketMidiCommandMessage.CreateFromCommand(WebSocketMidiCommand.MidiLyricStopTransportMonitoring);
                        break;
                    case WebSocketMidiCommand.MidiLyricIndex:
                        response = GetMidiLyricIndex();
                        break;
                    case WebSocketMidiCommand.MidiLyricContent:
                        response = GetMidiLyricContent(socketRequest.GetLyric());
                        break;
                }

                // Example: trigger MIDI
                // midiService.Send(message);

                var responseEncoded = Encoding.UTF8.GetBytes(response.Serialise());
                await socket.SendAsync(responseEncoded, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        static WebSocketMidiCommandMessage GetMidiTransportLocation()
        {
            Log?.Information("Received command to get Midi TransportLocation");
            return WebSocketMidiCommandMessage.CreateFromTransportCollection(MidiService.TransportLocation);
        }

        static WebSocketMidiCommandMessage GetMidiLyricContent(Lyric lyric)
        {
            Log?.Information("Received command to get Midi TransportLocation");
            return WebSocketMidiCommandMessage.CreateLyricContentResponse(LyricFileService.GetLyricContent(lyric));
        }

        static WebSocketMidiCommandMessage GetMidiLyricCurrentProject()
        {
            Log?.Information("Received command to get Midi Current Lyric");
            return WebSocketMidiCommandMessage.CreateFromLyricResponse(LyricFileService.GetProjectCurrentLyrics());
        }

        static WebSocketMidiCommandMessage GetMidiLyricIndex()
        {
            Log?.Information("Received command to get Midi Lyric Index");
            return WebSocketMidiCommandMessage.CreateFromLyricIndexResponse(LyricFileService.GetLyricIndex());
        }

        
        static WebSocketMidiCommandMessage GetMidiCommandList()
        {
            Log?.Information("Received command to get Midi Commands");
            var macroCollection = CubaseMacroCollection.Load();
            return WebSocketMidiCommandMessage.CreateFromMacroCollection(macroCollection.GetMidiRemoteCollection());
        }

        static WebSocketMidiCommandMessage RunMidiCommand(CubaseKeyCommand cubaseKeyCommand)
        {
            Log?.Information($"Received Midi Command {cubaseKeyCommand.Name}");
            if (MidiService.SendMidiMessage(cubaseKeyCommand))
            {
                return WebSocketMidiCommandMessage.CreateFromCommand(WebSocketMidiCommand.MidiCommand);
            }            
            else return WebSocketMidiCommandMessage.CreateError("Not implemented! - yet");
        }
    }
}
