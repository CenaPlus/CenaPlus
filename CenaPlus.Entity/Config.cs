using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace CenaPlus.Entity
{
    [Table("configs")]
    public class Config
    {
        [Column("key")]
        [Key]
        public string Key { get; set; }

        [Column("value")]
        public string Value { get; set; }
    }
}
