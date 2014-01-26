using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace CenaPlus.Entity
{
    [Table("contests")]
    public class Contest
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("start_time")]
        public DateTime StartTime { get; set; }

        [Column("end_time")]
        public DateTime EndTime { get; set; }

        [NotMapped]
        public TimeSpan Duration { get { return EndTime - StartTime; } }

        [Column("type")]
        public ContestType Type { get; set; }

        public virtual ICollection<Problem> Problems { get; set; }
    }

    public enum ContestType
    {
        OI, ACM, Codeforces, TopCoder
    }
}
