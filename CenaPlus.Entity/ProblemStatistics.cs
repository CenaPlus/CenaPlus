using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CenaPlus.Entity
{
    public class ProblemStatistics
    {
        public string ProblemTitle { get; set; }
        public int AC { get; set; }
        public int CE { get; set; }
        public int SE { get; set; }
        public int VE { get; set; }
        public int WA { get; set; }
        public int TLE { get; set; }
        public int MLE { get; set; }
        public int RE { get; set; }
    }
}
