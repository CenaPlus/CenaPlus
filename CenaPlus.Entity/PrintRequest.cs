using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
namespace CenaPlus.Entity
{
    [Table("print_requests")]
    public class PrintRequest
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("user_id")]
        [ForeignKey("User")]
        public int UserID { get; set; }

        [IgnoreDataMember]
        public virtual User User { get; set; }

        [NotMapped]
        public string UserNickName { get; set; }

        [Column("contest_id")]
        [ForeignKey("Contest")]
        public int ContestID { get; set; }

        [IgnoreDataMember]
        public virtual Contest Contest { get; set; }

        [NotMapped]
        public string ContestTitle { get; set; }

        [Column("copies")]
        public int Copies { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("time")]
        public DateTime Time { get; set; }

        [Column("status")]
        public int StatusAsInt { get; set; }

        [IgnoreDataMember]
        [NotMapped]
        public PrintRequestStatus Status
        {
            get { return (PrintRequestStatus)StatusAsInt; }
            set { StatusAsInt = (int)value; }
        }
    }

    public enum PrintRequestStatus
    {
        Pending, Printing, Done, Rejected
    }
}
