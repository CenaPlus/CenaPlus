using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CenaPlus.Cloud.Entity
{
    [Table("contests")]
    public class Contest
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("begin_time")]
        public DateTime BeginTime { get; set; }

        [Column("end_time")]
        public DateTime EndTime { get; set;}

        [Column("type")]
        public int TypeAsInt { get; set; }

        [NotMapped]
        public ContestType Type 
        {
            get
            {
                return (ContestType)TypeAsInt;
            }
            set
            {
                TypeAsInt = (int)value;
            }
        }

        [Column("cloud_server_id")]
        [ForeignKey("CloudServer")]
        public int CloudServerID { get; set; }

        public virtual CloudServer CloudServer { get; set; }

        [NotMapped]
        public ContestStatus Status
        {
            get 
            {
                if (DateTime.Now < BeginTime)
                    return ContestStatus.Pending;
                else if (DateTime.Now < EndTime)
                    return ContestStatus.Live;
                else
                    return ContestStatus.Passed;
            }
        }
        public override bool Equals(object obj)
        {
            Contest other = obj as Contest;
            if (other == null) return false;
            return other.ID == this.ID;
        }

        public override int GetHashCode()
        {
            return ID;
        }
    }
    public enum ContestType
    {
        OI, ACM, Codeforces, TopCoder
    }
    public enum ContestStatus
    {
        Live, Pending, Passed
    }
}
