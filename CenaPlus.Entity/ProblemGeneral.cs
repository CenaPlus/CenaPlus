using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CenaPlus.Entity
{
    public class ProblemGeneral
    {
        public string Title { get; set; }
        public ProblemGeneralStatus? Status { get; set; }
        public DateTime Time { get; set; }
        public int TimeLimit { get; set; }
        public int MemoryLimit { get; set; }
        public int ProblemID { get; set; }
        public bool SpecialJudge { get; set; }
        public int? Points { get; set; }
    }
    public enum ProblemGeneralStatus {Pending, Hacked, Accepted, Submitted};
}
