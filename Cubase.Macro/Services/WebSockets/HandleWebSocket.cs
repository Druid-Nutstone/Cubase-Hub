using Cubase.Macro.Common.Lyrics.Services;
using Cubase.Macro.Common.Models;
using Cubase.Macro.Services.Config;
using Cubase.Macro.Services.Lyrics;
using Cubase.Macro.Services.Midi;
using Cubase.Macro.Services.Window;
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

        private static IWindowService WindowService;

        public static async Task HandleWebSocket(WebSocket socket, 
                                                 IMidiService midiService, 
                                                 Serilog.ILogger logger,
                                                 ILyricFileService lyricFileService,
                                                 IWindowService windowService,
                                                 IConfigurationService configurationService)
        {
            Log = logger;
            ConfigurationService = configurationService;
            MidiService = midiService;
            WindowService = windowService;
            LyricFileService = lyricFileService;
            var buffer = new byte[1024 * 10];

            WebSocketReceiveResult result = null;

            while (socket.State == WebSocketState.Open)
            {
                try
                {
                    result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    Log.Error(ex,
                        "WebSocket ReceiveAsync failed. State={State}, CloseStatus={CloseStatus}, CloseDescription={CloseDescription}",
                        socket.State,
                        socket.CloseStatus,
                        socket.CloseStatusDescription);

                    if (ex is WebSocketException wsEx)
                    {
                        Log.Error(
                            "WebSocketErrorCode={ErrorCode}, Inner={Inner}",
                            wsEx.WebSocketErrorCode,
                            wsEx.InnerException?.Message);
                    }

                    if (socket.State != WebSocketState.Closed)
                        socket.Abort();
                    break;
                }
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
                    case WebSocketMidiCommand.MidiHeartBeat:
                        response = WebSocketMidiCommandMessage.CreateFromCommandWithMessage(WebSocketMidiCommand.MidiHeartBeat, "I am here");
                        break;
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
                    case WebSocketMidiCommand.MidiProjectStatus:
                        response = GetMidiProjectStatus();
                        break;
                }

                // Example: trigger MIDI
                // midiService.Send(message);

                var responseEncoded = Encoding.UTF8.GetBytes(response.Serialise());
                await socket.SendAsync(responseEncoded, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        static WebSocketMidiCommandMessage GetMidiProjectStatus()
        {
            Log?.Information("Received command to get Midi Project Status");
            var projectStatus = new CubaseMidiProjectStatus();
            if (!MidiService.Initialised)
            {
                projectStatus.ProjectStatus = CubaseMidiProjectStatusType.Unknown;
                projectStatus.Message = "Midi Service not initialised";
            }
            else
            {
                projectStatus.ProjectStatus = WindowService.IsCubaseMainWindowActive() ? CubaseMidiProjectStatusType.Active : CubaseMidiProjectStatusType.NotActive;
                projectStatus.ProjectName = WindowService.GetCubaseProjectTitle();
                projectStatus.Message = "Project Status retrieved successfully";
            }
            return WebSocketMidiCommandMessage.CreateFromProjectStatus(projectStatus);
        }

        static WebSocketMidiCommandMessage GetMidiTransportLocation()
        {
            Log?.Information("Received command to get Midi TransportLocation");
            if (!MidiService.MonitoringTransport) // if we are not monitoring - request it 
            {
                MidiService.StartTransportMonitoring();
                Task.Delay(500).Wait();
            }
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
