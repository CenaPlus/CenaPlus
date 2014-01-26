using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace CenaPlus.Entity
{
    [Table("records")]
    public class Record
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("user_id")]
        [ForeignKey("User")]
        public int UserID { get; set; }//For DB
        public virtual User User { get; set; }//For navigation
        [NotMapped]
        public string UserName { get; set; }//For client

        [Column("problem_id")]
        [ForeignKey("Problem")]
        public int ProblemID { get; set; }//For db
        public virtual Problem Problem { get; set; }//For navigation
        [NotMapped]
        public string ProblemTitle { get; set; }//For client

        [Column("status")]
        public RecordStatus Status { get; set; }

        [Column("language")]
        public ProgrammingLangauge Language { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("time_usage")]
        public int TimeUsage { get; set; }//in millisecond

        [Column("memory_usage")]
        public long MemoryUsage { get; set; }//in bytes

        [Column("detail")]
        public string Detail { get; set; }
    }

    public enum RecordStatus
    {
        Pending, Running, Accepted,
        CompileError, WrongAnswer, RuntimeError, PresentationError,
        TimeLimitExceeded, MemoryLimitExceeded, OutputLimitExceeded
    }

    public enum ProgrammingLangauge
    {
        C, Pascal
    }
}
