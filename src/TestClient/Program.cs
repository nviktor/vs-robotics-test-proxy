using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var proxy = new TcpClient("127.0.0.1", 6482);
            using var file = File.OpenRead("12345.wav");
            await file.CopyToAsync(proxy.GetStream());
        }
    }
}
