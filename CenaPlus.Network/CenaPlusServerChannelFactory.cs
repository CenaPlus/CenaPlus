using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Net;
using CenaPlus.Entity;
namespace CenaPlus.Network
{
    public interface ICenaPlusServerChannel : ICenaPlusServer, IClientChannel
    {
    }

    public static class CenaPlusServerChannelFactory
    {
        private static DuplexChannelFactory<ICenaPlusServerChannel> factory;

        private static ServiceEndpoint GetServiceEndPoint(IPEndPoint server)
        {
            ServiceEndpoint endpoint = new ServiceEndpoint(ContractDescription.GetContract(typeof(ICenaPlusServer)));
            Uri address = new UriBuilder("net.tcp", server.Address.ToString(), server.Port, "/CenaPlusServer").Uri;
            endpoint.Address = new EndpointAddress(address);
            endpoint.Binding = new NetTcpBinding(SecurityMode.None);
            return endpoint;
        }

        public static ICenaPlusServerChannel CreateChannel(IPEndPoint server, ICenaPlusServerCallback callback)
        {
            ICenaPlusServerChannel channel;
            factory = new DuplexChannelFactory<ICenaPlusServerChannel>(callback, GetServiceEndPoint(server));
            channel = factory.CreateChannel();

            var version = typeof(ICenaPlusServer).Assembly.GetName().Version;
            string clientVersion = version.Major + "." + version.Minor;
            string serverVersion = channel.GetVersion();
            if (clientVersion != serverVersion)
            {
                channel.Close();
                throw new Exception(string.Format("Version of client({0}) does not match version of server({1}).", clientVersion, serverVersion));
            }
            return channel;
        }
    }
}
