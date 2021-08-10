using System;
using System.Linq;
using System.Threading;

namespace Proxy
{
    internal class Servers
    {
        private volatile ServerNode _activeNode;

        public Servers(int[] serverPorts)
        {
            if (serverPorts == null || !serverPorts.Any())
            {
                throw new ArgumentOutOfRangeException(nameof(serverPorts));
            }

            var firstNode = _activeNode = new ServerNode
            {
                Port = serverPorts[0]
            };

            for (var i = 1; i < serverPorts.Length; i++)
            {
                var node = new ServerNode
                {
                    Port = serverPorts[i]
                };

                _activeNode.Next = node;
                _activeNode = node;
            }

            _activeNode.Next = firstNode;
        }

        public int ResolveHandlingServerPort()
        {
            var node = Interlocked.CompareExchange(ref _activeNode, _activeNode.Next, _activeNode);

            return node.Port;
        }

        private class ServerNode
        {
            public int Port { get; set; }

            public ServerNode Next { get; set; }
        }

    }
}
