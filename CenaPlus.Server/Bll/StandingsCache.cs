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
        /// <summary>
        /// 题目编号->比赛题目索引
        /// </summary>
        /// <param name="problem_id"></param>
        /// <returns></returns>
        public static int FindProblem(int problem_id, int contest_id)
        {
            using (Dal.DB db = new Dal.DB())
            {
                var problemids = (from p in db.Problems
                                  where p.ContestID == contest_id
                                  orderby p.Score ascending
                                  select p.ID).ToList();
                return problemids.FindIndex(x => x == problem_id);
            }
        }
        /// <summary>
        /// 更新某用户某题的排名元素
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="problem_id"></param>
        /// <param name="contest_id"></param>
        /// <param name="Type"></param>
        public static void UpdateSingleDetail(int user_id, int problem_id, int contest_id, Entity.ContestType Type)
        {
            using (Dal.DB db = new Dal.DB())
            {
                Entity.StandingDetail detail = new Entity.StandingDetail();
                detail.DisplayFormat = Type;
                switch (Type)
                {
                    case Entity.ContestType.OI:
                        {
                            var r = Dal.RecordHelper.GetLastRecord(user_id, problem_id);
                            if (r != null)
                            {
                                detail.FirstScore = r.Score.GetValueOrDefault();
                                detail.SecondScore = r.TimeUsage.GetValueOrDefault();
                            }
                            break;
                        }
                    case Entity.ContestType.ACM:
                        {
                            var r = Dal.RecordHelper.GetFirstAcceptedRecord(user_id, problem_id);
                            if (r != null)
                            {
                                detail.FirstScore = Dal.RecordHelper.GetEffectiveCount(user_id, problem_id, r.SubmissionTime);
                                var BeginTime = (from c in db.Contests
                                                 where c.ID == contest_id
                                                 select c.StartTime).FirstOrDefault();
                                detail.SecondScore = Convert.ToInt32((r.SubmissionTime - BeginTime).TotalSeconds) + 60 * 20 * (detail.FirstScore - 1);
                            }
                            else
                            {
                                detail.FirstScore = Dal.RecordHelper.GetEffectiveCount(user_id, problem_id);
                                detail.SecondScore = 0;
                            }
                            break;
                        }
                    case Entity.ContestType.TopCoder:
                        {
                            var r = Dal.RecordHelper.GetLastRecord(user_id, problem_id);
                            if (r != null)
                            {
                                detail.RecordID = r.ID;
                                detail.ThirdScore = Dal.RecordHelper.GetNoZeroPtsEffectiveCount(user_id, problem_id);
                                if (r.Status == Entity.RecordStatus.Accepted)
                                {
                                    int problem_pts = (from p in db.Problems
                                                       where p.ID == problem_id
                                                       select p.Score).FirstOrDefault();
                                    DateTime BeginTime = (from pv in db.ProblemViews
                                                          where pv.ProblemID == problem_id
                                                          && pv.UserID == user_id
                                                          select pv.Time).FirstOrDefault();
                                    int seconds = (int)(r.SubmissionTime - BeginTime).TotalSeconds;
                                    int minutes = seconds / 60;
                                    detail.FirstScore = Convert.ToInt32(problem_pts * (1 - 0.004 * minutes * minutes));//Topcoder动态分数计算公式
                                    if (detail.FirstScore < problem_pts * 0.3)
                                        detail.FirstScore = (int)(problem_pts * 0.3);
                                    detail.SecondScore = seconds;
                                }
                            }
                            else
                            {
                                detail.FirstScore = 0;
                                detail.SecondScore = 0;
                                detail.ThirdScore = Dal.RecordHelper.GetNoZeroPtsEffectiveCount(user_id, problem_id);
                            }
                            break;
                        }
                    case Entity.ContestType.Codeforces:
                        {
                            var r = Dal.RecordHelper.GetLastRecord(user_id, problem_id);
                            if (r != null)
                            {
                                detail.RecordID = r.ID;
                                detail.ThirdScore = Dal.RecordHelper.GetNoZeroPtsEffectiveCount(user_id, problem_id);
                                if (r.Status == Entity.RecordStatus.Accepted)
                                {
                                    int problem_pts = (from p in db.Problems
                                                       where p.ID == problem_id
                                                       select p.Score).FirstOrDefault();
                                    DateTime BeginTime = (from c in db.Contests
                                                          where c.ID == contest_id
                                                          select c.StartTime).FirstOrDefault();
                                    int seconds = (int)(r.SubmissionTime - BeginTime).TotalSeconds;
                                    int minutes = seconds / 60;
                                    detail.FirstScore = Convert.ToInt32(problem_pts * (1 - 0.004 * minutes));//Codeforces动态分数计算公式
                                    if (detail.FirstScore < problem_pts * 0.3)
                                        detail.FirstScore = (int)(problem_pts * 0.3);
                                    detail.SecondScore = seconds;
                                }
                            }
                            else
                            {
                                detail.FirstScore = 0;
                                detail.SecondScore = 0;
                                detail.ThirdScore = Dal.RecordHelper.GetNoZeroPtsEffectiveCount(user_id, problem_id);
                            }
                            break;
                        }
                }
                int userindex = (Standings[contest_id] as List<Entity.StandingItem>).FindIndex(x=>x.UserID == user_id);
                (Standings[contest_id] as List<Entity.StandingItem>)[userindex].Details[FindProblem(problem_id, contest_id)] = detail;
            }
        }
        /// <summary>
        /// 重构某用户的排名信息
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="contest_id"></param>
        /// <param name="problemids"></param>
        public static void UpdateSingleUser(int user_id, int contest_id, List<int> problemids)
        {
            using (Dal.DB db = new Dal.DB())
            {
                Entity.StandingItem standing = new Entity.StandingItem();
                standing.Type = (Entity.ContestType)(from c in db.Contests
                                                      where c.ID == contest_id
                                                      select c.TypeAsInt).FirstOrDefault();
                standing.UserID = user_id;
                standing.Competitor = (from u in db.Users
                                        where u.ID == user_id
                                        select u.NickName).FirstOrDefault();
                if (standing.Type == Entity.ContestType.Codeforces || standing.Type == Entity.ContestType.TopCoder)
                {
                    standing.SuccessfulHack = (from h in db.Hacks
                                                where h.HackerID == user_id
                                                && h.StatusAsInt == (int)Entity.HackStatus.Success
                                                && problemids.Contains(h.Record.Problem.ID)
                                                select h.ID).Count();
                    standing.UnsuccessfulHack = (from h in db.Hacks
                                                  where h.HackerID == user_id
                                                  && h.StatusAsInt == (int)Entity.HackStatus.Failure
                                                  && problemids.Contains(h.Record.Problem.ID)
                                                  select h.ID).Count();
                }
                if (standing.Details == null)
                    standing.Details = new Entity.StandingDetail[problemids.Count];
                int userindex=(Standings[contest_id] as List<Entity.StandingItem>).FindIndex(x=>x.UserID==user_id);
                if (userindex == -1)
                {
                    (Standings[contest_id] as List<Entity.StandingItem>).Add(standing);
                }
                else
                {
                    (Standings[contest_id] as List<Entity.StandingItem>)[userindex] = standing;
                }
                foreach (int problem_id in problemids)
                {
                    UpdateSingleDetail(user_id, problem_id, contest_id, standing.Type);
                }
            }
        }
        /// <summary>
        /// 重新构建排名
        /// </summary>
        /// <param name="contest_id"></param>
        public static void Rebuild(int contest_id)//Center server only
        {
            Standings[contest_id] = null;
            GC.Collect();
            Standings[contest_id] = new List<Entity.StandingItem>();
            using (Dal.DB db = new Dal.DB())
            {
                var problemids = (from p in db.Problems
                                  where p.ContestID == contest_id
                                  orderby p.Score ascending
                                  select p.ID).ToList();
                var userids = (from r in db.Records
                               where problemids.Contains(r.ProblemID)
                               select r.UserID).Distinct().ToList();
                foreach (int uid in userids)
                {
                    UpdateSingleUser(uid, contest_id, problemids);
                }
            }
        }
    }
}
