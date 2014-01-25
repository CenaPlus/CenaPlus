using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace CenaPlus.Server.Dal
{
    public class DB : DbContext
    {
        public DB(string nameOrConnectionString) : base(nameOrConnectionString) { }
    }
}
