using CenaPlus.Network;
using CenaPlus.Server.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CenaPlus.Server.Bll
{
    public class JudgeNodeCallback : IJudgeNodeCallback
    {
        public Entity.TestCase GetTestCase(int id)
        {
            using (DB db = new DB())
            {
                var testCase = db.TestCases.Find(id);
                if (testCase == null) return null;

                return new Entity.TestCase
                {
                    ID = testCase.ID,
                    Input = testCase.Input,
                    InputHash = testCase.InputHash,
                    Output = testCase.Output,
                    OutputHash = testCase.OutputHash,
                    ProblemID = testCase.ProblemID,
                    ProblemTitle = testCase.Problem.Title,
                    Type = testCase.Type,
                };
            }
        }

        public byte[] GetInputHash(int testCaseID)
        {
            using (DB db = new DB())
            {
                // Use linq to load only hash
                return (from t in db.TestCases
                        where t.ID == testCaseID
                        select t.InputHash).SingleOrDefault();
            }
        }

        public byte[] GetOutputHash(int testCaseID)
        {
            using (DB db = new DB())
            {
                // Use linq to load only hash
                return (from t in db.TestCases
                        where t.ID == testCaseID
                        select t.OutputHash).SingleOrDefault();
            }
        }
    }
}
