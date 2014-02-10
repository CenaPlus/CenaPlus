using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using CenaPlus.Entity;
namespace CenaPlus.Server.Dal
{
    public class DB : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Contest> Contests { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<TestCase> TestCases { get; set; }
        public DbSet<PrintRequest> PrintRequests { get; set; }
        public DbSet<Config> Configs { get; set; }

        public DB() :base("mysqldb") {
        }
        public DB(string nameOrConnectionString) : base(nameOrConnectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
