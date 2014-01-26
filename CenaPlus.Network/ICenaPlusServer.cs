using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using CenaPlus.Entity;

namespace CenaPlus.Network
{
    /// <summary>
    /// Represents all services provided by a CenaPlus server, both local and remote.
    /// </summary>
    [ServiceContract(CallbackContract = typeof(ICenaPlusServerCallback))]
    public interface ICenaPlusServer
    {
        /// <summary>
        /// Get CenaPlus version of the server.
        /// </summary>
        /// <returns>MajarVersion.MinorVersion</returns>
        [OperationContract]
        string GetVersion();

        /// <summary>
        /// Perform authentication
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>whether authentication success</returns>
        [OperationContract]
        bool Authenticate(string userName, string password);

        /// <summary>
        /// List all contests on the server.
        /// </summary>
        /// <returns>List of contest ids</returns>
        [OperationContract]
        List<int> GetContestList();

        /// <summary>
        /// Get contest by id
        /// </summary>
        /// <param name="id">contest id</param>
        /// <returns>contest information</returns>
        [OperationContract]
        Contest GetContest(int id);
    }
}
