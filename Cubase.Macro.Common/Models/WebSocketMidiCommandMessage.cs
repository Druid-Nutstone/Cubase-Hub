using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Cubase.Macro.Common.Models
{
    public class WebSocketMidiCommandMessage
    {
        public WebSocketMidiCommand Command { get; set; }

        public string Message { get; set; }

        public string Serialise()
        {
            return JsonSerializer.Serialize(this);
        }
        
        public static WebSocketMidiCommandMessage Deserialise(string message)
        {
            return JsonSerializer.Deserialize<WebSocketMidiCommandMessage>(message);
        }

        public CubaseKeyCommand GetCommand()
        {
            return CubaseKeyCommand.Deserialise(this.Message);
        }

        public CubaseRemoteMidiMacroCollection GetMacroCollection()
        {
            return CubaseRemoteMidiMacroCollection.Deserialise(this.Message);
        }

        public TransportLocationCollection GetTransportLocationCollection()
        {
            return TransportLocationCollection.Deserialise(this.Message);
        }

        public LyricResponseModel GetLyricResponseModel()
        {
            return LyricResponseModel.Deserialise(this.Message);
        }

        public CubaseMidiProjectStatus GetCubaseMidiProjectStatus()
        {
            return CubaseMidiProjectStatus.Deserialise(this.Message);
        }

        public LyricIndexCollection GetLyricIndex()
        {
            return LyricIndexCollection.Deserialise(this.Message);
        }

        public LyricContent GetLyricContent()
        {
            return LyricContent.Deserialise(this.Message);
        }

        public Lyric GetLyric()
        {
            return Lyric.Deserialise(this.Message);
        }

        public static WebSocketMidiCommandMessage CreateFromCommand(WebSocketMidiCommand command)
        {
            return new WebSocketMidiCommandMessage() { Command = command };
        }

        public static WebSocketMidiCommandMessage CreateFromCommandWithMessage(WebSocketMidiCommand command, string message )
        {
            return new WebSocketMidiCommandMessage() { Command = command, Message = message };
        }

        public static WebSocketMidiCommandMessage CreateLyricContentRequest(Lyric lyric)
        {
            return new WebSocketMidiCommandMessage()
            {
                Command = WebSocketMidiCommand.MidiLyricContent,
                Message = lyric.Serialise()
            };
        }
        
        public static WebSocketMidiCommandMessage CreateFromMacroCollection(CubaseRemoteMidiMacroCollection cubaseMacroCollection)
        {
            return new WebSocketMidiCommandMessage()
            {
                Command = WebSocketMidiCommand.MidiCommandList,
                Message = cubaseMacroCollection.Serialise()
            };
        }

        public static WebSocketMidiCommandMessage CreateFromProjectStatus(CubaseMidiProjectStatus cubaseMidiProjectStatus)
        {
            return new WebSocketMidiCommandMessage()
            {
                Command = WebSocketMidiCommand.MidiProjectStatus,
                Message = cubaseMidiProjectStatus.Serialise()
            };
        }

        public static WebSocketMidiCommandMessage CreateFromTransportCollection(TransportLocationCollection transportLocationCollection)
        {
            return new WebSocketMidiCommandMessage()
            {
                Command = WebSocketMidiCommand.MidiTransportLocation,
                Message = transportLocationCollection.Serialise()
            };
        }

        public static WebSocketMidiCommandMessage CreateFromLyricResponse(LyricResponseModel lyricResponseModel)
        {
            return new WebSocketMidiCommandMessage()
            {
                Command = WebSocketMidiCommand.MidiLyricCurrentProject,
                Message = lyricResponseModel.Serialise()
            };
        }
        // CreateLyricContentResponse

        public static WebSocketMidiCommandMessage CreateLyricContentResponse(LyricContent lyricResponse)
        {
            return new WebSocketMidiCommandMessage()
            {
                Command = WebSocketMidiCommand.MidiLyricCurrentProject,
                Message = lyricResponse.Serialise()
            };
        }

        public static WebSocketMidiCommandMessage CreateFromLyricIndexResponse(LyricIndexCollection lyricIndexCollection)
        {
            return new WebSocketMidiCommandMessage()
            {
                Command = WebSocketMidiCommand.MidiLyricIndex,
                Message = lyricIndexCollection.Serialise()
            };
        }

        public static WebSocketMidiCommandMessage CreateFromRequest(string message)
        {
            return JsonSerializer.Deserialize<WebSocketMidiCommandMessage>(message);
        }

        public static WebSocketMidiCommandMessage CreateError(string errorMessage)
        {
            return new WebSocketMidiCommandMessage()
            {
                Command = WebSocketMidiCommand.Error,
                Message = errorMessage
            };
        }

        public static WebSocketMidiCommandMessage CreateFromKeyCommand(CubaseKeyCommand cubaseKeyCommand)
        {
            return new WebSocketMidiCommandMessage()
            {
                Command = WebSocketMidiCommand.MidiCommand,
                Message = cubaseKeyCommand.Serialise()
            };
        }

    }
}
