using CenaPlus.Entity;
using CenaPlus.Network;
using CenaPlus.Server.Dal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public string GetVersion()
        {
            var fileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            return fileVersion.FileMajorPart + "." + fileVersion.FileMinorPart;
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
                return true;
            }
        }

        public List<int> GetContestList()
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                IQueryable<Contest> contests = db.Contests;
                if (CurrentUser.Role < UserRole.Manager)
                    contests = contests.Where(c => CurrentUser.AssignedContestIDs.Contains(c.ID));
                var ids = contests.Select(c => c.ID);
                return ids.ToList();
            }
        }

        public Contest GetContest(int id)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                if (CurrentUser.Role == UserRole.Competitor && !CurrentUser.AssignedContestIDs.Contains(id))
                    return null;

                var contest = db.Contests.Find(id);
                if (contest == null) return null;
                return new Contest
                {
                    ID = contest.ID,
                    Description = contest.Description,
                    EndTime = contest.EndTime,
                    StartTime = contest.StartTime,
                    Title = contest.Title,
                    Type = contest.Type
                };
            }
        }

        public List<int> GetProblemList(int contestID)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                if (CurrentUser.Role == UserRole.Competitor && !CurrentUser.AssignedContestIDs.Contains(contestID))
                    throw new FaultException<AccessDeniedError>(new AccessDeniedError());

                return (from p in db.Problems
                        where p.ContestID == contestID
                        orderby p.Score ascending
                        select p.ID).ToList();
            }
        }

        public Problem GetProblem(int id)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                var problem = db.Problems.Find(id);
                if (problem == null) return null;

                if (CurrentUser.Role == UserRole.Competitor && !CurrentUser.AssignedContestIDs.Contains(problem.ContestID))
                    throw new FaultException<AccessDeniedError>(new AccessDeniedError());

                return new Problem
                {
                    ID = problem.ID,
                    Content = problem.Content,
                    ContestID = problem.ContestID,
                    Score = problem.Score,
                    ContestTitle = problem.Contest.Title,
                    Title = problem.Title,
                    TestCasesCount = problem.TestCases.Count
                };
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

                if (CurrentUser.Role == UserRole.Competitor && !CurrentUser.AssignedContestIDs.Contains(problem.ContestID))
                    throw new FaultException<AccessDeniedError>(new AccessDeniedError());

                var record = new Record
                {
                    Code = code,
                    Language = language,
                    ProblemID = problemID,
                    Status = RecordStatus.Pending,
                    UserID = CurrentUser.ID
                };

                db.Records.Add(record);
                db.SaveChanges();
                return record.ID;
            }
        }


        public List<int> GetRecordList(int contestID)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                if (CurrentUser.Role == UserRole.Competitor && !CurrentUser.AssignedContestIDs.Contains(contestID))
                    throw new FaultException<AccessDeniedError>(new AccessDeniedError());

                var problemIDs = (from p in db.Problems
                                  where p.ContestID == contestID
                                  select p.ID).ToList();
                var recordIDs = from r in db.Records
                                where problemIDs.Contains(r.ProblemID)
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

                if (CurrentUser.Role == UserRole.Competitor && !CurrentUser.AssignedContestIDs.Contains(record.Problem.ContestID))
                    return null;

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
            }
        }


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


        public List<int> GetQuestionList(int contestID)
        {
            using (DB db = new DB())
            {
                CheckRole(db, UserRole.Competitor);

                if (CurrentUser.Role == UserRole.Competitor && !CurrentUser.AssignedContestIDs.Contains(contestID))
                    throw new FaultException<AccessDeniedError>(new AccessDeniedError());

                return (from q in db.Questions
                        where q.ContestID == contestID
                        where CurrentUser.RoleAsInt >= (int)UserRole.Manager
                            || q.AskerID == CurrentUser.ID || q.StatusAsInt == (int)QuestionStatus.Public
                        select q.ID).ToList();
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

                if (CurrentUser.Role == UserRole.Competitor && !CurrentUser.AssignedContestIDs.Contains(question.ContestID))
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

                if (CurrentUser.Role == UserRole.Competitor && !CurrentUser.AssignedContestIDs.Contains(contestID))
                    throw new FaultException<AccessDeniedError>(new AccessDeniedError());

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

        public void UpdateContest(int id, string title, string description, DateTime? startTime, DateTime? endTime, ContestType? type)
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
                if (contest.StartTime > contest.EndTime)
                    throw new FaultException<ValidationError>(new ValidationError());
                if (description != null)
                    contest.Description = description;
                if (type != null)
                    contest.Type = type.Value;

                db.SaveChanges();
            }
        }

        public int CreateProblem(int contestID, string title, string content, int score)
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
                    ContestID = contestID
                };

                db.Problems.Add(problem);
                db.SaveChanges();

                return problem.ID;
            }
        }
    }
}
