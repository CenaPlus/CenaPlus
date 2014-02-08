using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Net.NetworkInformation;
using CenaPlus.Network;

namespace CenaPlus.Server.Bll
{
    public class CenaPlusServerHost : ServiceHost
    {
        private const string SSDP_FORMAT = "NOTIFY * HTTP/1.1\r\n"
            + "HOST: 239.255.255.250:1900\r\n"
            + "CACHE-CONTROL: max-age=120\r\n"
            + "NT: CenaPlusServer:{0}\r\n"
            + "NTS: ssdp:alive\r\n"
            + "USN: uid:ee136b0e-5595-4b92-9b6e-efa12663b8a4\r\n"
            + "SERVER: {1}\r\n"
            + "LOCATION: {2}";
        private static readonly IPEndPoint SSDP_ENDPOINT = new IPEndPoint(IPAddress.Parse("239.255.255.250"), 1900);

        private string serverName;
        private int port;

        public CenaPlusServerHost(int port, string serverName)
            : base(typeof(LocalCenaServer))
        {
            this.serverName = serverName;
            this.port = port;

            Uri uri = new UriBuilder("net.tcp", "localhost", port, "/CenaPlusServer").Uri;
            base.AddServiceEndpoint(typeof(ICenaPlusServer), new NetTcpBinding(SecurityMode.None), uri.ToString());
        }

        protected override void OnOpened()
        {
            base.OnOpened();

            var version = typeof(ICenaPlusServer).Assembly.GetName().Version;
            var versionStr = version.Major + "." + version.Minor;
            var ssdpMsg = Encoding.UTF8.GetBytes(string.Format(SSDP_FORMAT, versionStr, serverName, port));

            UdpClient udp = new UdpClient();
            udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udp.Client.Bind(new IPEndPoint(IPAddress.Any, port));
            udp.JoinMulticastGroup(SSDP_ENDPOINT.Address);

            var thread = new Thread(() =>
            {
                try
                {
                    while (base.State != CommunicationState.Closed)
                    {
                        udp.Send(ssdpMsg, ssdpMsg.Length, SSDP_ENDPOINT);
                        Thread.Sleep(5 * 1000);
                    }
                }
                catch { }
            });
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
