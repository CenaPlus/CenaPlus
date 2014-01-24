using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CenaPlus.Entity
{
    public class Records
    {
        public int ID { get; set; }
        public string Username { get; set; }//For client
        public int UserID { get; set; }//For db
        public int RealTime { get; set; }
        public int UserTime { get; set; }
        public int MemoryUsed { get; set; }
        public string Problem { get; set; }//For client
        public int ProblemID { get; set; }//For db
    }
}
