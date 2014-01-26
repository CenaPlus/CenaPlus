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
using CenaPlus.Network;
namespace CenaPlus.Client.Bll
{
    public class CenaPlusServerProxy : ClientBase<ICenaPlusServer>, ICenaPlusServer
    {

        private static ServiceEndpoint GetServiceEndPoint(IPEndPoint server)
        {
            ServiceEndpoint endpoint = new ServiceEndpoint(ContractDescription.GetContract(typeof(ICenaPlusServer)));
            Uri address = new UriBuilder("net.tcp", server.Address.ToString(), server.Port, "/CenaPlusServer").Uri;
            endpoint.Address = new EndpointAddress(address);
            endpoint.Binding = new NetTcpBinding(SecurityMode.None);
            return endpoint;
        }

        public CenaPlusServerProxy(IPEndPoint server, ICenaPlusServerCallback callback)
            : base(new InstanceContext(callback), GetServiceEndPoint(server))
        {
            // Check version
            var fileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            string clientVersion = fileVersion.FileMajorPart + "." + fileVersion.FileMinorPart;
            string serverVersion = GetVersion();
            if (clientVersion != serverVersion)
            {
                throw new Exception(string.Format("Version of client({0}) does not match version of server({1}).", clientVersion, serverVersion));
            }
        }

        public string GetVersion()
        {
            return Channel.GetVersion();
        }

        public List<int> GetContestList()
        {
            return Channel.GetContestList();
        }

        public Contest GetContest(int id)
        {
            return Channel.GetContest(id);
        }

        public bool Authenticate(string userName, string password)
        {
            return Channel.Authenticate(userName, password);
        }

        public List<int> GetProblemList(int contestID)
        {
            return Channel.GetProblemList(contestID);
        }


        public Problem GetProblem(int id)
        {
            return Channel.GetProblem(id);
        }
    }
}
