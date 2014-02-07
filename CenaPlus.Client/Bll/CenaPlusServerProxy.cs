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
    public class CenaPlusServerProxy : DuplexClientBase<ICenaPlusServer>, ICenaPlusServer
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

        public int Submit(int problemID, string code, ProgrammingLanguage language)
        {
            return Channel.Submit(problemID, code, language);
        }

        public List<int> GetRecordList(int contestID)
        {
            return Channel.GetRecordList(contestID);
        }

        public Record GetRecord(int id)
        {
            return Channel.GetRecord(id);
        }

        public List<int> GetUserList()
        {
            return Channel.GetUserList();
        }

        public User GetUser(int id)
        {
            return Channel.GetUser(id);
        }

        public void UpdateUser(int id, string name, string nickname, string password, UserRole? role)
        {
            Channel.UpdateUser(id, name, nickname, password, role);
        }

        public void CreateUser(string name, string nickname, string password, UserRole role)
        {
            Channel.CreateUser(name, nickname, password, role);
        }

        public void DeleteUser(int id)
        {
            Channel.DeleteUser(id);
        }


        public List<int> GetOnlineList()
        {
            return Channel.GetOnlineList();
        }


        public void Kick(int userID)
        {
            Channel.Kick(userID);
        }


        public List<int> GetQuestionList(int contestID)
        {
            return Channel.GetQuestionList(contestID);
        }

        public Question GetQuestion(int id)
        {
            return Channel.GetQuestion(id);
        }

        public int AskQuestion(int contestID, string description)
        {
            return Channel.AskQuestion(contestID, description);
        }


        public void DeleteContest(int id)
        {
            Channel.DeleteContest(id);
        }

        public void UpdateContest(int id, string title, string description, DateTime? startTime, DateTime? endTime, ContestType? type)
        {
            Channel.UpdateContest(id, title, description, startTime, endTime, type);
        }


        public int CreateProblem(int contestID, string title, string content, int score, int timeLimit, long memoryLimit, string std, string spj, string validator, ProgrammingLanguage? stdLanguage, ProgrammingLanguage? spjLanguage, ProgrammingLanguage? validatorLanguage)
        {
            return Channel.CreateProblem(contestID, title, content, score, timeLimit, memoryLimit, std, spj, validator, stdLanguage, spjLanguage, validatorLanguage);
        }

        public void UpdateProblem(int id, string title, string content, int? score, int? timeLimit, long? memoryLimit, string std, string spj, string validator, ProgrammingLanguage? stdLanguage, ProgrammingLanguage? spjLanguage, ProgrammingLanguage? validatorLanguage)
        {
            Channel.UpdateProblem(id,title, content, score, timeLimit, memoryLimit, std, spj, validator, stdLanguage, spjLanguage, validatorLanguage);
        }

        public void DeleteProblem(int id)
        {
            Channel.DeleteProblem(id);
        }

        public void Rejudge(int recordID)
        {
            Channel.Rejudge(recordID);
        }

        public List<int> GetTestCaseList(int problemID)
        {
            return Channel.GetTestCaseList(problemID);
        }

        public TestCase GetTestCase(int id)
        {
            return Channel.GetTestCase(id);
        }

        public void DeleteTestCase(int id)
        {
            Channel.DeleteTestCase(id);
        }

        public void UpdateTestCase(int id, byte[] input, byte[] output, TestCaseType? type)
        {
            Channel.UpdateTestCase(id, input, output, type);
        }

        public int CreateTestCase(int problemID, byte[] input, byte[] output, TestCaseType type)
        {
            return Channel.CreateTestCase(problemID, input, output, type);
        }
    }
}
