using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CenaPlus.Cloud.Entity
{
    [Table("email_forbiddens")]
    public class EmailForbidden
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("address")]
        public string Address { get; set; }
    }
}
