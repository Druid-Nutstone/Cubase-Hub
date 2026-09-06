using Cubase.Macro.Common.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;

namespace Cubase.Macro.Common.Socket
{
    public class CubaseMacroWebSocketClient : IDisposable
    {
        private ClientWebSocket client;
        private Task? receiveTask;
        private CancellationTokenSource cts = new();

        private readonly ILogger<CubaseMacroWebSocketClient> logger;

        private Task heartbeatTask;

        private TaskCompletionSource<WebSocketMidiCommandMessage>? pendingResponse;

        public bool Connected
        {
            get
            {
                return client?.State == WebSocketState.Open;
            }
            private set;
        } = false;

        public CubaseMacroWebSocketClient(ILogger<CubaseMacroWebSocketClient> logger) : base()
        {
            this.logger = logger;
        }

        public void Dispose()
        {
            try
            {
                cts.Cancel();
                if (client != null)
                {
                    client.Dispose();
                }
            }
            catch { }
        }

        // --------------------------------------------------
        // CONNECT
        // --------------------------------------------------
        public async Task<bool> Connect(string ipAddress, Action<string> errorHandler, int port = 8014)
        {
            if (this.client != null && this.client.State == WebSocketState.Open)
            {
                this.Connected = true;
                return true;
            }

            // 2. If it's closed or aborted, we MUST dispose it and create a new one
            if (this.client != null)
            {
                // some sort of other error occurred, so we need to dispose the client and create a new one
                this.client.Dispose();
            }

            // Reset/Recreate the client and the cancellation token source

            this.client = new ClientWebSocket();
            this.client.Options.KeepAliveInterval = TimeSpan.FromSeconds(20);
            this.client.Options.KeepAliveInterval = TimeSpan.Zero;
            var ctsConnect = new CancellationTokenSource();

            try
            {
                // Set the timeout duration
                ctsConnect.CancelAfter(TimeSpan.FromSeconds(5));

                await this.client.ConnectAsync(
                    new Uri($"ws://{ipAddress}:{port}/ws"),
                    ctsConnect.Token);
            }
            catch (Exception ex)
            {
                errorHandler.Invoke(ex.Message);
                logger.LogError(ex, "Error occurred while connecting to WebSocket");
                return false;
            }
            if (this.client.State != WebSocketState.Open)
            {
                logger.LogError($"Failed to connect to WebSocket. web socket state {this.client.State}");
                return false;
            }
            logger.LogInformation($"Connected to WebSocket at ws://{ipAddress}:{port}/ws");
            // Start receive loop on background thread
            receiveTask = ReceiveLoop();
            this.Connected = true;
            //this.StartHeartbeat();
            return true;
        }


        // not used currently !!
        private void StartHeartbeat()
        {
            heartbeatTask = Task.Run(async () =>
            {
                while (this.client.State == WebSocketState.Open)
                {
                    try
                    {
                        var heartbeatRespone = await SendAndWait(
                                       WebSocketMidiCommandMessage.CreateFromCommand(
                                          WebSocketMidiCommand.MidiHeartBeat), (err) => { }); ;

                        Debug.WriteLine($"{DateTime.Now.ToString()} Heartbeat sent, response: {heartbeatRespone?.Message}");
                        await Task.Delay(
                            TimeSpan.FromSeconds(20)); // keep it under 30 seconds to avoid timeout on the server side
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Heartbeat failed: {ex.Message}");
                        break;
                    }
                }
            });
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

        public async Task<CubaseMidiProjectStatus?> GetProjectStatus(Action<string> onError)
        {
            var response = await SendAndWait(
                WebSocketMidiCommandMessage.CreateFromCommand(
                    WebSocketMidiCommand.MidiProjectStatus), onError);
            return response?.GetCubaseMidiProjectStatus();
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
            if (!this.Connected)
            {
                onError.Invoke("Web socket is NOT connected");
                return WebSocketMidiCommandMessage.CreateError("Web socket not connected");
            }

            /*
            if (this.State != WebSocketState.Open)
            {
                onError.Invoke("Web socket is NOT open");
                return WebSocketMidiCommandMessage.CreateError("Web socket not open");
            }
            */

            pendingResponse = new TaskCompletionSource<WebSocketMidiCommandMessage>(
                TaskCreationOptions.RunContinuationsAsynchronously);

            // Send
            var data = Encoding.UTF8.GetBytes(message.Serialise());

            try
            {

                await this.client.SendAsync(
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
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occurred while sending message: {ex.Message}");
            }

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
                       this.client.State == WebSocketState.Open)
                {
                    using var ms = new MemoryStream();
                    WebSocketReceiveResult result;

                    do
                    {
                        result = await this.client.ReceiveAsync(
                            new ArraySegment<byte>(buffer),
                            cts.Token);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await this.client.CloseAsync(
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
                Debug.WriteLine("Operation canceled");
                logger.LogInformation("WebSocket operation canceled");
                // expected on shutdown
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred in receive loop");
                pendingResponse?.TrySetException(ex);
            }
            logger.LogWarning($"Receive loop exited, WebSocket state: {this.client.State}. is CTS Token cancelled? {cts.Token.IsCancellationRequested}");
        }
    }
}