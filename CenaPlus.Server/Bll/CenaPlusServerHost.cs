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
        private ServiceBroadcaster broadcaster;

        public CenaPlusServerHost(int port, string serverName)
            : base(typeof(LocalCenaServer))
        {
            Uri uri = new UriBuilder("net.tcp", "localhost", port, "/CenaPlusServer").Uri;
            base.AddServiceEndpoint(typeof(ICenaPlusServer), new NetTcpBinding(SecurityMode.None), uri.ToString());

            broadcaster = new ServiceBroadcaster()
            {
                ServiceType = typeof(ICenaPlusServer),
                ServerName = serverName,
                Port = port
            };
        }

        protected override void OnOpened()
        {
            base.OnOpened();
            broadcaster.Start();
        }

        protected override void OnClosed()
        {
            base.OnClosed();
            broadcaster.Stop();
        }
    }
}
