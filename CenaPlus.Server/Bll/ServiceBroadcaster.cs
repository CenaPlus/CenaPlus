using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CenaPlus.Server.Bll
{
    class ServiceBroadcaster
    {
        private const string SSDP_FORMAT = "NOTIFY * HTTP/1.1\r\n"
            + "HOST: 239.255.255.250:1900\r\n"
            + "CACHE-CONTROL: max-age=120\r\n"
            + "NT: {0}:{1}\r\n"
            + "NTS: ssdp:alive\r\n"
            + "USN: uid:ee136b0e-5595-4b92-9b6e-efa12663b8a4\r\n"
            + "SERVER: {2}\r\n"
            + "LOCATION: {3}";
        private static readonly IPEndPoint SSDP_ENDPOINT = new IPEndPoint(IPAddress.Parse("239.255.255.250"), 1900);

        private volatile bool IsStopped;

        public Type ServiceType { get; set; }
        public string ServerName { get; set; }
        public int Port { get; set; }
        public int Interval { get; set; }

        public ServiceBroadcaster()
        {
            Interval = 5 * 1000;
        }

        public void Start()
        {
            var version = ServiceType.Assembly.GetName().Version;
            var versionStr = version.Major + "." + version.Minor;
            var ssdpMsg = Encoding.UTF8.GetBytes(string.Format(SSDP_FORMAT, ServiceType.Name, versionStr, ServerName, Port));

            UdpClient udp = new UdpClient();
            udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udp.Client.Bind(new IPEndPoint(IPAddress.Any, Port));
            udp.JoinMulticastGroup(SSDP_ENDPOINT.Address);

            IsStopped = false;
            var thread = new Thread(() =>
            {
                try
                {
                    while (!IsStopped)
                    {
                        udp.Send(ssdpMsg, ssdpMsg.Length, SSDP_ENDPOINT);
                        Thread.Sleep(Interval);
                    }
                }
                catch
                { }
                finally
                {
                    try { udp.Close(); }
                    catch { }
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

        public void Stop()
        {
            IsStopped = true;
        }
    }
}
