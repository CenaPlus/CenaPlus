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
        public void NewRecord(int recordID)
        {
            using (DB db = new DB())
            {
                Record record = db.Records.Find(recordID);
                Record r = new Record
                {
                    ID = record.ID,
                    Code = record.Code,
                    Detail = record.Detail,
                    Language = record.Language,
                    MemoryUsage = record.MemoryUsage,
                    ProblemID = record.ProblemID,
                    ProblemTitle = record.Problem.Title,
                    Score = record.Score,
                    Status = record.Status,
                    SubmissionTime = record.SubmissionTime,
                    TimeUsage = record.TimeUsage,
                    UserID = record.UserID,
                    UserNickName = record.UserNickName
                };

                foreach (var s in App.Clients.Values.Where(s => s.SessionMode == LocalCenaServer.SessionType.Server))
                {
                    System.Threading.Tasks.Task.Factory.StartNew(() => s.Callback.NewRecord(r));
                }
            }
        }
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
                    DefenderUserNickName = hack.Record.User.NickName,
                    HackerUserID = hack.HackerID,
                    HackerUserNickName = (from u in db.Users where u.ID == hack.HackerID select u.NickName).FirstOrDefault(),
                    Status = hack.Status,
                    RecordID = hack.RecordID,
                    ProblemTitle = hack.Record.Problem.Title,
                    Time = hack.Time
                };
                var contest_id = hack.Record.Problem.ContestID;
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
                    StandingsCache.UpdateSingleDetail(hack.HackeeID, hack.Record.ProblemID, contest_id, hack.Record.Problem.Contest.Type);
                    foreach (var client in App.Clients.Values)
                    {
                        System.Threading.Tasks.Task.Factory.StartNew(() =>
                        {
                            client.Callback.StandingsPush(contest_id, (StandingsCache.Standings[contest_id] as List<Entity.StandingItem>).Find(x => x.UserID == hack.HackeeID));
                        });
                    }
                }
                if (hack.Status == HackStatus.Success || hack.Status == HackStatus.Failure)
                {
                    StandingsCache.UpdateSingleUser(hack.HackerID, contest_id, (from p in db.Problems
                                                                                where p.ContestID == contest_id
                                                                                orderby p.Score ascending
                                                                                select p.ID).ToList());
                    foreach (var client in App.Clients.Values)
                    {
                        System.Threading.Tasks.Task.Factory.StartNew(() =>
                        {
                            client.Callback.StandingsPush(contest_id, (StandingsCache.Standings[contest_id] as List<Entity.StandingItem>).Find(x => x.UserID == hack.HackerID));
                        });
                    }
                }
                foreach (var client in App.Clients.Values)
                {
                    System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                        client.Callback.HackResultPush(re);
                    });
                }
            }
        }
        public void JudgeFinished(int record_id)
        {
            using (DB db = new DB())
            {
                Record record = (from r in db.Records
                                 where r.ID == record_id
                                 select r).FirstOrDefault();
                var Type = record.Problem.Contest.Type;
                if (Type == ContestType.OI) return;
                var dtl = (record.Problem.Contest.Type == ContestType.ACM && record.Status != RecordStatus.CompileError ? "" : record.Detail);
                Result re = new Result()
                {
                    StatusID = record.ID,
                    Status = record.Status,
                    TimeUsage = record.TimeUsage,
                    MemoryUsage = record.MemoryUsage,
                    UserID = record.UserID,
                    SubmissionTime = record.SubmissionTime,
                    UserNickName = record.UserNickName,
                    Detail = dtl,
                    Language = record.Language,
                    ProblemTitle = record.Problem.Title
                };
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    LocalCenaServer client;
                    if (App.Clients.TryGetValue(record.UserID, out client))
                    {
                        client.Callback.JudgeFinished(re);
                    }
                });

                // Push to remote management servers
                foreach (var s in App.Clients.Values.Where(s => s.SessionMode == LocalCenaServer.SessionType.Server))
                {
                    System.Threading.Tasks.Task.Factory.StartNew(() => s.Callback.JudgeFinished(re));
                }

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
