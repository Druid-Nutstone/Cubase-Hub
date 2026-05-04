using Cubase.Macro.Common.Socket;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

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

            var client = new CubaseMacroWebSocketClient("localhost");

            var connected = await client.Connect();
            if (connected)
            {

            }

        }
    
    }
}
