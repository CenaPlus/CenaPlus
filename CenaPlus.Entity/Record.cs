using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
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

        [IgnoreDataMember]
        public virtual User User { get; set; }//For navigation

        [NotMapped]
        public string UserNickName { get; set; }//For client

        [Column("problem_id")]
        [ForeignKey("Problem")]
        public int ProblemID { get; set; }//For db

        [IgnoreDataMember]
        public virtual Problem Problem { get; set; }//For navigation

        [NotMapped]
        public string ProblemTitle { get; set; }//For client

        [Column("status")]
        public int StatusAsInt { get; set; }

        [IgnoreDataMember]
        [NotMapped]
        public RecordStatus Status
        {
            get { return (RecordStatus)StatusAsInt; }
            set { StatusAsInt = (int)value; }
        }

        [Column("language")]
        public int LanguageAsInt { get; set; }

        [IgnoreDataMember]
        [NotMapped]
        public ProgrammingLanguage Language
        {
            get { return (ProgrammingLanguage)LanguageAsInt; }
            set { LanguageAsInt = (int)value; }
        }

        [Column("code")]
        public string Code { get; set; }

        [Column("submission_time")]
        public DateTime SubmissionTime { get; set; }

        [Column("time_usage")]
        public int? TimeUsage { get; set; }//in millisecond

        [Column("memory_usage")]
        public long? MemoryUsage { get; set; }//in bytes

        [Column("detail")]
        public string Detail { get; set; }
    }

    public enum RecordStatus
    {
        Accepted, PresentationError, WrongAnswer, OutputLimitExceeded, ValidatorError,//Validator Level
        MemoryLimitExceeded, TimeLimitExceeded, RuntimeError, RestrictedFunction,//Runner Level
        CompileError, SystemError,//System Level
        Running, Pending//DB Level
    }

    public enum ProgrammingLanguage
    {
        C, CXX, CXX11, Java, Pascal, Python27, Python33, Ruby
    }
}
