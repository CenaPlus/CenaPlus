
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
            IPEndPoint SSDP_ENDPOINT = new IPEndPoint(IPAddress.Parse("239.255.255.250"), 1900);
            if (true)
            {
                byte[] bytes = new byte[] { (byte)'H', (byte)'e' };
                var udp = new UdpClient();
                udp.Client.Bind(new IPEndPoint(IPAddress.Any, 9980));
                udp.JoinMulticastGroup(IPAddress.Parse("239.255.255.250"));
                new Thread(() =>
                {
                    while (true)
                    {
                        udp.Send(bytes, 2, SSDP_ENDPOINT); Thread.Sleep(1 * 1000);
                    }
                }).Start();
            }
            else
            {
                var client = new UdpClient();
                client.Client.Bind(new IPEndPoint(IPAddress.Any, 1900));
                client.JoinMulticastGroup(SSDP_ENDPOINT.Address);
                while (true)
                {
                    IPEndPoint remote = null;
                    byte[] mssg = client.Receive(ref remote);
                    Console.OpenStandardOutput().Write(mssg, 0, mssg.Length);
                    Console.WriteLine();
                }
            }
            /*
            string msg = "CenaPlusNotify\n1.0.0\nHighPerformanceCenaPlusServer";
            var broadcastAddrs = from iface in NetworkInterface.GetAllNetworkInterfaces()
                             where iface.NetworkInterfaceType != NetworkInterfaceType.Loopback
                                && iface.OperationalStatus == OperationalStatus.Up
                             let properties = iface.GetIPProperties()
                             from unicast in properties.UnicastAddresses
                             where unicast.Address.AddressFamily == AddressFamily.InterNetwork
                             select GetBroadcastAddress(unicast.Address, unicast.IPv4Mask);
            foreach (var a in broadcastAddrs) Console.WriteLine(a);
            return;
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sock.Bind(new IPEndPoint(IPAddress.Loopback, 9999));
            sock.SendTo(Encoding.UTF8.GetBytes(msg), new IPEndPoint(IPAddress.Parse("192.168.0.255"), 9980));
            sock.Close();
            return;
            //if (false)
            {
                using (CenaPlusServerHost host = new CenaPlusServerHost(9999,"DB Server"))
                {

                    host.Opened += (x, y) =>
                    {
                        Console.WriteLine("Opened");
                        {
                            CenaPlusServerProxy proxy = new CenaPlusServerProxy(new IPEndPoint(IPAddress.Loopback, 9999), new Ca());
                            foreach (var i in proxy.GetContestList())
                            {
                                Console.WriteLine(i);
                            }
                            proxy.GetVersion();
                            proxy.GetVersion();
                            proxy.GetVersion();
                            proxy.GetVersion();
                            proxy.Close();
                            proxy = new CenaPlusServerProxy(new IPEndPoint(IPAddress.Loopback, 9999), new Ca());
                            proxy.GetVersion(); proxy.GetVersion(); proxy.GetVersion(); proxy.GetVersion(); proxy.GetVersion();
                            Console.WriteLine(proxy.GetContest(1).Title);
                            proxy.Close();
                        }
                    };
                    host.Open();

                    Console.WriteLine("Closing service");
                }
            }
            // else
            */
        }
    }
}
