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
