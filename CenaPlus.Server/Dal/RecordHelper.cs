using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CenaPlus.Entity;
using CenaPlus.Server.Dal;

namespace CenaPlus.Server.Dal
{
    public static class RecordHelper
    {
        /// <summary>
        /// 获取最后一条记录
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="problem_id"></param>
        /// <returns></returns>
        public static Record GetLastRecord(int user_id, int problem_id)
        {
            using (DB db = new DB())
            {
                Record record = (from r in db.Records
                                 where r.UserID == user_id
                                 && r.ProblemID == problem_id
                                 orderby r.SubmissionTime descending
                                 select r).FirstOrDefault();
                return record;
            }
        }
        /// <summary>
        /// 获取最后一条AC的记录
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="problem_id"></param>
        /// <returns></returns>
        public static Record GetLastAcceptedRecord(int user_id, int problem_id)
        {
            using (DB db = new DB())
            {
                Record record = (from r in db.Records
                                 where r.UserID == user_id
                                 && r.ProblemID == problem_id
                                 && r.StatusAsInt == (int)RecordStatus.Accepted
                                 orderby r.SubmissionTime descending
                                 select r).FirstOrDefault();
                return record;
            }
        }
        /// <summary>
        /// 获取第一个AC状态的记录
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="problem_id"></param>
        /// <returns></returns>
        public static Record GetFirstAcceptedRecord(int user_id, int problem_id)
        {
            using (DB db = new DB())
            {
                Record record = (from r in db.Records
                                 where r.UserID == user_id
                                 && r.ProblemID == problem_id
                                 && r.StatusAsInt == (int)RecordStatus.Accepted
                                 orderby r.SubmissionTime ascending
                                 select r).FirstOrDefault();
                return record;
            }
        }
        /// <summary>
        /// 有效状态
        /// </summary>
        public static readonly RecordStatus[] EffectiveStatus = 
        {
            RecordStatus.Accepted,
            RecordStatus.Hacked,
            RecordStatus.MemoryLimitExceeded,
            RecordStatus.TimeLimitExceeded,
            RecordStatus.RuntimeError,
            RecordStatus.WrongAnswer,
            RecordStatus.PresentationError
        };
        /// <summary>
        /// 获取有效记录个数
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="problem_id"></param>
        /// <returns></returns>
        public static int GetEffectiveCount(int user_id, int problem_id)
        {
            using (DB db = new DB())
            {
                int count = (from r in db.Records
                             where r.UserID == user_id
                             && r.ProblemID == problem_id
                             && EffectiveStatus.Contains((Entity.RecordStatus)r.StatusAsInt)
                             select r.ID).Count();
                return count;
            }
        }
        /// <summary>
        /// 获取在time前的有效记录个数
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="problem_id"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int GetEffectiveCount(int user_id, int problem_id, DateTime time)
        {
            using (DB db = new DB())
            {
                int count = (from r in db.Records
                             where r.UserID == user_id
                             && r.ProblemID == problem_id
                             && EffectiveStatus.Contains((Entity.RecordStatus)r.StatusAsInt)
                             && r.SubmissionTime <= time
                             select r.ID).Count();
                return count;
            }
        }
    }
}
