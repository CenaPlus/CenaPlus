using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using CenaPlus.Network;

namespace CenaPlus.Server.Bll
{
    public class CenaPlusServerHost : ServiceHost
    {
        public CenaPlusServerHost(int port)
            : base(typeof(LocalCenaServer))
        {
            
            Uri uri = new UriBuilder("net.tcp", "localhost", port, "/CenaPlusServer").Uri;
            base.AddServiceEndpoint(typeof(ICenaPlusServer), new NetTcpBinding(SecurityMode.None), uri.ToString());
        }
    }
}
