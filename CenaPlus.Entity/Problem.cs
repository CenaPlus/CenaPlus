using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;

namespace CenaPlus.Entity
{
    [Table("problems")]
    public class Problem
    {
        [Column("id")]
        [ScriptIgnore]
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
        [ScriptIgnore]
        public int? StdLanguageAsInt { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public ProgrammingLanguage? StdLanguage
        {
            get { return (ProgrammingLanguage?)StdLanguageAsInt; }
            set { StdLanguageAsInt = (int?)value; }
        }

        [Column("spj_language")]
        [ScriptIgnore]
        public int? SpjLanguageAsInt { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public ProgrammingLanguage? SpjLanguage
        {
            get { return (ProgrammingLanguage?)SpjLanguageAsInt; }
            set { SpjLanguageAsInt = (int?)value; }
        }


        [Column("validator_language")]
        [ScriptIgnore]
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
        [ScriptIgnore]
        public int ContestID { get; set; }//for db

        [IgnoreDataMember]
        [ScriptIgnore]
        public virtual Contest Contest { get; set; }//for navigation

        [NotMapped]
        [ScriptIgnore]
        public string ContestTitle { get; set; }//for client

        [Column("forbidden_languages")]
        [ScriptIgnore]
        public string ForbiddenLanguagesAsString { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public IEnumerable<ProgrammingLanguage> ForbiddenLanguages
        {
            get
            {
                return ForbiddenLanguagesAsString.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(l => (ProgrammingLanguage)Enum.Parse(typeof(ProgrammingLanguage), l));
            }
            set
            {
                ForbiddenLanguagesAsString = string.Join("|", value);
            }
        }

        [IgnoreDataMember]
        [ScriptIgnore]
        public virtual ICollection<TestCase> TestCases { get; set; }

        [NotMapped]
        [ScriptIgnore]
        public int TestCasesCount { get; set; }

        [IgnoreDataMember]
        [ScriptIgnore]
        public virtual ICollection<User> LockedUsers { get; set; }

        public override bool Equals(object obj)
        {
            Problem other = obj as Problem;
            if (other == null) return false;
            return other.ID == this.ID;
        }

        public override int GetHashCode()
        {
            return ID;
        }
    }
}
