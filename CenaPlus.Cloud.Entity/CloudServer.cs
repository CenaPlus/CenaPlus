using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CenaPlus.Cloud.Entity
{
    [Table("cloud_servers")]
    public class CloudServer
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("port")]
        public int Port { get; set; }

        [Column("client_secret")]
        public string ClientSecret { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("status")]
        public int StatusAsInt { get; set; }

        [Column("expired_time")]
        public DateTime ExpiredTime { get; set; }

        [NotMapped]
        public CloudServerStatus Status 
        {
            get { return (CloudServerStatus)StatusAsInt; }
            set { StatusAsInt = (int)value; }
        }
    }
    public enum CloudServerStatus { Vertifying, Normal, Expired };
}
