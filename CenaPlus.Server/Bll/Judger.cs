using CenaPlus.Server.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CenaPlus.Entity;
using CenaPlus.Network;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
namespace CenaPlus.Server.Bll
{
    class Judger
    {
        public void StartJudgeAllPending()
        {
            List<int> pendingRecordIDs;
            using (DB db = new DB())
            {
                pendingRecordIDs = (from r in db.Records
                                    where r.StatusAsInt == (int)RecordStatus.Pending
                                    select r.ID).ToList();
            }
            foreach (var id in pendingRecordIDs)
            {
                int _id = id;
                System.Threading.Tasks.Task.Factory.StartNew(() => JudgeRecord(_id));
            }
        }

        public void JudgeRecord(int recordID)
        {
            using (DB db = new DB())
            {
                Contest contest;
                Problem problem;
                Record record;
                StringBuilder detail = new StringBuilder();

                var r = db.Records.Find(recordID);
                if (r == null) return;

                if (r.Status != RecordStatus.Pending) return;
                r.Status = Entity.RecordStatus.Running;
                db.SaveChanges();

                record = new Record
                {
                    ID = r.ID,
                    Code = r.Code,
                    Language = r.Language,
                    ProblemID = r.ProblemID,
                    ProblemTitle = r.Problem.Title,
                    Status = RecordStatus.Running,
                    SubmissionTime = r.SubmissionTime,
                    UserID = r.UserID,
                    UserNickName = r.UserNickName
                };

                var p = r.Problem;
                problem = new Problem
                {
                    ID = p.ID,
                    ContestID = p.ContestID,
                    ContestTitle = p.Contest.Title,
                    ForbiddenLanguages = p.ForbiddenLanguages,
                    MemoryLimit = p.MemoryLimit,
                    Score = p.Score,
                    Spj = p.Spj,
                    SpjLanguage = p.SpjLanguage,
                    Std = p.Std,
                    StdLanguage = p.StdLanguage,
                    TestCasesCount = p.TestCases.Count,
                    TimeLimit = p.TimeLimit,
                    Title = p.Title,
                    Validator = p.Validator,
                    ValidatorLanguage = p.ValidatorLanguage
                };

                contest = p.Contest;

                using (var node = GetFreestNode())
                {
                    var ret = node.Compile(problem, record);
                    if (ret.RecordStatus != RecordStatus.Accepted)
                    {
                        r.Status = ret.RecordStatus;
                        r.Detail = ret.CompilerOutput;
                        db.SaveChanges();
                        return;
                    }

                    detail.AppendLine(ret.CompilerOutput);

                    var testCases = from t in db.TestCases
                                    where t.ProblemID == problem.ID
                                    select t;
                    if ((contest.Type == ContestType.Codeforces || contest.Type == ContestType.TopCoder) && contest.EndTime > DateTime.Now)
                    {
                        testCases = testCases.Where(t => t.Type == TestCaseType.Systemtest);
                    }

                    var testCaseIDs = testCases.Select(t => t.ID);
                    List<Task<TaskFeedback_Run>> runs = new List<Task<TaskFeedback_Run>>();
                    foreach (var id in testCaseIDs)
                    {
                        var _id = id;
                        var run = System.Threading.Tasks.Task.Factory.StartNew(() =>
                        {
                            return node.Run(problem, record, _id);
                        });
                        runs.Add(run);
                    }

                    System.Threading.Tasks.Task.WaitAll(runs.ToArray());

                    RecordStatus finalStatus = RecordStatus.Accepted;
                    int totalTime = 0;
                    long maxMemory = 0;
                    for (int i = 0; i < runs.Count; i++)
                    {
                        var result = runs[i].Result;
                        detail.AppendFormat("{0}# {1} ({2} ms, {3} KiB)\r\n", i, result.RecordStatus, result.TimeUsage, result.MemUsage / 1024);
                        if (result.RecordStatus > finalStatus)
                        {
                            finalStatus = result.RecordStatus;
                        }
                        totalTime += result.TimeUsage;
                        maxMemory = Math.Max((long)result.MemUsage, maxMemory);
                    }

                    r.Status = finalStatus;
                    r.TimeUsage = totalTime;
                    r.MemoryUsage = maxMemory;
                    r.Detail = detail.ToString();
                    db.SaveChanges();
                }
            }
        }

