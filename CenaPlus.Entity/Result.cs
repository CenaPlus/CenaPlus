using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CenaPlus.Entity
{
    public class Result
    {
        public int StatusID { get; set; }
        public string ProblemTitle { get; set; }
        public long? MemoryUsage { get; set; }
        public int? TimeUsage { get; set; }
        public string Detail { get; set; }
        public ProgrammingLanguage Language { get; set; }
        public RecordStatus Status { get; set; }
        public DateTime SubmissionTime { get; set; }
        public int UserID { get; set; }
        public string UserNickName { get; set; }
    }
}
