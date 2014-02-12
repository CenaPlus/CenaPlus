using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CenaPlus.Judge;

namespace CenaPlus.Server.Judge
{
    public class FullJudge
    {
        public int RecordID;
        public void Start()
        {
            if (RecordID <= 0)
            {
                throw new Exception("Record not found.");
            }

        }
    }
}
