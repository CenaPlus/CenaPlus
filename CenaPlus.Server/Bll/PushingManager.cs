using CenaPlus.Entity;
using CenaPlus.Server.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CenaPlus.Server.Bll
{
    public class PushingManager
    {
        public void HackFinished(int hack_id)
        {
            using (DB db = new DB())
            {
                Hack hack = (from h in db.Hacks
                             where h.ID == hack_id
                             select h).FirstOrDefault();
                HackResult re = new HackResult()
                {
                    HackID = hack.ID,
                    DefenderUserID = hack.HackeeID,
                    DefenderUserNickName = hack.HackeeNickName,
                    HackerUserID = hack.HackerID,
                    HackerUserNickName = hack.HackerNickName,
                    Status = hack.Status,
                    RecordID = hack.RecordID,
                    ProblemTitle = hack.Record.ProblemTitle,
                    Time = hack.Time
                };
                if (hack.Status == HackStatus.Success)
                {
                    System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                        LocalCenaServer client;
                        if (App.Clients.TryGetValue(hack.HackeeID, out client))
                        {
                            client.Callback.BeHackedPush(re);
                        }
                    });
                }
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    LocalCenaServer client;
                    if (App.Clients.TryGetValue(hack.HackerID, out client))
                    {
                        client.Callback.HackResultPush(re);
                    }
                });
            }
        }
        public void JudgeFinished(int record_id)
        {
            using (DB db = new DB())
            {
                Record record = (from r in db.Records
                                 where r.ID == record_id
                                 select r).FirstOrDefault();
                Result re = new Result()
                {
                    StatusID = record.ID,
                    Status = record.Status,
                    TimeUsage = record.TimeUsage,
                    MemoryUsage = record.MemoryUsage,
                    UserID = record.UserID,
                    SubmissionTime = record.SubmissionTime,
                    UserNickName = record.UserNickName,
                    Detail = record.Detail,
                    Language = record.Language,
                    ProblemTitle = record.Problem.Title
                };
                var Type = record.Problem.Contest.Type;
                if (Type == ContestType.OI) return;
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    LocalCenaServer client;
                    if (App.Clients.TryGetValue(record.UserID, out client))
                    {
                        client.Callback.JudgeFinished(re);
                    }
                });
                if (StandingsCache.Standings[record.Problem.ContestID] == null)
                {
                    StandingsCache.Rebuild(record.Problem.ContestID);
                }
                var userindex = (StandingsCache.Standings[record.Problem.ContestID] as List<Entity.StandingItem>).FindIndex(x => x.UserID == record.UserID);
                if (userindex == -1)
                {
                    StandingsCache.UpdateSingleUser(record.UserID, record.Problem.ContestID, (from p in db.Problems where p.ContestID == record.Problem.ContestID select p.ID).ToList());
                }
                else
                {
                    StandingsCache.UpdateSingleDetail(record.UserID, record.ProblemID, record.Problem.ContestID, Type);
                }
                foreach (var c in App.Clients.Values)
                {
                    System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                        c.Callback.StandingsPush(record.Problem.ContestID, (StandingsCache.Standings[record.Problem.ContestID] as List<Entity.StandingItem>).Find(x => x.UserID == record.UserID));
                    });
                }
            }
        }
    }
}
