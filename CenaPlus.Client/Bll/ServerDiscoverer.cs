using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using CenaPlus.Network;
namespace CenaPlus.Client.Bll
{
    class IntranetServer
    {
        public string Name { get; set; }
        public IPEndPoint Location { get; set; }
    }

    class ServerDiscoverer
    {
        private static readonly IPEndPoint SSDP_ENDPOINT = new IPEndPoint(IPAddress.Parse("239.255.255.250"), 1900);
        private static string versionExpected;

        static ServerDiscoverer()
        {
            var version = typeof(ICenaPlusServer).Assembly.GetName().Version;
            versionExpected = version.Major + "." + version.Minor;
        }

        public event Action<IntranetServer> FoundServer;
        private UdpClient client;

        public void Start()
        {
            client = new UdpClient();
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            client.Client.Bind(new IPEndPoint(IPAddress.Any, SSDP_ENDPOINT.Port));
            client.JoinMulticastGroup(SSDP_ENDPOINT.Address);

            client.BeginReceive(ProcessPackage, client);
        }

        private void ProcessPackage(IAsyncResult result)
        {
            UdpClient client = (UdpClient)result.AsyncState;
            IPEndPoint remote = null;
            byte[] bytes ;
            try
            {
                bytes = client.EndReceive(result, ref remote);
            }
            catch (ObjectDisposedException) // Closed already
            {
                return;
            }

            string ssdp;
            try
            {
                ssdp = Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                client.BeginReceive(ProcessPackage, client);
                return;
            }

            string[] lines = ssdp.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            string serverVersion = null;
            string serverName = null;
            int? serverPort = null;
            foreach (var line in lines)
            {
                if (line.StartsWith("NT: CenaPlusServer:"))
                {
                    serverVersion = line.Substring("NT: CenaPlusServer:".Length);
                }
                else if (line.StartsWith("SERVER: "))
                {
                    serverName = line.Substring("SERVER: ".Length);
                }
                else if (line.StartsWith("LOCATION: "))
                {
                    try
                    {
                        serverPort = int.Parse(line.Substring("LOCATION: ".Length));
                    }
                    catch
                    {
                        client.BeginReceive(ProcessPackage, client);
                        return;
                    }
                }
            }

            if (serverVersion == versionExpected && serverName != null && serverPort != null)
            {
                if (FoundServer != null)
                    FoundServer(new IntranetServer
                    {
                        Name = serverName,
                        Location = new IPEndPoint(remote.Address, (int)serverPort)
                    });
            }

            client.BeginReceive(ProcessPackage, client);
        }

        public void Stop()
        {
            client.Close();
        }
    }
}
