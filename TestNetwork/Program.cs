
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        static void Main(string[] args)
        {
            //if (false)
            {
                using (CenaPlusServerHost host = new CenaPlusServerHost(9999))
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
            
        }
    }
}
