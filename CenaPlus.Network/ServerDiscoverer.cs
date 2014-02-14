using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using CenaPlus.Network;
namespace CenaPlus.Network
{
    public class IntranetServer
    {
        public string Name { get; set; }
        public IPEndPoint Location { get; set; }
    }

    public class ServerDiscoverer
    {
        private static readonly IPEndPoint SSDP_ENDPOINT = new IPEndPoint(IPAddress.Parse("239.255.255.250"), 1900);

        private string expectedSpecifier;
        public Type ExpectedService { get; set; }
        public event Action<IntranetServer> FoundServer;
        private UdpClient client;

        public void Start()
        {
            var version = ExpectedService.Assembly.GetName().Version;
            var versionStr = version.Major + "." + version.Minor;
            expectedSpecifier = ExpectedService.Name + ":" + versionStr;

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
            byte[] bytes;
            try
            {
                bytes = client.EndReceive(result, ref remote);
                client.BeginReceive(ProcessPackage, client);
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
                return;
            }

            string[] lines = ssdp.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var splited = lines.Select(l => l.Split(new string[] { ": " }, 2, StringSplitOptions.None));

            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var kv in splited)
            {
                if (kv.Length == 2)
                {
                    dict.Add(kv[0], kv[1]);
                }
            }

            IntranetServer found = new IntranetServer();

            if (!dict.ContainsKey("NT")) return;
            if (dict["NT"] != expectedSpecifier) return;

            if (!dict.ContainsKey("SERVER")) return;
            found.Name = dict["SERVER"];

            if (!dict.ContainsKey("LOCATION")) return;
            int serverPort;
            if (!int.TryParse(dict["LOCATION"], out serverPort)) return;
            found.Location = new IPEndPoint(remote.Address, serverPort);

            if (FoundServer != null)
            {
                FoundServer(found);
            }
        }

        public void Stop()
        {
            client.Close();
        }
    }
}
