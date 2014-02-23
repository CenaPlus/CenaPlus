using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using CenaPlus.Entity;

namespace CenaPlus.Cloud.Web.Dal
{
    public class Cena : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Contest> Contests { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Hack> Hacks { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<TestCase> TestCases { get; set; }
        public DbSet<PrintRequest> PrintRequests { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<ProblemView> ProblemViews { get; set; }
        //public DbSet<ProblemLocks> ProblemLocks { get; set; }

        public Cena()
            : base("mysqldb")
        {
        }
        public Cena(string nameOrConnectionString) : base(nameOrConnectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Problem>()
                .HasMany(p => p.LockedUsers)
                .WithMany(u => u.LockedProblems)
                .Map(c => c.MapLeftKey("problem_id").MapRightKey("user_id").ToTable("problem_locks"));
        }
    }
}