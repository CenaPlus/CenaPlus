using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace CenaPlus.Entity
{
    [Table("questions")]
    public class Question
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("asker_id")]
        [ForeignKey("Asker")]
        public int AskerID { get; set; }

        [NotMapped]
        public string AskerNickName { get; set; }

        [IgnoreDataMember]
        public virtual User Asker { get; set; }

        [Column("contest_id")]
        [ForeignKey("Contest")]
        public int ContestID { get; set; }

        [NotMapped]
        public string ContestName { get; set; }

        [IgnoreDataMember]
        public virtual Contest Contest { get; set; }

        [Column("time")]
        public DateTime Time { get; set; }

        [Column("status")]
        public int StatusAsInt { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public QuestionStatus Status
        {
            get { return (QuestionStatus)StatusAsInt; }
            set { StatusAsInt = (int)value; }
        }

        [Column("description")]
        public string Description { get; set; }

        [Column("answer")]
        public string Answer { get; set; }
    }

    public enum QuestionStatus
    {
        Pending, Private, Public,Rejected
    }
}
