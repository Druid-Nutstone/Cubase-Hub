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

        public bool Connected { get; private set; } = false;

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
            {
                this.Connected = true;
                return true;
            }

            try
            {
                using var cts = new CancellationTokenSource();
                // Set the timeout duration
                cts.CancelAfter(TimeSpan.FromSeconds(20));

                await client.ConnectAsync(
                    new Uri($"ws://{ipAddress}:{port}/ws"),
                    cts.Token);
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
            this.Connected = true;

            return true;
        }

        // --------------------------------------------------
        // PUBLIC API
        // --------------------------------------------------
        public async Task<CubaseRemoteMidiMacroCollection?> GetMacroCollection(Action<string> onError)
        {
            var response = await SendAndWait(
                WebSocketMidiCommandMessage.CreateFromCommand(
                    WebSocketMidiCommand.MidiCommandList), onError);

            return response?.GetMacroCollection();
        }

        public async Task<bool> StartTransportMonitoring(Action<string> onError)
        {
            var response = await SendAndWait(
                WebSocketMidiCommandMessage.CreateFromCommand(
                    WebSocketMidiCommand.MidiLyricStartTransportMonitoring), onError);
            return true;
        }

        public async Task<bool> StopTransportMonitoring(Action<string> onError)
        {
            var response = await SendAndWait(
                WebSocketMidiCommandMessage.CreateFromCommand(
                    WebSocketMidiCommand.MidiLyricStopTransportMonitoring), onError);
            return true;
        }

        public async Task<TransportLocationCollection?> GetTransportLocation(Action<string> onError)
        {
            var response = await SendAndWait(
                WebSocketMidiCommandMessage.CreateFromCommand(
                    WebSocketMidiCommand.MidiTransportLocation), onError);
            return response?.GetTransportLocationCollection();
        }

        public async Task<LyricResponseModel?> GetCurrentLyric(Action<string> onError)
        {
            var response = await SendAndWait(
                WebSocketMidiCommandMessage.CreateFromCommand(
                    WebSocketMidiCommand.MidiLyricCurrentProject), onError);
            return response?.GetLyricResponseModel();
        }

        public async Task<LyricIndexCollection?> GetLyricIndex(Action<string> onError)
        {
            var response = await SendAndWait(
                WebSocketMidiCommandMessage.CreateFromCommand(
                    WebSocketMidiCommand.MidiLyricIndex), onError);
            return response?.GetLyricIndex();
        }

        public async Task<LyricContent?> GetLyricContent(Lyric lyric, Action<string> onError)
        {
            var response = await SendAndWait(
                WebSocketMidiCommandMessage.CreateLyricContentRequest(lyric), onError);
            return response?.GetLyricContent();
        }

        public async Task<WebSocketMidiCommandMessage> SendMidiCommand(CubaseKeyCommand cubaseKeyCommand, Action<string> onError)
        {
            return await SendAndWait(WebSocketMidiCommandMessage.CreateFromKeyCommand(cubaseKeyCommand), onError);
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
            WebSocketMidiCommandMessage message, Action<string> onError,
            int timeoutMs = 5000)
        {
            if (!IsConnected)
            {
                onError.Invoke("Web socket is NOT connected");
                return WebSocketMidiCommandMessage.CreateError("Web socket not connected");
            }
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