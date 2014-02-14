using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;

namespace CenaPlus.Network
{
    public interface IJudgeNodeChannel : IJudgeNode, IClientChannel
    {
    }

    public static class JudgeNodeChannelFactory
    {
        private static ServiceEndpoint GetServiceEndPoint(IPEndPoint server)
        {
            ServiceEndpoint endpoint = new ServiceEndpoint(ContractDescription.GetContract(typeof(ICenaPlusServer)));
            Uri address = new UriBuilder("net.tcp", server.Address.ToString(), server.Port, "/JudgeNode").Uri;
            endpoint.Address = new EndpointAddress(address);
            endpoint.Binding = new NetTcpBinding(SecurityMode.None);
            return endpoint;
        }

        public static IJudgeNodeChannel CreateChannel(IPEndPoint server)
        {
            IJudgeNodeChannel channel;
            var factory = new ChannelFactory<IJudgeNodeChannel>(GetServiceEndPoint(server));
            channel = factory.CreateChannel();

            var version = typeof(IJudgeNode).Assembly.GetName().Version;
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
