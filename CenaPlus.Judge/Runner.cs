using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace CenaPlus.Judge
{
    public class Runner
    {
        public Identity Identity = new Identity();
        public RunnerInfo RunnerInfo = new RunnerInfo();
        private string xmlresult;
        public string XMLResult { get { return xmlresult; } }
        public void Start()//synchronized
        {
            if (RunnerInfo == null || RunnerInfo.Cmd == null || RunnerInfo.TimeLimit == null || RunnerInfo.MemoryLimit == null || RunnerInfo.Cmd == null)
                throw new Exception("Missing arguments");
            Process Process = new Process();
            Process.StartInfo.FileName = "CenaPlus.Core.exe";
            Process.StartInfo.Arguments = String.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\" \"{6}\" \"{7}\" \"{8}\"", RunnerInfo.Cmd.Trim('\"'), RunnerInfo.StdInFile == String.Empty ? "NULL" : RunnerInfo.StdInFile.Trim('\"'), RunnerInfo.StdOutFile == String.Empty ? "NULL" : RunnerInfo.StdOutFile.Trim('\"'), RunnerInfo.StdErrFile == String.Empty ? "NULL" : RunnerInfo.StdErrFile.Trim('\"'), RunnerInfo.TimeLimit, RunnerInfo.MemoryLimit, RunnerInfo.HighPriorityTime, RunnerInfo.APIHook==String.Empty?"NULL":RunnerInfo.APIHook.Trim('\"'), RunnerInfo.XmlFile == String.Empty ? "NULL" : RunnerInfo.XmlFile.Trim('\"'));
            Process.StartInfo.CreateNoWindow = true;
            Process.StartInfo.UseShellExecute = false;
            Process.StartInfo.RedirectStandardOutput = true;
            if(RunnerInfo.WorkingDirectory!=String.Empty)
                Process.StartInfo.WorkingDirectory = RunnerInfo.WorkingDirectory;
            if (Identity.UserName != String.Empty)
            {
                Process.StartInfo.UserName = Identity.UserName;
                Process.StartInfo.Password = Identity.secPassword;
            }
            Process.Start();
            Process.WaitForExit(3 * RunnerInfo.TimeLimit);
            xmlresult = Process.StandardOutput.ReadToEnd();
        }
    }
    public class Identity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public  System.Security.SecureString secPassword
        {
            get 
            {
                System.Security.SecureString pwd = new System.Security.SecureString();
                foreach (char c in Password)
                {
                    pwd.AppendChar(c);
                }
                return pwd;
            }
        }
    }
    public class RunnerInfo
    {
        public string Cmd { get; set; }
        public int TimeLimit { get; set; }
        public int MemoryLimit { get; set; }
        public int HighPriorityTime { get; set; }
        public string APIHook { get; set; }
        public string StdInFile { get; set; }
        public string StdOutFile { get; set; }
        public string StdErrFile { get; set; }
        public string XmlFile { get; set; }
        public string WorkingDirectory { get; set; }
    }
}
