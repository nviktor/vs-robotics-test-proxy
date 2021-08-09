using Microsoft.Extensions.Configuration;
using Proxy.Config;
using System;
using System.IO;
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

            var config = new AppConfig();
            configuration.Bind(config);

            //Console.WriteLine($"Server ports: {config.SpeechServerPorts}");

            var listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 6482);
            listener.Start();

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                client.NoDelay = true;

                // каким то образом выбираем, к какому серверу подключаться, получаем из списка его имя и порт

                var server = new TcpClient();
                server.Connect("127.0.0.1", 4830);
                server.NoDelay = true;


                var clientStream = client.GetStream();
                var serverStream = server.GetStream();

                await Task.WhenAny(clientStream.CopyToAsync(serverStream), serverStream.CopyToAsync(clientStream));
            }
        }
    }
}
