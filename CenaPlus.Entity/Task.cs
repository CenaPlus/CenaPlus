using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CenaPlus.Entity
{
    public class Task
    {
        public Record Record { get; set; }
        public TaskType Type { get; set; }
        public int? TestCaseID { get; set; }
        public Problem Problem { get; set; }
    }
    public enum TaskType {Compile, Run};
}
