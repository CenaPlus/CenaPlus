using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using CenaPlus.Network;

namespace CenaPlus.Server.Bll
{
    public class CenaPlusServerHost : ServiceHost
    {
        public CenaPlusServerHost(int port)
            : base(typeof(LocalCenaServer),new UriBuilder("net.tcp", "localhost", port, "/CenaPlusServer").Uri)
        {
        }
    }
}
