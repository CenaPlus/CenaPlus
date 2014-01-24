using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDB
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new DB();
            Console.WriteLine(db.Configuration.LazyLoadingEnabled);
            /*
            Contest contest = new Contest
            {
                Type=ContestType.OI
            };
            db.Contests.Add(contest);
            db.Problems.Add(new Problem
            {
                Title="Titlt",
                Content="asdfasdfasdf",
                Contest = contest
            });
            db.SaveChanges();
             */
            var tmp = db.Set<Problem>().Find(1);
            Problem p = db.Problems.Where(x=>x.ID==1).Single();
            //db.Entry(p).Reference(x => x.Contest).Load();
            Console.WriteLine(p.Contest);
           
        }
    }
}
