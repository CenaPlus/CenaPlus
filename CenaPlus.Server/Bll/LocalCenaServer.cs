using CenaPlus.Entity;
using CenaPlus.Network;
using CenaPlus.Server.Dal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace CenaPlus.Server.Bll
{
    [ServiceBehavior(UseSynchronizationContext = false)]
    public class LocalCenaServer : ICenaPlusServer
    {
        public User CurrentUser { get; set; }
        public InstanceContext Context { get; set; }
        public ICenaPlusServerCallback Callback { get; set; }

        #region Events
        public static event Action<int> NewRecord;
        public static event Action<int> NewHack;
        public static event Action<int> NewContest;
        public static event Action<int> ContestModified;
        public static event Action<int> ContestDeleted;
        public static event Action<int> RecordRejudged;
        public static event Action<int> UserLoggedIn;
        public static event Action<int> UserLoggedOut;
        #endregion

        public LocalCenaServer()
        {
            if (OperationContext.Current != null)
            {
                Context = OperationContext.Current.InstanceContext;
                Callback = OperationContext.Current.GetCallbackChannel<ICenaPlusServerCallback>();
                Context.Closed += InstanceContext_Closed;
            }
        }

        void InstanceContext_Closed(object sender, EventArgs e)
        {
            if (CurrentUser != null)
            {
                lock (App.Clients)
                {
                    App.Clients.Remove(CurrentUser.ID);
                }

                if (UserLoggedOut != null)
                    System.Threading.Tasks.Task.Factory.StartNew(() => UserLoggedOut(CurrentUser.ID));
            }
        }

        private void CheckRole(DB db, UserRole leastRole)
        {
            if (CurrentUser == null)
                throw new FaultException<AccessDeniedError>(new AccessDeniedError(), "Not authenticated");

            // Fake system user need not refreshing
            if (CurrentUser.Role != UserRole.System)
            {
                db.Users.Attach(CurrentUser);
                db.Entry(CurrentUser).Reload();
            }

            if (CurrentUser.Role < leastRole)
                throw new FaultException<AccessDeniedError>(new AccessDeniedError(), "Do not have the required role.");
        }

        #region Misc
        public string GetVersion()
        {
            var version = typeof(ICenaPlusServer).Assembly.GetName().Version;
            return version.Major + "." + version.Minor;
        }

        public string GetCircular()
        {
            using (DB db = new DB())
            {
                var config = db.Configs.Find(ConfigKey.Circular);
                if (config == null) return ConfigKey.DefaultCircular;
                return config.Value;
            }
        }

        public bool Authenticate(string userName, string password)
        {
            if (CurrentUser != null)
                return false;

            byte[] pwdHash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(password));
            using (DB db = new DB())
            {
                var user = (from u in db.Users
                            where u.Name == userName && u.Password == pwdHash
                            select u).SingleOrDefault();

                if (user == null)
                {
                    return false;
                }

                lock (App.Clients)
                {
                    if (App.Clients.ContainsKey(user.ID))
                        throw new FaultException<AlreadyLoggedInError>(new AlreadyLoggedInError());
                    App.Clients.Add(user.ID, this);
                }

                CurrentUser = user;

                if (UserLoggedIn != null)
                    System.Threading.Tasks.Task.Factory.StartNew(() => UserLoggedIn(user.ID));
                return true;
            }
        }

        public User GetProfile()
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                return new User
                {
                    ID = CurrentUser.ID,
                    Name = CurrentUser.Name,
                    NickName = CurrentUser.NickName,
                    Role = CurrentUser.Role
                };
            }
        }

        public List<int> GetOnlineList()
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Manager);
            }

            lock (App.Clients)
            {
                return (from c in App.Clients
                        where c.Value.CurrentUser != null && c.Value.CurrentUser.Role != UserRole.System
                        select c.Value.CurrentUser.ID).ToList();
            }
        }

        public void Kick(int userID)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Manager);
            }

            LocalCenaServer server;
            lock (App.Clients)
            {
                if (!App.Clients.ContainsKey(userID))
                    throw new FaultException<NotFoundError>(new NotFoundError { ID = userID, Type = "OnlineUser" });

                server = App.Clients[userID];
            }

            server.Callback.Bye();
            server.Context.Abort();
        }
        #endregion
        #region Contest
        public void DeleteContest(int id)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Manager);

                Contest contest = db.Contests.Find(id);
                if (contest == null)
                    throw new FaultException<NotFoundError>(new NotFoundError { ID = id, Type = "Contest" });

                db.Contests.Remove(contest);
                db.SaveChanges();
                if (ContestDeleted != null)
                    System.Threading.Tasks.Task.Factory.StartNew(() => ContestDeleted(id));
            }
        }

        public int CreateContest(string title, string description, DateTime startTime, DateTime? restTime, DateTime? hackStartTime, DateTime endTime, ContestType type, bool printingEnabled)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Manager);

                if (startTime > endTime)
                    throw new FaultException<ValidationError>(new ValidationError());
                if (restTime != null && (restTime < startTime || restTime > endTime))
                    throw new FaultException<ValidationError>(new ValidationError());
                if (hackStartTime != null && (hackStartTime < startTime || hackStartTime > endTime))
                    throw new FaultException<ValidationError>(new ValidationError());
                if (restTime != null && hackStartTime != null && restTime > hackStartTime)
                    throw new FaultException<ValidationError>(new ValidationError());

                Contest contest = new Contest
                {
                    Description = description,
                    EndTime = endTime,
                    PrintingEnabled = printingEnabled,
                    StartTime = startTime,
                    RestTime = restTime,
                    HackStartTime = hackStartTime,
                    Title = title,
                    Type = type
                };

                db.Contests.Add(contest);
                db.SaveChanges();
                if (NewContest != null)
                    System.Threading.Tasks.Task.Factory.StartNew(() => NewContest(contest.ID));
                return contest.ID;
            }
        }
        public void UpdateContest(int id, string title, string description, DateTime? startTime, DateTime? restTime, DateTime? hackStartTime, DateTime? endTime, ContestType? type, bool? printingEnabled)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Manager);

                Contest contest = db.Contests.Find(id);
                if (contest == null)
                    throw new FaultException<NotFoundError>(new NotFoundError { ID = id, Type = "Contest" });

                if (title != null)
                    contest.Title = title;
                if (startTime != null)
                    contest.StartTime = startTime.Value;
                if (endTime != null)
                    contest.EndTime = endTime.Value;
                if (restTime != null)
                    contest.RestTime = restTime.Value;
                if (hackStartTime != null)
                    contest.HackStartTime = hackStartTime.Value;

                if (contest.StartTime > contest.EndTime)
                    throw new FaultException<ValidationError>(new ValidationError());
                if (contest.RestTime != null && (contest.RestTime < contest.StartTime || contest.RestTime > contest.EndTime))
                    throw new FaultException<ValidationError>(new ValidationError());
                if (contest.HackStartTime != null && (contest.HackStartTime < contest.StartTime || contest.HackStartTime > contest.EndTime))
                    throw new FaultException<ValidationError>(new ValidationError());
                if (contest.RestTime != null && contest.HackStartTime != null && contest.RestTime > contest.HackStartTime)
                    throw new FaultException<ValidationError>(new ValidationError());

                if (description != null)
                    contest.Description = description;
                if (type != null)
                    contest.Type = type.Value;
                if (printingEnabled != null)
                    contest.PrintingEnabled = printingEnabled.Value;

                db.SaveChanges();
                if (ContestModified != null)
                    System.Threading.Tasks.Task.Factory.StartNew(() => ContestModified(contest.ID));
            }
        }
        public List<int> GetContestList()
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                IQueryable<Contest> contests = db.Contests;
                var ids = contests.Select(c => c.ID);
                return ids.ToList();
            }
        }

        public Contest GetContest(int id)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                var contest = db.Contests.Find(id);
                if (contest == null) return null;
                return new Contest
                {
                    ID = contest.ID,
                    Description = contest.Description,
                    EndTime = contest.EndTime,
                    StartTime = contest.StartTime,
                    RestTime = contest.RestTime,
                    HackStartTime = contest.HackStartTime,
                    Title = contest.Title,
                    Type = contest.Type,
                    PrintingEnabled = contest.PrintingEnabled
                };
            }
        }

        #endregion
        #region Problem
        public ProblemStatistics GetProblemStatistics(int id)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                Problem problem = db.Problems.Find(id);
                if (problem == null) throw new FaultException<NotFoundError>(new NotFoundError { ID = id, Type = "Problem" });

                if (problem.Contest.Type == ContestType.OI && CurrentUser.Role == UserRole.Competitor)
                    throw new FaultException<AccessDeniedError>(new AccessDeniedError());

                var records = from r in db.Records
                              where r.ProblemID == problem.ID
                              select r;
                return new ProblemStatistics
                {
                    ProblemTitle = problem.Title,
                    AC = records.Where(r => r.StatusAsInt == (int)RecordStatus.Accepted).Count(),
                    CE = records.Where(r => r.StatusAsInt == (int)RecordStatus.CompileError).Count(),
                    MLE = records.Where(r => r.StatusAsInt == (int)RecordStatus.MemoryLimitExceeded).Count(),
                    RE = records.Where(r => r.StatusAsInt == (int)RecordStatus.RuntimeError).Count(),
                    SE = records.Where(r => r.StatusAsInt == (int)RecordStatus.SystemError).Count(),
                    TLE = records.Where(r => r.StatusAsInt == (int)RecordStatus.TimeLimitExceeded).Count(),
                    VE = records.Where(r => r.StatusAsInt == (int)RecordStatus.ValidatorError).Count(),
                    WA = records.Where(r => r.StatusAsInt == (int)RecordStatus.WrongAnswer).Count(),
                };
            }
        }
        public int CreateProblem(int contestID, string title, string content, int score, int timeLimit, long memoryLimit,
            string std, string spj, string validator, ProgrammingLanguage? stdLanguage, ProgrammingLanguage? spjLanguage, ProgrammingLanguage? validatorLanguage, IEnumerable<ProgrammingLanguage> forbiddenLanguages)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Manager);

                if (db.Contests.Find(contestID) == null)
                    throw new FaultException<NotFoundError>(new NotFoundError { ID = contestID, Type = "Contest" });

                Problem problem = new Problem
                {
                    Title = title,
                    Content = content,
                    Score = score,
                    ContestID = contestID,
                    TimeLimit = timeLimit,
                    MemoryLimit = memoryLimit,
                    Std = std,
                    Spj = spj,
                    Validator = validator,
                    StdLanguage = stdLanguage,
                    SpjLanguage = spjLanguage,
                    ValidatorLanguage = validatorLanguage,
                    ForbiddenLanguages = forbiddenLanguages
                };

                db.Problems.Add(problem);
                db.SaveChanges();

                return problem.ID;
            }
        }

        public void UpdateProblem(int id, string title, string content, int? score, int? timeLimit, long? memoryLimit,
            string std, string spj, string validator, ProgrammingLanguage? stdLanguage, ProgrammingLanguage? spjLanguage, ProgrammingLanguage? validatorLanguage, IEnumerable<ProgrammingLanguage> forbiddenLanguages)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Manager);

                Problem problem = db.Problems.Find(id);
                if (problem == null) throw new FaultException<NotFoundError>(new NotFoundError { ID = id, Type = "Problem" });

                if (title != null)
                    problem.Title = title;
                if (content != null)
                    problem.Content = content;
                if (score != null)
                    problem.Score = score.Value;
                if (memoryLimit != null)
                    problem.MemoryLimit = memoryLimit.Value;
                if (std != null)
                    problem.Std = std;
                if (spj != null)
                    problem.Spj = spj;
                if (validator != null)
                    problem.Validator = validator;
                if (stdLanguage != null)
                    problem.StdLanguage = stdLanguage;
                if (spjLanguage != null)
                    problem.SpjLanguage = spjLanguage;
                if (validatorLanguage != null)
                    problem.ValidatorLanguage = validatorLanguage;
                if (forbiddenLanguages != null)
                    problem.ForbiddenLanguages = forbiddenLanguages;

                db.SaveChanges();
            }
        }

        public void LockProblem(int id)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                Problem problem = db.Problems.Find(id);
                if (problem == null) throw new FaultException<NotFoundError>(new NotFoundError { ID = id, Type = "Problem" });

                Contest contest = problem.Contest;
                if (contest.Type != ContestType.Codeforces)
                    throw new FaultException<InvalidOperationError>(new InvalidOperationError(), "Not codeforces");

                bool accepted = (from r in db.Records
                                 where r.ProblemID == id && r.UserID == CurrentUser.ID
                                 where r.StatusAsInt == (int)RecordStatus.Accepted
                                 select r).Any();
                if (!accepted)
                    throw new FaultException<InvalidOperationError>(new InvalidOperationError(), "Not accepted");

                problem.LockedUsers.Add(CurrentUser);
                db.SaveChanges();
            }
        }

        public void DeleteProblem(int id)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Manager);

                Problem problem = db.Problems.Find(id);
                if (problem == null)
                    throw new FaultException<NotFoundError>(new NotFoundError { ID = id, Type = "Problem" });

                db.Problems.Remove(problem);
                db.SaveChanges();
            }
        }
        public List<int> GetProblemList(int contestID)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                var contest = db.Contests.Find(contestID);
                if (contest == null)
                    throw new FaultException<NotFoundError>(new NotFoundError { ID = contestID, Type = "Contest" });

                if (CurrentUser.Role == UserRole.Competitor && contest.StartTime > DateTime.Now)
                    return new List<int>();

                return (from p in db.Problems
                        where p.ContestID == contestID
                        orderby p.Score ascending
                        select p.ID).ToList();
            }
        }

        public Entity.ProblemGeneral GetProblemTitle(int id)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                var problem = db.Problems.Find(id);
                if (problem == null)
                    throw new FaultException<NotFoundError>(new NotFoundError { ID = id, Type = "Problem" });

                if (CurrentUser.Role == UserRole.Competitor && problem.Contest.StartTime > DateTime.Now)
                    throw new FaultException<AccessDeniedError>(new AccessDeniedError());

                Entity.ProblemGeneral problem_general = new ProblemGeneral();
                problem_general.Title = problem.Title;
                problem_general.TimeLimit = problem.TimeLimit;
                problem_general.MemoryLimit = Convert.ToInt32(problem.MemoryLimit / 1024 / 1024);
                problem_general.ProblemID = problem.ID;
                problem_general.SpecialJudge = problem.Spj == null ? false : true;

                var contest = db.Contests.Find(problem.ContestID);
                var last_record = RecordHelper.GetLastRecord(CurrentUser.ID, problem.ID);
                switch (contest.Type)
                {
                    case ContestType.OI:
                        {
                            if (last_record != null) 
                            {
                                problem_general.Status = ProblemGeneralStatus.Submitted;
                                problem_general.Time = last_record.SubmissionTime;
                            }
                            break;
                        }
                    case ContestType.ACM:
                        {
                            if (last_record != null)
                            {
                                problem_general.Status = ProblemGeneralStatus.Pending;
                                problem_general.Time = last_record.SubmissionTime;
                            }
                            else
                                problem_general.Status = null;
                            if (RecordHelper.GetFirstAcceptedRecord(CurrentUser.ID, problem.ID)!=null)
                                problem_general.Status = ProblemGeneralStatus.Accepted;
                            break;
                        }
                    case ContestType.Codeforces:
                        {
                            if (last_record == null)
                                problem_general.Status = null;
                            else
                            {
                                problem_general.Time = last_record.SubmissionTime;
                                if (last_record.Status == RecordStatus.Hacked)
                                    problem_general.Status = ProblemGeneralStatus.Hacked;
                                else if (last_record.Status == RecordStatus.Accepted)
                                    problem_general.Status = ProblemGeneralStatus.Accepted;
                                else
                                    problem_general.Status = ProblemGeneralStatus.Pending;
                            }
                            break;
                        }
                }
                return problem_general;
            }
        }
        
        public Problem GetProblem(int id)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                var problem = db.Problems.Find(id);
                if (problem == null) return null;

                if (CurrentUser.Role == UserRole.Competitor && problem.Contest.StartTime > DateTime.Now)
                    throw new FaultException<AccessDeniedError>(new AccessDeniedError());

                if (problem.Contest.Type == ContestType.TopCoder)
                {
                    var view = db.ProblemViews.Find(id, CurrentUser.ID);
                    if (view == null)
                    {
                        view = new ProblemView
                        {
                            ProblemID = id,
                            UserID = CurrentUser.ID,
                            Time = DateTime.Now
                        };
                        db.ProblemViews.Add(view);
                        db.SaveChanges();
                    }
                }

                return new Problem
                {
                    ID = problem.ID,
                    Content = problem.Content,
                    ContestID = problem.ContestID,
                    Score = problem.Score,
                    ContestTitle = problem.Contest.Title,
                    Title = problem.Title,
                    TestCasesCount = problem.TestCases.Count,
                    MemoryLimit = problem.MemoryLimit,
                    TimeLimit = problem.TimeLimit,
                    Spj = CurrentUser.Role >= UserRole.Manager ? problem.Spj : null,
                    Std = CurrentUser.Role >= UserRole.Manager ? problem.Std : null,
                    Validator = CurrentUser.Role >= UserRole.Manager ? problem.Validator : null,
                    SpjLanguage = CurrentUser.Role >= UserRole.Manager ? problem.SpjLanguage : null,
                    StdLanguage = CurrentUser.Role >= UserRole.Manager ? problem.StdLanguage : null,
                    ValidatorLanguage = CurrentUser.Role >= UserRole.Manager ? problem.ValidatorLanguage : null,
                    ForbiddenLanguages = problem.ForbiddenLanguages
                };
            }
        }

        #endregion
        #region Record
        public void Rejudge(int recordID)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Manager);

                Record record = db.Records.Find(recordID);
                if (record == null) throw new FaultException<NotFoundError>(new NotFoundError { ID = recordID, Type = "Record" });

                record.Status = RecordStatus.Pending;
                record.TimeUsage = null;
                record.MemoryUsage = null;
                record.Detail = null;
                db.SaveChanges();

                if (RecordRejudged != null)
                    System.Threading.Tasks.Task.Factory.StartNew(() => RecordRejudged(recordID));
            }
        }
        public int Submit(int problemID, string code, ProgrammingLanguage language)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                var problem = db.Problems.Find(problemID);
                if (problem == null)
                    throw new FaultException<NotFoundError>(new NotFoundError { ID = problemID, Type = "Problem" });

                // Out of contest time
                if (CurrentUser.Role == UserRole.Competitor && (problem.Contest.StartTime > DateTime.Now || problem.Contest.EndTime < DateTime.Now))
                    throw new FaultException<AccessDeniedError>(new AccessDeniedError());

                // After TC rest time
                if (problem.Contest.Type == ContestType.TopCoder && CurrentUser.Role == UserRole.Competitor && problem.Contest.RestTime < DateTime.Now)
                    throw new FaultException<AccessDeniedError>(new AccessDeniedError(), "After tc rest time");

                // CF Locked
                if (problem.Contest.Type == ContestType.Codeforces && CurrentUser.Role == UserRole.Competitor && problem.LockedUsers.Contains(CurrentUser))
                    throw new FaultException<AccessDeniedError>(new AccessDeniedError(), "problem locked");

                var record = new Record
                {
                    Code = code,
                    Language = language,
                    ProblemID = problemID,
                    Status = RecordStatus.Pending,
                    SubmissionTime = DateTime.Now,
                    UserID = CurrentUser.ID,
                    Score = 0
                };

                db.Records.Add(record);
                db.SaveChanges();
                if (NewRecord != null)
                    System.Threading.Tasks.Task.Factory.StartNew(() => NewRecord(record.ID));
                return record.ID;
            }
        }

        public List<int> GetRecordList(int contestID)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                var contest = db.Contests.Find(contestID);
                if (contest == null)
                    throw new FaultException<NotFoundError>(new NotFoundError { ID = contestID, Type = "Contest" });

                var problemIDs = (from p in db.Problems
                                  where p.ContestID == contestID
                                  select p.ID).ToList();
                var recordIDs = from r in db.Records
                                where problemIDs.Contains(r.ProblemID)
                                where CurrentUser.Role >= UserRole.Manager || r.UserID == CurrentUser.ID
                                orderby r.ID descending
                                select r.ID;
                return recordIDs.ToList();
            }
        }

        public Record GetRecord(int id)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                var record = db.Records.Find(id);
                if (record == null) return null;

                var contest = record.Problem.Contest;
                var problem = record.Problem;

                // Managers can view all the details
                // All records should be published when the contest ends.
                if (CurrentUser.Role >= UserRole.Manager || contest.EndTime < DateTime.Now)
                    return new Record
                    {
                        ID = record.ID,
                        Code = record.Code,
                        Detail = record.Detail,
                        Language = record.Language,
                        MemoryUsage = record.MemoryUsage,
                        ProblemID = record.ProblemID,
                        ProblemTitle = record.Problem.Title,
                        Status = record.Status,
                        SubmissionTime = record.SubmissionTime,
                        TimeUsage = record.TimeUsage,
                        UserID = record.UserID,
                        UserNickName = record.User.NickName
                    };

                // If not started, competitors cannot view any records
                if (contest.StartTime > DateTime.Now)
                    return null;

                // until now, we've got sure that the user is a competitor
                // and the contest is in progress.
                switch (contest.Type)
                {
                    case ContestType.OI:
                        if (CurrentUser.ID == record.UserID)
                            return new Record
                            {
                                ID = record.ID,
                                Code = record.Code,
                                Detail = record.Status == RecordStatus.CompileError ? record.Detail : null,
                                Language = record.Language,
                                ProblemID = record.ProblemID,
                                ProblemTitle = record.Problem.Title,
                                Status = record.Status == RecordStatus.CompileError ? RecordStatus.CompileError : RecordStatus.Unknown,
                                SubmissionTime = record.SubmissionTime,
                                UserID = record.UserID,
                                UserNickName = record.User.NickName
                            };
                        else
                            return new Record
                            {
                                ID = record.ID,
                                ProblemID = record.ProblemID,
                                ProblemTitle = record.Problem.Title,
                                Status = RecordStatus.Unknown,
                                SubmissionTime = record.SubmissionTime,
                                UserID = record.UserID,
                                UserNickName = record.User.NickName
                            };
                    case ContestType.ACM:
                        if (CurrentUser.ID == record.UserID)
                            return new Record
                            {
                                ID = record.ID,
                                Code = record.Code,
                                Detail = record.Status == RecordStatus.CompileError ? record.Detail : null,
                                Language = record.Language,
                                MemoryUsage = record.MemoryUsage,
                                ProblemID = record.ProblemID,
                                ProblemTitle = record.Problem.Title,
                                Status = record.Status,
                                SubmissionTime = record.SubmissionTime,
                                TimeUsage = record.TimeUsage,
                                UserID = record.UserID,
                                UserNickName = record.User.NickName
                            };
                        else
                            return new Record
                            {
                                ID = record.ID,
                                Language = record.Language,
                                MemoryUsage = record.MemoryUsage,
                                ProblemID = record.ProblemID,
                                ProblemTitle = record.Problem.Title,
                                Status = record.Status,
                                SubmissionTime = record.SubmissionTime,
                                TimeUsage = record.TimeUsage,
                                UserID = record.UserID,
                                UserNickName = record.User.NickName
                            };
                    case ContestType.Codeforces:
                        if (CurrentUser.ID == record.ID || problem.LockedUsers.Contains(CurrentUser))
                            return new Record
                            {
                                ID = record.ID,
                                Code = record.Code,
                                Detail = record.Detail,
                                Language = record.Language,
                                MemoryUsage = record.MemoryUsage,
                                ProblemID = record.ProblemID,
                                ProblemTitle = record.Problem.Title,
                                Status = record.Status,
                                SubmissionTime = record.SubmissionTime,
                                TimeUsage = record.TimeUsage,
                                UserID = record.UserID,
                                UserNickName = record.User.NickName
                            };
                        else
                            return new Record
                            {
                                ID = record.ID,
                                Language = record.Language,
                                MemoryUsage = record.MemoryUsage,
                                ProblemID = record.ProblemID,
                                ProblemTitle = record.Problem.Title,
                                Status = record.Status,
                                SubmissionTime = record.SubmissionTime,
                                TimeUsage = record.TimeUsage,
                                UserID = record.UserID,
                                UserNickName = record.User.NickName
                            };
                    case ContestType.TopCoder:
                        if (CurrentUser.ID == record.ID || contest.HackStartTime < DateTime.Now)
                            return new Record
                            {
                                ID = record.ID,
                                Code = record.Code,
                                Detail = record.Detail,
                                Language = record.Language,
                                MemoryUsage = record.MemoryUsage,
                                ProblemID = record.ProblemID,
                                ProblemTitle = record.Problem.Title,
                                Status = record.Status,
                                SubmissionTime = record.SubmissionTime,
                                TimeUsage = record.TimeUsage,
                                UserID = record.UserID,
                                UserNickName = record.User.NickName
                            };
                        else
                            return new Record
                            {
                                ID = record.ID,
                                Language = record.Language,
                                MemoryUsage = record.MemoryUsage,
                                ProblemID = record.ProblemID,
                                ProblemTitle = record.Problem.Title,
                                Status = record.Status,
                                SubmissionTime = record.SubmissionTime,
                                TimeUsage = record.TimeUsage,
                                UserID = record.UserID,
                                UserNickName = record.User.NickName
                            };
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        #endregion
        #region Hack
        public int HackRecord(int recordID, string dataOrDatamaker, ProgrammingLanguage? datamakerLanguage)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                Record record = db.Records.Find(recordID);
                if (record == null) throw new FaultException<NotFoundError>(new NotFoundError { ID = recordID, Type = "Record" });

                if (record.Status != RecordStatus.Accepted)
                    throw new FaultException<InvalidOperationError>(new InvalidOperationError(), "Only accepted records can be hacked");

                Contest contest = record.Problem.Contest;
                Problem problem = record.Problem;

                if (contest.StartTime > DateTime.Now)
                    throw new FaultException<InvalidOperationError>(new InvalidOperationError(), "Contest not started");
                if (contest.EndTime < DateTime.Now)
                    throw new FaultException<InvalidOperationError>(new InvalidOperationError(), "Contest has finished");

                if (contest.Type == ContestType.Codeforces)
                {
                    if (!problem.LockedUsers.Contains(CurrentUser))
                        throw new FaultException<InvalidOperationError>(new InvalidOperationError(), "Please lock the problem first");
                }
                else if (contest.Type == ContestType.TopCoder)
                {
                    if (contest.HackStartTime > DateTime.Now)
                        throw new FaultException<InvalidOperationError>(new InvalidOperationError(), "Hack before hackStartTime");
                }
                else
                    throw new FaultException<InvalidOperationError>(new InvalidOperationError(), "This contest does not support hacking.");

                Hack hack = new Hack
                {
                    DatamakerLanguage = datamakerLanguage,
                    DataOrDatamaker = dataOrDatamaker,
                    HackerID = CurrentUser.ID,
                    RecordID = record.ID,
                    Status = HackStatus.Pending
                };

                db.Hacks.Add(hack);
                db.SaveChanges();

                if (NewHack != null)
                    System.Threading.Tasks.Task.Factory.StartNew(() => NewHack(hack.ID));
                return hack.ID;
            }
        }

        public List<int> GetHackList(int contestID)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                return (from h in db.Hacks
                        where h.Record.Problem.ContestID == contestID
                        select h.ID).ToList();
            }
        }

        public Hack GetHack(int id)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                Hack hack = db.Hacks.Find(id);
                if (hack == null) return null;

                return new Hack
                {
                    DatamakerLanguage = hack.DatamakerLanguage,
                    DataOrDatamaker = hack.DataOrDatamaker,
                    Detail = hack.Detail,
                    HackerID = hack.HackerID,
                    HackerNickName = hack.Hacker.NickName,
                    HackeeNickName = hack.Record.User.NickName,
                    HackeeID = hack.Record.UserID,
                    ID = hack.ID,
                    RecordID = hack.RecordID,
                    Status = hack.Status
                };
            }
        }
        #endregion
        #region User

        public List<int> GetUserList()
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Manager);
                return db.Users.Select(u => u.ID).ToList();
            }
        }

        public User GetUser(int id)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Manager);

                var user = db.Users.Find(id);
                if (user == null) return null;

                return new User
                {
                    ID = user.ID,
                    Name = user.Name,
                    NickName = user.NickName,
                    Role = user.Role
                };
            }
        }


        public void UpdateUser(int id, string name, string nickname, string password, UserRole? role)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Manager);

                var user = db.Users.Find(id);
                if (user == null)
                    throw new FaultException<NotFoundError>(new NotFoundError { ID = id, Type = "User" });

                if (name != null)
                    user.Name = name;
                if (user.NickName != null)
                    user.NickName = nickname;
                if (password != null)
                    user.Password = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(password));
                if (role != null)
                    user.Role = role.Value;

                db.SaveChanges();
            }
        }

        public void DeleteUser(int id)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Manager);

                User user = db.Users.Find(id);
                if (user == null)
                    throw new FaultException<NotFoundError>(new NotFoundError { ID = id, Type = "User" });

                db.Users.Remove(user);
                db.SaveChanges();
            }
        }


        public void CreateUser(string name, string nickname, string password, UserRole role)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Manager);

                if (db.Users.Where(u => u.Name == name).Any())
                    throw new FaultException<ConflictError>(new ConflictError());

                db.Users.Add(new User
                {
                    Name = name,
                    NickName = nickname,
                    Password = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(password)),
                    Role = role
                });

                db.SaveChanges();
            }
        }

        #endregion
        #region Question
        public List<int> GetQuestionList(int contestID)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                return (from q in db.Questions
                        where q.ContestID == contestID
                        where CurrentUser.RoleAsInt >= (int)UserRole.Manager
                            || q.AskerID == CurrentUser.ID || q.StatusAsInt == (int)QuestionStatus.Public
                        select q.ID).ToList();
            }
        }

        public void UpdateQuestion(int id, string description, string answer, QuestionStatus? status)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                Question question = db.Questions.Find(id);
                if (question == null)
                    throw new FaultException<NotFoundError>(new NotFoundError { ID = id, Type = "Question" });

                if (CurrentUser.Role == UserRole.Competitor && question.AskerID != CurrentUser.ID)
                    throw new FaultException<AccessDeniedError>(new AccessDeniedError(), "The question is not owned by you!");

                if (description != null)
                    question.Description = description;
                if (CurrentUser.Role >= UserRole.Manager && answer != null)
                    question.Answer = answer;
                if (CurrentUser.Role >= UserRole.Manager && status != null)
                    question.Status = status.Value;

                db.SaveChanges();

                Question q =  new Question
                {
                    Answer = question.Answer,
                    AskerID = question.AskerID,
                    AskerNickName = question.Asker.NickName,
                    ContestID = question.ContestID,
                    ContestName = question.ContestName,
                    Description = question.Description,
                    ID = question.ID,
                    Status = question.Status,
                    Time = question.Time
                };
                if (question.Status == QuestionStatus.Public)
                {
                    foreach (var client in App.Clients.Values)
                    {
                        System.Threading.Tasks.Task.Factory.StartNew(() => client.Callback.QuestionUpdated(q));
                    }
                }
                else if (question.Status == QuestionStatus.Private)
                {
                    App.Clients[question.AskerID].Callback.QuestionUpdated(q);
                }
            }
        }

        public Question GetQuestion(int id)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                Question question = db.Questions.Find(id);
                if (question == null)
                    return null;

                if (CurrentUser.Role >= UserRole.Manager || question.Status == QuestionStatus.Public || question.AskerID == CurrentUser.ID)
                {
                    return new Question
                    {
                        Answer = question.Answer,
                        AskerID = question.AskerID,
                        AskerNickName = question.Asker.NickName,
                        ContestID = question.ContestID,
                        ContestName = question.ContestName,
                        Description = question.Description,
                        ID = question.ID,
                        Status = question.Status,
                        Time = question.Time
                    };
                }
                else return null;
            }
        }


        public int AskQuestion(int contestID, string description)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                Question question = new Question
                {
                    AskerID = CurrentUser.ID,
                    ContestID = contestID,
                    Description = description,
                    Status = QuestionStatus.Pending,
                    Time = DateTime.Now
                };

                db.Questions.Add(question);
                db.SaveChanges();

                return question.ID;
            }
        }

        #endregion
        #region TestCase
        public List<int> GetTestCaseList(int problemID)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Manager);

                return db.TestCases.Where(t => t.ProblemID == problemID).Select(t => t.ID).ToList();
            }
        }
        public TestCase GetTestCase(int id)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Manager);

                TestCase testCase = db.TestCases.Find(id);
                if (testCase == null) return null;

                return new TestCase
                {
                    ID = testCase.ID,
                    InputHash = testCase.InputHash,
                    OutputHash = testCase.OutputHash,
                    InputPreview = Encoding.UTF8.GetString(testCase.Input.Take(100).ToArray()),
                    OutputPreview = Encoding.UTF8.GetString(testCase.Output.Take(100).ToArray()),
                    InputSize = testCase.Input.Length,
                    OutputSize = testCase.Output.Length,
                    ProblemID = testCase.ProblemID,
                    ProblemTitle = testCase.ProblemTitle,
                    Type = testCase.Type
                };
            }
        }
        public void DeleteTestCase(int id)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Manager);

                TestCase testCase = db.TestCases.Find(id);
                if (testCase == null) throw new FaultException<NotFoundError>(new NotFoundError { ID = id, Type = "TestCase" });

                db.TestCases.Remove(testCase);
                db.SaveChanges();
            }
        }
        public void UpdateTestCase(int id, byte[] input, byte[] output, TestCaseType? type)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Manager);

                TestCase testCase = db.TestCases.Find(id);
                if (testCase == null) throw new FaultException<NotFoundError>(new NotFoundError { ID = id, Type = "TestCase" });

                if (input != null)
                {
                    testCase.Input = input;
                    testCase.InputHash = MD5.Create().ComputeHash(input);
                }
                if (output != null)
                {
                    testCase.Output = output;
                    testCase.OutputHash = MD5.Create().ComputeHash(output);
                }
                if (type != null)
                    testCase.Type = type.Value;

                db.SaveChanges();
            }
        }
        public int CreateTestCase(int problemID, byte[] input, byte[] output, TestCaseType type)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Manager);

                TestCase testCase = new TestCase
                {
                    Input = input,
                    InputHash = MD5.Create().ComputeHash(input),
                    Output = output,
                    OutputHash = MD5.Create().ComputeHash(output),
                    ProblemID = problemID,
                    Type = type
                };
                db.TestCases.Add(testCase);
                db.SaveChanges();
                return testCase.ID;
            }
        }
        #endregion
        #region PrintRequest
        public int RequestPrinting(int contestID, string content, int copies)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                Contest contest = db.Contests.Find(contestID);
                if (contest == null) throw new FaultException<NotFoundError>(new NotFoundError { ID = contestID, Type = "Contest" });

                if (!contest.PrintingEnabled)
                    throw new FaultException<AccessDeniedError>(new AccessDeniedError(), "printing is not enabled");

                if (contest.StartTime > DateTime.Now || contest.EndTime < DateTime.Now)
                    throw new FaultException<AccessDeniedError>(new AccessDeniedError(), "Out of contest time");

                PrintRequest request = new PrintRequest
                {
                    Content = content,
                    ContestID = contestID,
                    Copies = copies,
                    Status = PrintRequestStatus.Pending,
                    Time = DateTime.Now,
                    UserID = CurrentUser.ID
                };
                db.PrintRequests.Add(request);
                db.SaveChanges();
                return request.ID;
            }
        }
        public PrintRequest GetPrintRequest(int id)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                PrintRequest request = db.PrintRequests.Find(id);
                if (request == null) return null;

                if (CurrentUser.Role >= UserRole.Manager || CurrentUser.ID == request.ID)
                {
                    return new PrintRequest
                    {
                        ID = request.ID,
                        Content = request.Content,
                        ContestID = request.ContestID,
                        ContestTitle = request.Contest.Title,
                        Copies = request.Copies,
                        Status = request.Status,
                        Time = request.Time,
                        UserID = request.UserID,
                        UserNickName = request.User.NickName
                    };
                }
                else
                {
                    return null;
                }
            }
        }
        public void UpdatePrintRequest(int id, string content, int? copies, PrintRequestStatus? status)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                PrintRequest request = db.PrintRequests.Find(id);
                if (request == null)
                    throw new FaultException<NotFoundError>(new NotFoundError { ID = id, Type = "PrintRequest" });
                if (CurrentUser.Role < UserRole.Manager && request.Status > PrintRequestStatus.Pending)
                    throw new FaultException<AccessDeniedError>(new AccessDeniedError());

                if (content != null)
                    request.Content = content;
                if (copies != null)
                    request.Copies = copies.Value;
                if (CurrentUser.Role >= UserRole.Manager && status != null)
                    request.Status = status.Value;
                db.SaveChanges();
            }
        }
        public List<int> GetPrintRequestList(int contestID)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                return (from r in db.PrintRequests
                        where r.ContestID == contestID
                            && (CurrentUser.Role >= UserRole.Manager ? true : r.UserID == CurrentUser.ID)
                        orderby r.StatusAsInt ascending
                        select r.ID).ToList();
            }
        }
        public void DeletePrintRequest(int id)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                PrintRequest request = db.PrintRequests.Find(id);
                if (request == null) throw new FaultException<NotFoundError>(new NotFoundError { ID = id, Type = "PrintRequest" });

                if (CurrentUser.Role >= UserRole.Manager || request.UserID == CurrentUser.ID)
                {
                    db.PrintRequests.Remove(request);
                    db.SaveChanges();
                }
                else
                {
                    throw new FaultException<AccessDeniedError>(new AccessDeniedError());
                }
            }
        }
        #endregion
        #region Config
        public string GetConfig(string key)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Manager);

                Config config = db.Configs.Find(key);
                if (config == null) return null;

                return config.Value;
            }
        }
        public void SetConfig(string key, string value)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Manager);

                Config config = db.Configs.Find(key);
                if (config == null)
                {
                    config = new Config { Key = key, Value = value };
                    db.Configs.Add(config);
                    db.SaveChanges();
                }
                else
                {
                    config.Value = value;
                    db.SaveChanges();
                }
            }
        }
        #endregion
        #region Standings
        public List<StandingItem> GetStandings(int contest_id)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                if (Bll.StandingsCache.Standings[contest_id] == null)
                {
                    Bll.StandingsCache.Rebuild(contest_id);
                }
                return (List<StandingItem>)Bll.StandingsCache.Standings[contest_id];
            }
        }
        #endregion
    }
}
