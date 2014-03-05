using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CenaPlus.Judge
{
    public class Runner
    {
        public class EnvironmentVariableItem
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }
        public List<EnvironmentVariableItem> EnvironmentVariables = new List<EnvironmentVariableItem>();
        public Identity Identity = new Identity();
        public RunnerInfo RunnerInfo = new RunnerInfo();
        private string xmlresult;
        private XmlDocument xmldocument = new XmlDocument();
        public string XMLResult { get { return xmlresult; } }
        public void Start()//synchronized
        {
            if (RunnerInfo == null || RunnerInfo.Cmd == null || RunnerInfo.TimeLimit == 0 || RunnerInfo.MemoryLimit == 0 || RunnerInfo.Cmd == null)
                throw new Exception("Missing arguments");
            Process Process = new Process();
            Process.StartInfo.FileName = RunnerInfo.CenaCoreDirectory;
            Process.StartInfo.Arguments = String.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\" \"{6}\" \"{7}\" \"{8}\"", RunnerInfo.Cmd.Trim('\"'), RunnerInfo.StdInFile == null ? "NULL" : RunnerInfo.StdInFile.Trim('\"'), RunnerInfo.StdOutFile == null ? "NULL" : RunnerInfo.StdOutFile.Trim('\"'), RunnerInfo.StdErrFile == null ? "NULL" : RunnerInfo.StdErrFile.Trim('\"'), RunnerInfo.TimeLimit, RunnerInfo.MemoryLimit, RunnerInfo.HighPriorityTime, RunnerInfo.APIHook==null?"NULL":RunnerInfo.APIHook.Trim('\"'), RunnerInfo.XmlFile == null ? "NULL" : RunnerInfo.XmlFile.Trim('\"'));
            Process.StartInfo.CreateNoWindow = true;
            Process.StartInfo.UseShellExecute = false;
            Process.StartInfo.RedirectStandardOutput = true;
            foreach(var ev in EnvironmentVariables)
            {
                if (Process.StartInfo.EnvironmentVariables[ev.Key]==null)
                    Process.StartInfo.EnvironmentVariables[ev.Key] = ev.Value;
                else
                    Process.StartInfo.EnvironmentVariables[ev.Key]+=";"+ev.Value;
            }
            if (RunnerInfo.WorkingDirectory != null)
                Process.StartInfo.WorkingDirectory = RunnerInfo.WorkingDirectory;
            if (Process.StartInfo.EnvironmentVariables["Path"] == null)
                Process.StartInfo.EnvironmentVariables["Path"] = RunnerInfo.WorkingDirectory;
            else
                Process.StartInfo.EnvironmentVariables["Path"] += ";" + RunnerInfo.WorkingDirectory;
            if (Identity.UserName != null)
            {
                Process.StartInfo.UserName = Identity.UserName;
                Process.StartInfo.Password = Identity.secPassword;
            }
            Process.Start();
            Process.WaitForExit(2 * RunnerInfo.TimeLimit + 1000);
            try
            {
                if (!Process.HasExited) RunnerInfo.KillProcessTree = true;
                Process.Kill();
                if (RunnerInfo.KillProcessTree)
                {
                    try
                    {
                        ProcessesTreeKiller ptk = new ProcessesTreeKiller();
                        ptk.FindAndKillProcess(Process.Id);
                    }
                    catch
                    {
                    }
                }
                else
                {
                    Process.Kill();
                }

            }
            catch { }
            xmlresult = Process.StandardOutput.ReadToEnd();
        }
        public RunnerResult RunnerResult
        {
            get
            {
                RunnerResult RunnerResult = new Judge.RunnerResult();
                if (xmlresult != null && xmlresult != String.Empty)
                {
                    xmldocument.LoadXml(xmlresult);
                    XmlNodeList xmlnodelist;
                    xmlnodelist = xmldocument.GetElementsByTagName("ExitCode");
                    RunnerResult.ExitCode = Convert.ToInt32(xmlnodelist[0].InnerText);
                    xmlnodelist = xmldocument.GetElementsByTagName("TimeUsed");
                    RunnerResult.TimeUsed = Convert.ToInt32(xmlnodelist[0].InnerText);
                    xmlnodelist = xmldocument.GetElementsByTagName("PagedSize");
                    RunnerResult.PagedSize = Convert.ToInt32(xmlnodelist[0].InnerText);
                    xmlnodelist = xmldocument.GetElementsByTagName("WorkingSet");
                    RunnerResult.WorkingSetSize = Convert.ToInt32(xmlnodelist[0].InnerText);
                }
                else
                {
                    RunnerResult.ExitCode = -1;
                    RunnerResult.TimeUsed = RunnerInfo.TimeLimit + 1;
                }
                return RunnerResult;
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
        public string CenaCoreDirectory = "CenaPlus.Core.exe";
        public bool KillProcessTree = false;
    }
    public class RunnerResult
    {
        public int TimeUsed { get; set; }
        public int PagedSize { get; set; }
        public int ExitCode = -1;
        public int WorkingSetSize { get; set; }
    }
}
