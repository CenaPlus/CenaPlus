using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CenaPlus.Judge;

namespace CenaPlus.Server.Judge
{
    public class FullJudge
    {
        public Entity.Task Task;
        public void Start()
        {
            if (Task == null) throw new Exception("Task not found.");
            if (Task.Type == Entity.TaskType.Run)
            {
                string ExecuteFile = Bll.ConfigHelper.WorkingDirectory + "\\" + Task.Record.ID + "\\Main";
                switch (Task.Record.Language)
                { 
                    case Entity.ProgrammingLanguage.Java:
                        ExecuteFile += ".class";
                        break;
                    case Entity.ProgrammingLanguage.Python27:
                    case Entity.ProgrammingLanguage.Python33:
                        ExecuteFile += ".py";
                        break;
                    case Entity.ProgrammingLanguage.Ruby:
                        ExecuteFile += ".rb";
                        break;
                    default:
                        ExecuteFile += ".exe";
                        break;
                }
                if (!System.IO.File.Exists(ExecuteFile)) throw new Exception("Compiled file is not found.");
                if (System.IO.File.Exists(Bll.ConfigHelper.WorkingDirectory + "\\" + Task.TestCaseID + ".in") && System.IO.File.Exists(Bll.ConfigHelper.WorkingDirectory + "\\" + Task.TestCaseID + ".out") && System.IO.File.Exists(Bll.ConfigHelper.WorkingDirectory + "\\" + Task.TestCaseID + ".hash.in") && System.IO.File.Exists(Bll.ConfigHelper.WorkingDirectory + "\\" + Task.TestCaseID + ".hash.out"))
                {
                    var InputHash = System.IO.File.ReadAllText(Bll.ConfigHelper.WorkingDirectory + "\\" + Task.TestCaseID + ".in.hash");
                    var OutputHash = System.IO.File.ReadAllText(Bll.ConfigHelper.WorkingDirectory + "\\" + Task.TestCaseID + ".out.hash");
                    //TODO: Test case version check

                }
                else
                { 
                    //TODO: Download test cases
                }
                Runner Runner = new Runner();
                Runner.Identity.UserName = Bll.ConfigHelper.UserName;
                Runner.Identity.Password = Bll.ConfigHelper.Password;
                Runner.RunnerInfo.CenaCoreDirectory = Environment.CurrentDirectory + "\\Core\\CenaPlus.Core.exe";
                Runner.RunnerInfo.APIHook = Environment.CurrentDirectory + "\\Core\\CenaPlus.Core.Defender.dll";
                Runner.RunnerInfo.StdInFile = "..\\" + Task.TestCaseID + ".in";
                Runner.RunnerInfo.StdOutFile = Task.TestCaseID + ".user";
                Runner.RunnerInfo.TimeLimit = Task.Problem.TimeLimit;
                Runner.RunnerInfo.MemoryLimit = Convert.ToInt32(Task.Problem.MemoryLimit * 1024);
                Runner.RunnerInfo.HighPriorityTime = 1000;
                Runner.RunnerInfo.WorkingDirectory = Environment.CurrentDirectory + "\\" + Task.Record.ID;
                switch (Task.Record.Language)
                { 
                    case Entity.ProgrammingLanguage.Java:
                        Runner.RunnerInfo.Cmd = App.Server.GetConfig(Bll.ConfigKey.Compiler.Java) ?? Bll.ConfigKey.Compiler.DefaultJava; break;
                    case Entity.ProgrammingLanguage.Python27:
                        Runner.RunnerInfo.Cmd = App.Server.GetConfig(Bll.ConfigKey.Compiler.Python27) ?? Bll.ConfigKey.Compiler.DefaultPython27; break;
                    case Entity.ProgrammingLanguage.Python33:
                        Runner.RunnerInfo.Cmd = App.Server.GetConfig(Bll.ConfigKey.Compiler.Python33) ?? Bll.ConfigKey.Compiler.DefaultPython33; break;
                    case Entity.ProgrammingLanguage.Ruby:
                        Runner.RunnerInfo.Cmd = App.Server.GetConfig(Bll.ConfigKey.Compiler.Ruby) ?? Bll.ConfigKey.Compiler.DefaultRuby; break;
                    default:
                        Runner.RunnerInfo.Cmd = "Main.exe";
                        break;
                }
                Runner.Start();
                if (Runner.RunnerResult.TimeUsed > Task.Problem.TimeLimit)
                {
                    //TODO: Send the result of this case to the center server.
                    return;
                }
                var mem = Runner.RunnerResult.PagedSize;
                if (Task.Record.Language == Entity.ProgrammingLanguage.Java)
                    mem = Runner.RunnerResult.WorkingSetSize;
                if (mem > Task.Problem.MemoryLimit)
                {
                    //TODO: Send the result of this case to the center server.
                    return;
                }
                if (Task.Record.Language == Entity.ProgrammingLanguage.C && Runner.RunnerResult.ExitCode != 0 && Runner.RunnerResult.ExitCode != 1 || Runner.RunnerResult.ExitCode!=0 && Task.Record.Language!= Entity.ProgrammingLanguage.C)
                {
                    //TODO: Send the result of this case to the center server.
                    return;
                }
                //TODO: SPJ
            }
        }
    }
}