        public void JudgeHack(int hackID)
        {
            using (DB db = new DB())
            {
                Contest contest;
                Problem problem;
                Record record;
                Hack hack;

                var h = db.Hacks.Find(hackID);
                if (h == null) return;

                if (h.Status != HackStatus.Pending) return;
                h.Status = Entity.HackStatus.Running;
                db.SaveChanges();

                hack = new Hack
                {
                    DatamakerLanguage = h.DatamakerLanguage,
                    DataOrDatamaker = h.DataOrDatamaker,
                    HackeeID = h.Record.UserID,
                    HackeeNickName = h.Record.User.NickName,
                    HackerID = h.HackerID,
                    HackerNickName = h.Hacker.NickName,
                    RecordID = h.RecordID,
                    Status = HackStatus.Running
                };

                var r = h.Record;
                record = new Record
                {
                    ID = r.ID,
                    Code = r.Code,
                    Language = r.Language,
                    ProblemID = r.ProblemID,
                    ProblemTitle = r.Problem.Title,
                    Status = RecordStatus.Running,
                    SubmissionTime = r.SubmissionTime,
                    UserID = r.UserID,
                    UserNickName = r.UserNickName
                };

                var p = r.Problem;
                problem = new Problem
                {
                    ID = p.ID,
                    ContestID = p.ContestID,
                    ContestTitle = p.Contest.Title,
                    ForbiddenLanguages = p.ForbiddenLanguages,
                    MemoryLimit = p.MemoryLimit,
                    Score = p.Score,
                    Spj = p.Spj,
                    SpjLanguage = p.SpjLanguage,
                    Std = p.Std,
                    StdLanguage = p.StdLanguage,
                    TestCasesCount = p.TestCases.Count,
                    TimeLimit = p.TimeLimit,
                    Title = p.Title,
                    Validator = p.Validator,
                    ValidatorLanguage = p.ValidatorLanguage
                };

                using (var conn = GetFreestNode())
                {
                    var ret = conn.Hack(problem, record, hack);
                    h.Status = ret.HackStatus;

                    if (ret.HackStatus == HackStatus.Success)
                    {
                        h.Record.Status = RecordStatus.Hacked;
                        var inputHash = MD5.Create().ComputeHash(ret.HackData.Input);
                        var outputHash = MD5.Create().ComputeHash(ret.HackData.Output);

                        bool existed = (from t in db.TestCases
                                        where t.ProblemID == p.ID
                                         && t.InputHash == inputHash
                                        select t).Any();
                        if (!existed)
                        {
                            db.TestCases.Add(new TestCase
                            {
                                Input = ret.HackData.Input,
                                InputHash = inputHash,
                                Output = ret.HackData.Output,
                                OutputHash = outputHash,
                                ProblemID = p.ID,
                                Type = TestCaseType.Systemtest
                            });
                        }
                    }
                    else
                    {
                        h.Detail = ret.CompilerOutput;
                    }
                    db.SaveChanges();
                }
            }
        }

        private IJudgeNodeChannel GetFreestNode()
        {
            IJudgeNodeChannel bestNode = null;
            int maxFreeCount = -1;
            foreach (var node in App.JudgeNodes)
            {
                IJudgeNodeChannel conn = null;
                try
                {
                    conn = node.CreateConnection();

                    var free = conn.GetFreeCoreCount();
                    if (free > maxFreeCount)
                    {
                        maxFreeCount = free;
                        if (bestNode != null) bestNode.Close();
                        bestNode = conn;
                    }
                    else
                    {
                        conn.Close();
                    }
                }
                catch
                {
                    if (conn != null)
                        try
                        {
                            conn.Abort();
                        }
                        catch { }
                }
            }

            if (bestNode != null)
                return bestNode;
            else
                throw new Exception("No avaliable node");
        }
    }
}
