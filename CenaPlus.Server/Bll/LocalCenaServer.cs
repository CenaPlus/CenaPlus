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
                var user = (from u in db.Set<User>()
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
                return db.Set<Contest>().Select(c => c.ID).ToList();
            }
        }

        public Contest GetContest(int id)
        {
            CheckRole(UserRole.Competitor);

            return new Contest
            {
                ID = id,
                Title = "Foobar Contest",
                Description = "Haha"
            };
        }


    }
}
