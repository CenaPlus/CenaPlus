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

namespace CenaPlus.Server.Bll
{
    [ServiceBehavior(UseSynchronizationContext = false, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class LocalCenaServer : ICenaPlusServer
    {
        public User CurrentUser { get; set; }

        private void CheckRole(UserRole leastRole)
        {
            if (CurrentUser == null)
                throw new AccessDeniedException("Not authenticated");
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
            CheckRole(UserRole.Competitor);

            using (DB db = new DB())
            {
                return db.Contests.Select(c => c.ID).ToList();
            }
        }

        public Contest GetContest(int id)
        {
            CheckRole(UserRole.Competitor);

            using (DB db = new DB())
            {
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
            CheckRole(UserRole.Competitor);

            using (DB db = new DB())
            {
                return (from p in db.Problems
                        where p.ContestID == contestID
                        select p.ID).ToList();
            }
        }

        public Problem GetProblem(int id)
        {
            CheckRole(UserRole.Competitor);

            using (DB db = new DB())
            {
                var problem = db.Problems.Find(id);
                if (problem == null) return null;
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
            CheckRole(UserRole.Competitor);

            using (DB db = new DB())
            {
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
            CheckRole(UserRole.Competitor);

            using (DB db = new DB())
            {
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
            CheckRole(UserRole.Competitor);

            using (DB db = new DB())
            {
                var record = db.Records.Find(id);
                if (record == null) return null;
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
                    UserName = record.User.Name
                };
            }
        }
    }
}
