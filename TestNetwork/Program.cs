
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
        public void PopAd(string ad)
        {
            Console.WriteLine(ad);
        }
    }

    class Program
    {
        private static IPAddress GetBroadcastAddress(IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }
            return new IPAddress(broadcastAddress);
        }

        static void Main(string[] args)
        {
            CenaPlusServerProxy proxy = new CenaPlusServerProxy(new IPEndPoint(IPAddress.Loopback,9999),new Ca());
            try
            {
                proxy.GetUserList();
            }
            catch (FaultException<AccessDeniedError> e)
            {
                Console.WriteLine("access");
            }
        }
    }
}
