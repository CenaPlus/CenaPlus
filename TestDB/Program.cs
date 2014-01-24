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
             * */
            Problem p = db.Problems.Where(x=>x.ID==1).Single();
            Console.WriteLine(p.Contest);
           
        }
    }
}
