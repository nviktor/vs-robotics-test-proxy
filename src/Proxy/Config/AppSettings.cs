namespace Proxy.Config
{
    internal class AppSettings
    {
        public string IpAddress { get; set; }
        public int IncomingPort { get; set; }
        public int[] SpeechServerPorts { get; set; }
    }
}
