using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CenaPlus.Entity
{
    public class Contests
    {
        public static string[] FormatStr = { "OI", "ACM", "Codeforces", "TopCoder" };
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public int Length 
        {
            get { return (int)(End - Begin).TotalSeconds; }
        }
        public string Host { get; set; }
        public int Format { get; set; }//0-OI, 1-ACM, 2-Codeforces, 3-TopCoder
        public int Rating { get; set; }
    }
}
