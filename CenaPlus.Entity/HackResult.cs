using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CenaPlus.Entity
{
    public class HackResult
    {
        public int HackerUserID { get; set; }
        public int DefenderUserID { get; set; }
        public string HackerUserNickName { get; set; }
        public string DefenderUserNickName { get; set; }
        public HackStatus Status { get; set; }
        public int RecordID { get; set; }
        public DateTime Time { get; set; }
        public string ProblemTitle { get; set; }
        public int HackID { get; set; }
    }
}
