using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using CenaPlus.Cloud.Entity;

namespace CenaPlus.Cloud.Web.Dal
{
    public class Cloud : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<EmailForbidden> EmailForbidden { get; set; }
        public Cloud()
            : base("mysqldb")
        { 
        }
    }
}