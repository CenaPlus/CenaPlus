using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Security.Cryptography;
using CenaPlus.Network;
using CenaPlus.Entity;
using CenaPlus.Server.Dal;
using System.Data.Objects;

namespace CenaPlus.Server.Bll
{
    [ServiceBehavior(UseSynchronizationContext = false)]
    public class LocalCenaServer : ICenaPlusServer
    {
        public User CurrentUser { get; set; }

        private void CheckRole(DB db, UserRole leastRole)
        {
            if (CurrentUser == null)
                throw new AccessDeniedException("Not authenticated");

            // Fake system user need not refreshing
            if (CurrentUser.Role != UserRole.System)
            {
                db.Users.Attach(CurrentUser);
                db.Entry(CurrentUser).Reload();
            }

            if (CurrentUser.Role < leastRole)
                throw new AccessDeniedException("Do not have required role.");
        }

        public string GetVersion()
        {
            var fileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            return fileVersion.FileMajorPart + "." + fileVersion.FileMinorPart;
        }

        public bool Authenticate(string userName, string password)
        {
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
                if (CurrentUser.Role != UserRole.Manager)
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
                    throw new AccessDeniedException();

                return (from p in db.Problems
                        where p.ContestID == contestID
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
                    throw new AccessDeniedException();

                return new Problem
                {
                    ID = problem.ID,
                    Content = problem.Content,
                    ContestID = problem.ContestID,
                    ContestTitle = problem.Contest.Title,
                    Title = problem.Title
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
                    throw new NotFoundException();

                if (CurrentUser.Role == UserRole.Competitor && !CurrentUser.AssignedContestIDs.Contains(problem.ContestID))
                    throw new AccessDeniedException();

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
                    throw new AccessDeniedException();

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
                    throw new NotFoundException();

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
    }
}
