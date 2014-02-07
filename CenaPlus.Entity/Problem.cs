using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CenaPlus.Entity
{
    [Table("problems")]
    public class Problem
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("score")]
        public int Score { get; set; }

        [Column("time_limit")]
        public int TimeLimit { get; set; } // in ms

        [Column("memory_limit")]
        public long MemoryLimit { get; set; } // in bytes

        [Column("std")]
        public string Std { get; set; }

        [Column("spj")]
        public string Spj { get; set; }

        [Column("validator")]
        public string Validator { get; set; }

        [Column("std_language")]
        public int? StdLanguageAsInt { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public ProgrammingLanguage? StdLanguage
        {
            get { return (ProgrammingLanguage?)StdLanguageAsInt; }
            set { StdLanguageAsInt = (int?)value; }
        }

        [Column("spj_language")]
        public int? SpjLanguageAsInt { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public ProgrammingLanguage? SpjLanguage
        {
            get { return (ProgrammingLanguage?)SpjLanguageAsInt; }
            set { SpjLanguageAsInt = (int?)value; }
        }


        [Column("validator_language")]
        public int? ValidatorLanguageAsInt { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public ProgrammingLanguage? ValidatorLanguage
        {
            get { return (ProgrammingLanguage?)ValidatorLanguageAsInt; }
            set { ValidatorLanguageAsInt = (int?)value; }
        }

        [Column("contest_id")]
        [ForeignKey("Contest")]
        public int ContestID { get; set; }//for db
        
        [IgnoreDataMember]
        public virtual Contest Contest { get; set; }//for navigation

        [NotMapped]
        public string ContestTitle { get; set; }//for client

        [IgnoreDataMember]
        public virtual ICollection<TestCase> TestCases { get; set; }

        [NotMapped]
        public int TestCasesCount { get; set; }
    }
}
