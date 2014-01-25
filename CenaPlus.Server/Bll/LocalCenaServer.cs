using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using CenaPlus.Network;
using CenaPlus.Entity;
using CenaPlus.Server.Dal;

namespace CenaPlus.Server.Bll
{
    public class LocalCenaServer : ICenaPlusServer
    {
        private int counter;
        public string GetVersion()
        {
            Console.WriteLine("counter = " + counter++);
            var fileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            return fileVersion.FileMajorPart + "." + fileVersion.FileMinorPart;
        }

        public List<int> GetContestList()
        {
            var savedContext = OperationContext.Current;
            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.AutoReset = false;
            timer.Elapsed += (x, y) =>
            {
                savedContext.GetCallbackChannel<ICenaPlusServerCallback>().PopAd("Free iPhone5 ON SALE!");
            };
            timer.Start();
            return new List<int> { 1, 2, 3, 4 };

            // Should be implemented as below:
            // using (DB db = new DB("some connection string"))
            // {
            //    return db.Set<Contest>().Select(c => c.ID).ToList();
            // }
        }


        public Contest GetContest(int id)
        {
            return new Contest
            {
                ID = id,
                Title = "Foobar Contest",
                Description = "Haha"
            };
        }
    }
}
