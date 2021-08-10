using Microsoft.Extensions.Configuration;
using Proxy.Config;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Proxy
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();

            var config = new AppSettings();
            configuration.Bind(config);

            var servers = new Servers(config.SpeechServerPorts);

            var listener = new TcpListener(IPAddress.Parse(config.IpAddress), config.IncomingPort);
            listener.Start();

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                client.NoDelay = true;

                // каким то образом выбираем, к какому серверу подключаться, получаем из списка его имя и порт
                var port = servers.ResolveHandlingServerPort();

                var server = new TcpClient();
                server.Connect(config.IpAddress, port);
                server.NoDelay = true;

                var clientStream = client.GetStream();
                var serverStream = server.GetStream();

                Task.Run(() => clientStream.CopyToAsync(serverStream));
                Task.Run(() => serverStream.CopyToAsync(clientStream));

                //await Task.WhenAny(clientStream.CopyToAsync(serverStream), serverStream.CopyToAsync(clientStream));
            }
        }
    }
}
