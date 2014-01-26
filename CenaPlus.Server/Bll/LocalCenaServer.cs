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
    }
}
