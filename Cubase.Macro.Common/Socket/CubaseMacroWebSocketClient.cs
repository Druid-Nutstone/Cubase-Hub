using Cubase.Macro.Common.Models;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace Cubase.Macro.Common.Socket
{
    // Cannot inherit from sealed ClientWebSocket; wrap it instead.
    public class CubaseMacroWebSocketClient : IDisposable
    {
        private readonly ClientWebSocket client = new ClientWebSocket();

        private string ipAddress;

        private WebSocketMidiCommandMessage response = null;

        public ClientWebSocket Client => client;

        // Commonly useful convenience members forwarded to the inner instance.
        public WebSocketState State => client.State;
        public void Abort() => client.Abort();
        public void Dispose() => client.Dispose();

        public CubaseMacroWebSocketClient(string ipAddress)
        {
            this.ipAddress = ipAddress;
        }

        public async Task<bool> Connect()
        {
            await client.ConnectAsync(new Uri($"ws://{ipAddress}:8014/ws"), CancellationToken.None);
            var maxWait = 200;
            var current = 0;
            while (client.State != WebSocketState.Open || current > maxWait)
            {
                Task.Delay(200).Wait();
            }
            _ = ReceiveLoop();
            return maxWait < 200;
        }

        public async Task<CubaseMacroCollection?> GetKeyCommands()
        {
            if (IsConnected())
            {
                this.response = null;
                await this.SendWebSocketCommand(WebSocketMidiCommandMessage.CreateFromCommand(WebSocketMidiCommand.MidiCommandList));
                var response = await this.WaitForResponse();
                if (response == null)
                {
                    return null;
                }
                return response.GetMacroCollection();
            }
            return null;
        }

        private async Task<WebSocketMidiCommandMessage?> WaitForResponse()
        {
            var maxCount = 200;
            var current = 0;
            while (this.response == null || current > maxCount)
            {
                Task.Delay(200).Wait();
            }
            if (current > maxCount)
            {
                return null;
            }
            return this.response;
        }

        public bool IsConnected()
        {
            return client?.State == WebSocketState.Open;    
        }

        private async Task<bool> SendWebSocketCommand(WebSocketMidiCommandMessage message)
        {
            var data = Encoding.UTF8.GetBytes(message.Serialise());
            await client.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Text, true, CancellationToken.None);
            return true;
        }

        private async Task ReceiveLoop()
        {
            var buffer = new byte[8192];

            while (client.State == WebSocketState.Open)
            {
                using var ms = new MemoryStream();
                WebSocketReceiveResult result;

                do
                {
                    result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    ms.Write(buffer, 0, result.Count);
                } while (!result.EndOfMessage);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                }
                else if (result.MessageType == WebSocketMessageType.Text)
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    using var reader = new StreamReader(ms, Encoding.UTF8);
                    string message = await reader.ReadToEndAsync();
                    response = WebSocketMidiCommandMessage.Deserialise(message);
                }
            }
        }
    }
}
