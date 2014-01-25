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
            var a = from p in db.Problems
                    select new
                    {
                        ContestType = p.Contest.Type,
                        ProblemTitle = p.Title
                    };
            Console.WriteLine(a.ToString());
           
        }
    }
}
