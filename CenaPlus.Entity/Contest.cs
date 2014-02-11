using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
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

        [Column("rest_time")]
        public DateTime? RestTime  { get; set; }

        [Column("hack_start_time")]
        public DateTime? HackStartTime { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public TimeSpan Duration { get { return EndTime - StartTime; } }

        [Column("type")]
        public int TypeAsInt { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public ContestType Type
        {
            get { return (ContestType)TypeAsInt; }
            set { TypeAsInt = (int)value; }
        }

        [IgnoreDataMember]
        public virtual ICollection<Problem> Problems { get; set; }

        [Column("printing_enabled")]
        public bool PrintingEnabled { get; set; }
    }

    public enum ContestType
    {
        OI, ACM, Codeforces, TopCoder
    }
}
