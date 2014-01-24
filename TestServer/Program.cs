using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using ProtoBuf;
using TestContract;
namespace TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread a = new Thread(() =>
            {
                Thread.Sleep(1000);
                using (Socket sock = new Socket(SocketType.Stream, ProtocolType.Tcp))
                {
                    sock.Connect("localhost", 9999);
                    NetworkStream stream = new NetworkStream(sock);
                    Problem problem = new Problem
                    {
                        ID = Guid.NewGuid(),
                        Name = "Foobar",
                        Content = "bar bar"
                    };
                    Serializer.Serialize(stream, problem);
                }
            });
            a.Start();
            using (Socket sock = new Socket(SocketType.Stream, ProtocolType.Tcp))
            {
                sock.Bind(new IPEndPoint(IPAddress.Loopback, 9999));
                sock.Listen(5);
                using (Socket client = sock.Accept())
                {
                    NetworkStream stream = new NetworkStream(client);
                    Problem p =Serializer.Deserialize<Problem>(stream);
                    Console.WriteLine("ID=" + p.ID.ToString() + " Name=" + p.Name + " Content=" + p.Content);
                }
            }
            a.Join();
        }
    }
}
