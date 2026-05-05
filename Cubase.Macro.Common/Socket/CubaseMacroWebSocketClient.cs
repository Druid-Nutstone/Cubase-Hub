using Cubase.Macro.Common.Models;
using System.Net.WebSockets;
using System.Text;

namespace Cubase.Macro.Common.Socket
{
    public class CubaseMacroWebSocketClient : IDisposable
    {
        private readonly ClientWebSocket client = new ClientWebSocket();
        private Task? receiveTask;
        private CancellationTokenSource cts = new();

        private TaskCompletionSource<WebSocketMidiCommandMessage>? pendingResponse;

        public WebSocketState State => client.State;

        public bool IsConnected => client.State == WebSocketState.Open;

        public void Dispose()
        {
            try
            {
                cts.Cancel();
                client.Dispose();
            }
            catch { }
        }

        // --------------------------------------------------
        // CONNECT
        // --------------------------------------------------
        public async Task<bool> Connect(string ipAddress, Action<string> errorHandler, int port = 8014)
        {
            if (client.State == WebSocketState.Open)
                return true;

            try
            {
                await client.ConnectAsync(
                    new Uri($"ws://{ipAddress}:{port}/ws"),
                    CancellationToken.None);
            }
            catch (Exception ex)
            {
                errorHandler.Invoke(ex.Message);
                return false;
            }
            if (client.State != WebSocketState.Open)
                return false;

            // Start receive loop on background thread
            receiveTask = Task.Run(ReceiveLoop);

            return true;
        }

        // --------------------------------------------------
        // PUBLIC API
        // --------------------------------------------------
        public async Task<CubaseRemoteMidiMacroCollection?> GetMacroCollection()
        {
            var response = await SendAndWait(
                WebSocketMidiCommandMessage.CreateFromCommand(
                    WebSocketMidiCommand.MidiCommandList));

            return response?.GetMacroCollection();
        }

        public async Task<WebSocketMidiCommandMessage> SendMidiCommand(CubaseKeyCommand cubaseKeyCommand)
        {
            return await SendAndWait(WebSocketMidiCommandMessage.CreateFromKeyCommand(cubaseKeyCommand));
        }

        public async Task Close()
        {
            if (this.client != null)
            {
                if (this.client.State == WebSocketState.Open)
                {
                    await this.client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client Disconnected", CancellationToken.None);
                }
            }
        }

        // --------------------------------------------------
        // CORE SEND/RECEIVE
        // --------------------------------------------------
        private async Task<WebSocketMidiCommandMessage?> SendAndWait(
            WebSocketMidiCommandMessage message,
            int timeoutMs = 5000)
        {
            if (!IsConnected)
                throw new InvalidOperationException("WebSocket not connected");

            pendingResponse = new TaskCompletionSource<WebSocketMidiCommandMessage>(
                TaskCreationOptions.RunContinuationsAsynchronously);

            // Send
            var data = Encoding.UTF8.GetBytes(message.Serialise());
            await client.SendAsync(
                new ArraySegment<byte>(data),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);

            // Wait for response or timeout
            var completed = await Task.WhenAny(
                pendingResponse.Task,
                Task.Delay(timeoutMs));

            if (completed != pendingResponse.Task)
                return null; // timeout

            return await pendingResponse.Task;
        }

        // --------------------------------------------------
        // RECEIVE LOOP
        // --------------------------------------------------
        private async Task ReceiveLoop()
        {
            var buffer = new byte[1024 * 10];

            try
            {
                while (!cts.Token.IsCancellationRequested &&
                       client.State == WebSocketState.Open)
                {
                    using var ms = new MemoryStream();
                    WebSocketReceiveResult result;

                    do
                    {
                        result = await client.ReceiveAsync(
                            new ArraySegment<byte>(buffer),
                            cts.Token);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await client.CloseAsync(
                                WebSocketCloseStatus.NormalClosure,
                                "Closing",
                                CancellationToken.None);
                            return;
                        }

                        ms.Write(buffer, 0, result.Count);

                    } while (!result.EndOfMessage);

                    // Handle text message
                    ms.Seek(0, SeekOrigin.Begin);
                    using var reader = new StreamReader(ms, Encoding.UTF8);
                    var message = await reader.ReadToEndAsync();

                    var deserialised =
                        WebSocketMidiCommandMessage.Deserialise(message);

                    pendingResponse?.TrySetResult(deserialised);
                }
            }
            catch (OperationCanceledException)
            {
                // expected on shutdown
            }
            catch (Exception ex)
            {
                pendingResponse?.TrySetException(ex);
            }
        }
    }
}