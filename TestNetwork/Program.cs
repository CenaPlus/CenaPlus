
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using CenaPlus.Server.Bll;
using CenaPlus.Network;
using CenaPlus.Client.Bll;
using CenaPlus.Server.Dal;
namespace TestNetwork
{
    class Ca : ICenaPlusServerCallback
    {

        public void Bye()
        {
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
