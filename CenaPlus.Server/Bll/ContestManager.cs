using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using CenaPlus.Server.Dal;
using CenaPlus.Entity;
namespace CenaPlus.Server.Bll
{
    public class ContestManager
    {
        private Dictionary<int, List<Timer>> timers = new Dictionary<int, List<Timer>>();

        public void ScheduleAll()
        {
            using (DB db = new DB())
            {
                var ids = db.Contests.Where(c => c.EndTime > DateTime.Now).Select(c => c.ID);
                foreach (var id in ids)
                {
                    Reschedule(id);
                }
            }
        }

        public void Reschedule(int contestID)
        {
            using (DB db = new DB())
            {
                Contest contest = db.Contests.Find(contestID);

                lock (timers)
                {
                    List<Timer> previousTimers;
                    if (timers.TryGetValue(contestID, out previousTimers))
                    {
                        foreach (var timer in previousTimers) timer.Stop();
                        timers.Remove(contestID);
                    }

                    if (contest == null) return;
                    if (contest.EndTime <= DateTime.Now) return;

                    List<Timer> newTimers = new List<Timer>();
                    if (contest.StartTime > DateTime.Now)
                    {
                        var whenStart = new Timer((contest.StartTime - DateTime.Now).TotalMilliseconds);
                        whenStart.AutoReset = false;
                        whenStart.Elapsed += (o, e) => WhenContestStart(contestID);
                        whenStart.Start();
                        newTimers.Add(whenStart);
                    }

                    if (contest.EndTime > DateTime.Now)
                    {
                        var whenEnd = new Timer((contest.EndTime - DateTime.Now).TotalMilliseconds);
                        whenEnd.AutoReset = false;
                        whenEnd.Elapsed += (o, e) => WhenContestEnd(contestID);
                        whenEnd.Start();
                        newTimers.Add(whenEnd);
                    }

                    timers.Add(contestID, newTimers);
                }
            }
        }

        public void RemoveSchedule(int contestID)
        {
            lock (timers)
            {
                List<Timer> oldTimers;
                if (timers.TryGetValue(contestID, out oldTimers))
                {
                    foreach (var timer in oldTimers)
                    {
                        timer.Stop();
                    }
                }
                timers.Remove(contestID);
            }
        }

        private void WhenContestStart(int contestID)
        {
            //TODO: unimplemented
        }

        private void WhenContestEnd(int contestID)
        {
            using (DB db = new DB())
            {
                var contest = db.Contests.Find(contestID);

                switch (contest.Type)
                {
                    case ContestType.Codeforces:
                        //TODO: rejudge all records including system test.
                        break;
                    case ContestType.TopCoder:
                        //TODO: rejudge all records including system test.
                        break;
                }
            }
        }
    }
}
