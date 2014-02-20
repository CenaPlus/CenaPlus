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
        [FaultContract(typeof(AlreadyLoggedInError))]
        bool Authenticate(string userName, string password);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        User GetProfile();

        /// <summary>
        /// List all contests on the server.
        /// </summary>
        /// <returns>List of contest ids</returns>
        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        List<int> GetContestList();

        /// <summary>
        /// Get contest by id
        /// </summary>
        /// <param name="id">contest id</param>
        /// <returns>contest information</returns>
        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        Contest GetContest(int id);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        [FaultContract(typeof(NotFoundError))]
        void DeleteContest(int id);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        [FaultContract(typeof(NotFoundError))]
        [FaultContract(typeof(ValidationError))]
        void UpdateContest(int id, string title, string description, DateTime? startTime, DateTime? restTime, DateTime? hackStartTime, DateTime? endTime, ContestType? type, bool? printingEnabled);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        [FaultContract(typeof(ValidationError))]
        int CreateContest(string title, string description, DateTime startTime, DateTime? restTime, DateTime? hackStartTime, DateTime endTime, ContestType type, bool printingEnabled);

        /// <summary>
        /// Get problem ids of the contest
        /// </summary>
        /// <param name="contestID">id of the contest</param>
        /// <returns>ids of all problems in the contest</returns>
        [OperationContract]
        [FaultContract(typeof(NotFoundError))]
        [FaultContract(typeof(AccessDeniedError))]
        List<int> GetProblemList(int contestID);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        [FaultContract(typeof(NotFoundError))]
        ProblemStatistics GetProblemStatistics(int id);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        [FaultContract(typeof(NotFoundError))]
        int CreateProblem(int contestID, string title, string content, int score, int timeLimit, long memoryLimit,
            string std, string spj, string validator, ProgrammingLanguage? stdLanguage, ProgrammingLanguage? spjLanguage, ProgrammingLanguage? validatorLanguage, IEnumerable<ProgrammingLanguage> forbiddenLanguages);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        [FaultContract(typeof(NotFoundError))]
        void UpdateProblem(int id, string title, string content, int? score, int? timeLimit, long? memoryLimit,
            string std, string spj, string validator, ProgrammingLanguage? stdLanguage, ProgrammingLanguage? spjLanguage, ProgrammingLanguage? validatorLanguage, IEnumerable<ProgrammingLanguage> forbiddenLanguages);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        [FaultContract(typeof(NotFoundError))]
        void LockProblem(int id);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        [FaultContract(typeof(NotFoundError))]
        Entity.ProblemGeneral GetProblemTitle(int id);

        /// <summary>
        /// Get problem by id
        /// </summary>
        /// <param name="id">problem id</param>
        /// <returns>problem information</returns>
        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        Problem GetProblem(int id);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        [FaultContract(typeof(NotFoundError))]
        void DeleteProblem(int id);

        /// <summary>
        /// Submit a solution.
        /// </summary>
        /// <param name="problemID">id of the solved problem</param>
        /// <param name="code">source code</param>
        /// <param name="language">language of the code</param>
        /// <returns>id of the new record</returns>
        [OperationContract]
        [FaultContract(typeof(NotFoundError))]
        [FaultContract(typeof(AccessDeniedError))]
        int Submit(int problemID, string code, ProgrammingLanguage language);

        /// <summary>
        /// List all records of a contest
        /// </summary>
        /// <param name="contestID">id of the contest</param>
        /// <returns>all record ids of the contest</returns>
        [OperationContract]
        [FaultContract(typeof(NotFoundError))]
        [FaultContract(typeof(AccessDeniedError))]
        List<int> GetRecordList(int contestID);

        [OperationContract]
        [FaultContract(typeof(NotFoundError))]
        [FaultContract(typeof(AccessDeniedError))]
        void Rejudge(int recordID);

        /// <summary>
        /// Get record by id
        /// </summary>
        /// <param name="id">record id</param>
        /// <returns>record information</returns>
        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        Record GetRecord(int id);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        [FaultContract(typeof(NotFoundError))]
        List<int> GetQuestionList(int contestID);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        Question GetQuestion(int id);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        [FaultContract(typeof(NotFoundError))]
        void UpdateQuestion(int id, string description, string answer, QuestionStatus? status);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        [FaultContract(typeof(NotFoundError))]
        int AskQuestion(int contestID, string description);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        [FaultContract(typeof(NotFoundError))]
        int HackRecord(int recordID, string dataOrDatamaker, ProgrammingLanguage? datamakerLanguage);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        [FaultContract(typeof(NotFoundError))]
        List<int> GetHackList(int contestID);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        Hack GetHack(int id);

        /// <summary>
        /// List all users
        /// </summary>
        /// <returns>ids of users</returns>
        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        List<int> GetUserList();

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>user information</returns>
        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        User GetUser(int id);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        void CreateUser(string name, string nickname, string password, UserRole role);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        [FaultContract(typeof(NotFoundError))]
        void UpdateUser(int id, string name, string nickname, string password, UserRole? role);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        [FaultContract(typeof(NotFoundError))]
        void DeleteUser(int id);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        List<int> GetOnlineList();

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        [FaultContract(typeof(NotFoundError))]
        void Kick(int userID);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        [FaultContract(typeof(NotFoundError))]
        List<int> GetTestCaseList(int problemID);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        TestCase GetTestCase(int id);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        [FaultContract(typeof(NotFoundError))]
        void DeleteTestCase(int id);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        [FaultContract(typeof(NotFoundError))]
        void UpdateTestCase(int id, byte[] input, byte[] output, TestCaseType? type);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        [FaultContract(typeof(NotFoundError))]
        int CreateTestCase(int problemID, byte[] input, byte[] output, TestCaseType type);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        [FaultContract(typeof(NotFoundError))]
        int RequestPrinting(int contestID, string content, int copies);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        PrintRequest GetPrintRequest(int id);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        [FaultContract(typeof(NotFoundError))]
        List<int> GetPrintRequestList(int contestID);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        [FaultContract(typeof(NotFoundError))]
        void UpdatePrintRequest(int id, string content, int? copies, PrintRequestStatus? status);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        [FaultContract(typeof(NotFoundError))]
        void DeletePrintRequest(int id);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        string GetConfig(string key);

        [OperationContract]
        [FaultContract(typeof(AccessDeniedError))]
        void SetConfig(string key, string value);

        [OperationContract]
        string GetCircular();

        [OperationContract]
        [FaultContract(typeof(NotFoundError))]
        List<StandingItem> GetStandings(int contest_id);

        [OperationContract]
        [FaultContract(typeof(NotFoundError))]
        int GetProblemRelatedContest(int id);
    }
}
