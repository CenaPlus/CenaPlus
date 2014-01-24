using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
namespace TestDB
{
    enum ContestType
    {
        CF, OI
    }
    [Table("contests")]
    class Contest
    {
        [Column("id")]
        public long ID { get; set; }

        [Column("type")]
        public ContestType Type { get; set; }

        [ForeignKey("ID")]
        public virtual ICollection<Problem> Problems { get; set; }
    }
    [Table("problems")]
    class Problem
    {
        [Column("id")]
        public long ID { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("contest_id")]
        [ForeignKey("Contest")]
        public long ContestID { get; set; }

        public Contest Contest { get; set; }
    }
    class DB : DbContext
    {
        public DbSet<Problem> Problems { get; set; }
        public DbSet<Contest> Contests { get; set; }
        public DB()
            : base("mysqldb")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Problem>().HasRequired(s => s.Contest)
           //.WithMany(s => s.Problems).HasForeignKey(s => s.ContestID);
        }
    }
}
