using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CenaPlus.Server.Bll
{
    public static class StandingsCache
    {
        public static Hashtable Standings = new System.Collections.Hashtable();
        public static void Rebuild(int contest_id)//Center server only
        {
            Standings[contest_id] = null;
            GC.Collect();
            Standings[contest_id] = new List<Entity.StandingItem>();
            using (Dal.DB db = new Dal.DB())
            { 
                var problemids = (from p in db.Problems
                                      where p.ContestID == contest_id
                                      select p.ID).ToList();
                var userids = (from r in db.Records
                               where problemids.Contains(r.ProblemID)
                               select r.UserID).Distinct().ToList();
                foreach(int uid in userids)
                {
                    Entity.StandingItem standings = new Entity.StandingItem();
                    standings.Type = (Entity.ContestType)(from c in db.Contests
                                     where c.ID == contest_id
                                     select c.TypeAsInt).FirstOrDefault();
                    standings.UserID = uid;
                    standings.Competitor = (from u in db.Users
                                           where u.ID == uid
                                           select u.NickName).FirstOrDefault();
                    if (standings.Type == Entity.ContestType.Codeforces || standings.Type == Entity.ContestType.TopCoder)
                    {
                        standings.SuccessfulHack = (from h in db.Hacks
                                           where h.HackerID == uid
                                           && h.StatusAsInt == (int)Entity.HackStatus.Success
                                           && problemids.Contains(h.Record.Problem.ID)
                                           select h.ID).Count();
                        standings.UnsuccessfulHack = (from h in db.Hacks
                                           where h.HackerID == uid
                                           && h.StatusAsInt == (int)Entity.HackStatus.Failure
                                           && problemids.Contains(h.Record.Problem.ID)
                                           select h.ID).Count();
                    }
                    foreach (int pid in problemids)
                    {
                        Entity.StandingDetail detail = new Entity.StandingDetail();
                        switch (standings.Type)
                        {
                            case Entity.ContestType.OI:
                                {
                                    var r = Dal.RecordHelper.GetLastRecord(uid, pid);
                                    //TODO: Get the score

                                    detail.FirstScore = 100;
                                    detail.SecondScore = (int)r.TimeUsage;
                                    break;
                                }
                            case Entity.ContestType.ACM:
                                {
                                    var r = Dal.RecordHelper.GetFirstAcceptedRecord(uid, pid);
                                    if (r != null)
                                    {
                                        detail.FirstScore = Dal.RecordHelper.GetEffectiveCount(uid, pid, r.SubmissionTime);
                                        var BeginTime = (from c in db.Contests
                                                         where c.ID == contest_id
                                                         select c.StartTime).FirstOrDefault();
                                        detail.SecondScore = Convert.ToInt32((r.SubmissionTime - BeginTime).TotalSeconds) + 60 * 20 * (detail.FirstScore - 1);
                                    }
                                    else
                                    {
                                        detail.FirstScore = Dal.RecordHelper.GetEffectiveCount(uid, pid);
                                        detail.SecondScore = 0;
                                    }
                                    break;
                                }
                            case Entity.ContestType.TopCoder:
                                {
                                    var r = Dal.RecordHelper.GetLastRecord(uid, pid);
                                    if(r!=null)
                                    {
                                        detail.RecordID = r.ID;
                                    }
                                    break;
                                }
                            case Entity.ContestType.Codeforces:
                                {
                                    var r = Dal.RecordHelper.GetLastRecord(uid, pid);
                                    if (r != null)
                                    {
                                        detail.RecordID = r.ID;
                                        detail.ThirdScore = Dal.RecordHelper.GetNoZeroPtsEffectiveCount(uid, pid);
                                        if (r.Status == Entity.RecordStatus.Accepted)
                                        {
                                            int problem_pts = (from p in db.Problems
                                                               where p.ID == pid
                                                               select p.Score).FirstOrDefault();
                                            DateTime BeginTime = (from c in db.Contests
                                                                  where c.ID == contest_id
                                                                  select c.StartTime).FirstOrDefault();
                                            int seconds = (int)(r.SubmissionTime - BeginTime).TotalSeconds;
                                            int minutes = seconds / 60;
                                            detail.FirstScore = Convert.ToInt32(problem_pts * (1 - 0.004 * minutes));
                                            if(detail.FirstScore < problem_pts*0.3)
                                                detail.FirstScore  = (int)(problem_pts*0.3);
                                            detail.SecondScore = seconds;
                                        }
                                    }
                                    else
                                    {
                                        detail.FirstScore = 0;
                                        detail.SecondScore = 0;
                                        detail.ThirdScore = Dal.RecordHelper.GetNoZeroPtsEffectiveCount(uid, pid);
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
        }
    }
}
