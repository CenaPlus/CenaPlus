using System;
using System.Collections.Generic;
using System.Text;

namespace CenaPlus.Judge
{
    public class SpecialJudge
    {
        public SpecialJudgeInfo SpecialJudgeInfo = new SpecialJudgeInfo();
        public Identity Identity = new Identity();
        private Runner Runner;
        public void Start()
        {
            Runner = new Runner();
            Runner.Identity = Identity;
            Runner.RunnerInfo.MemoryLimit = 256 * 1024;
            Runner.RunnerInfo.TimeLimit = 3000;
            Runner.RunnerInfo.HighPriorityTime = 1000;
            Runner.RunnerInfo.StdOutFile = SpecialJudgeInfo.WorkingDirectory.TrimEnd('\\') + "\\SpecialJudge_" + SpecialJudgeInfo.TestCaseNumber + ".out";
            Runner.RunnerInfo.WorkingDirectory = SpecialJudgeInfo.WorkingDirectory;
            Runner.RunnerInfo.Cmd = SpecialJudgeInfo.Cmd;
            Runner.Start();
        }
        public Entity.RecordStatus Result
        {
            get
            {
                switch (Runner.RunnerResult.ExitCode)
                { 
                    case 0:
                        return Entity.RecordStatus.Accepted;
                    case 1:
                        return Entity.RecordStatus.PresentationError;
                    case 2:
                        return Entity.RecordStatus.WrongAnswer;
                    default: 
                        return Entity.RecordStatus.ValidatorError;
                }
            }
        }
    }
    public class SpecialJudgeInfo
    {
        public string InputFile { get; set; }
        public string OutputFile { get; set; }
        public string AnswerFile { get; set; }
        public string Cmd { get; set; }
        public string WorkingDirectory { get; set; }
        public int TestCaseNumber { get; set; }
        //public string Identification { get; set; }
    }
}
