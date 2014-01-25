using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Column("contest_id")]
        [ForeignKey("Contest")]
        public int ContestID { get; set; }

        public virtual Contest Contest { get; set; }
    }
}
