using Cubase.Macro.Common.Models;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Cubase.Macro.Common.Socket
{
    public class CubaseMacroWebSocketClient : IDisposable
    {
        private ClientWebSocket client;
        private Task? receiveTask;
        private CancellationTokenSource cts = new();

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
                ctsConnect.CancelAfter(TimeSpan.FromSeconds(10));

                await this.client.ConnectAsync(
                    new Uri($"ws://{ipAddress}:{port}/ws"),
                    ctsConnect.Token);
            }
            catch (Exception ex)
            {
                errorHandler.Invoke(ex.Message);
                return false;
            }
            if (this.client.State != WebSocketState.Open)
                return false;

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
            } catch (Exception ex) {
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
                // expected on shutdown
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occurred in receive loop: {ex.Message}");
                pendingResponse?.TrySetException(ex);
            }
        }
    }
}