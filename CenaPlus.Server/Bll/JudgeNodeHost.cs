using CenaPlus.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace CenaPlus.Server.Bll
{
    class JudgeNodeHost:ServiceHost
    {
        private ServiceBroadcaster broadcaster;

        public JudgeNodeHost(int port, string serverName)
            : base(typeof(JudgeNode))
        {
            Uri uri = new UriBuilder("net.tcp", "localhost", port, "/JudgeNode").Uri;
            base.AddServiceEndpoint(typeof(IJudgeNode), new NetTcpBinding(SecurityMode.None), uri.ToString());

            broadcaster = new ServiceBroadcaster()
            {
                ServiceType = typeof(IJudgeNode),
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
