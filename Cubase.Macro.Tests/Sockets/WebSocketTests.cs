using Cubase.Macro.Common.Socket;

namespace Cubase.Macro.Tests.Sockets
{
    [TestClass]
    public class WebSocketTests
    {

        [TestMethod]
        public async Task Can_Connect_To_WebSocket()
        {
            var host = Cubase.Macro.Program.CreateHostApiAndServices();
            await host.StartAsync();

            await Task.Delay(200); // important

            var client = new CubaseMacroWebSocketClient(null);

            var connected = await client.Connect("locahost", (msg) => { });
            if (connected)
            {
                var macroCollection = await client.GetMacroCollection((e) => { });
            }

        }

    }
}
