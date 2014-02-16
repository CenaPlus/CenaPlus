using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CenaPlus.Entity
{
    [Table("problem_views")]
    public class ProblemView
    {
        [Key]
        [ForeignKey("Problem")]
        [Column("problem_id", Order = 0)]
        public int ProblemID { get; set; }

        [IgnoreDataMember]
        public virtual Problem Problem { get; set; }

        [Key]
        [ForeignKey("User")]
        [Column("user_id", Order = 1)]
        public int UserID { get; set; }

        [IgnoreDataMember]
        public virtual User User { get; set; }

        [Column("time")]
        public DateTime Time { get; set; }
    }
}
