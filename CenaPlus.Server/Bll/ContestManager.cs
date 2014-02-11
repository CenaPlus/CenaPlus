using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using CenaPlus.Server.Dal;
using CenaPlus.Entity;
namespace CenaPlus.Server.Bll
{
    class ContestManager
    {
        private Dictionary<int, List<Timer>> timers = new Dictionary<int, List<Timer>>();

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
                        var whenStart = new Timer(contest.StartTime.Millisecond - DateTime.Now.Millisecond);
                        whenStart.AutoReset = false;
                        whenStart.Elapsed += (o, e) => WhenContestStart(contestID);
                        whenStart.Start();
                        newTimers.Add(whenStart);
                    }

                    if (contest.EndTime > DateTime.Now)
                    {
                        var whenEnd = new Timer(contest.EndTime.Millisecond - DateTime.Now.Millisecond);
                        whenEnd.AutoReset = false;
                        whenEnd.Elapsed += (o, e) => WhenContestEnd(contestID);
                        whenEnd.Start();
                        newTimers.Add(whenEnd);
                    }

                    timers.Add(contestID, newTimers);
                }
            }
        }

        private void WhenContestStart(int contestID)
        {

        }

        private void WhenContestEnd(int contestID)
        {
        }
    }
}
