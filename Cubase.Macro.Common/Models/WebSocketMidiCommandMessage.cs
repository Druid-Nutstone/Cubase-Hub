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

        public static WebSocketMidiCommandMessage CreateFromCommand(WebSocketMidiCommand command)
        {
            return new WebSocketMidiCommandMessage() { Command = command };
        }
        
        public static WebSocketMidiCommandMessage CreateFromMacroCollection(CubaseRemoteMidiMacroCollection cubaseMacroCollection)
        {
            return new WebSocketMidiCommandMessage()
            {
                Command = WebSocketMidiCommand.MidiCommandList,
                Message = cubaseMacroCollection.Serialise()
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
