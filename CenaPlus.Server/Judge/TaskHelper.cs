using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CenaPlus.Judge;

namespace CenaPlus.Server.Judge
{
    public class TaskHelper
    {
        public Entity.Task Task;
        public string spjOutput;
        public readonly string WorkDirectory = Bll.ConfigHelper.WorkingDirectory;
        public void Start()
        {
            if (Task == null) throw new Exception("Task not found.");
            if (Task.Type == Entity.TaskType.Run)
            {
                string ExecuteFile = WorkDirectory + "\\" + Task.Record.ID + "\\Main";
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
                if (System.IO.File.Exists(WorkDirectory + "\\" + Task.TestCaseID + ".in") && System.IO.File.Exists(WorkDirectory + "\\" + Task.TestCaseID + ".out") && System.IO.File.Exists(WorkDirectory + "\\" + Task.TestCaseID + ".hash.in") && System.IO.File.Exists(WorkDirectory + "\\" + Task.TestCaseID + ".hash.out"))
                {
                    var InputHash = System.IO.File.ReadAllText(WorkDirectory + "\\" + Task.TestCaseID + ".in.hash");
                    var OutputHash = System.IO.File.ReadAllText(WorkDirectory + "\\" + Task.TestCaseID + ".out.hash");
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
                        Runner.RunnerInfo.APIHook = Environment.CurrentDirectory + "\\Core\\CenaPlus.Core.Defender.dll";
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
                if (Task.Record.Language == Entity.ProgrammingLanguage.C && Runner.RunnerResult.ExitCode != 0 && Runner.RunnerResult.ExitCode != 1 || Runner.RunnerResult.ExitCode != 0 && Task.Record.Language != Entity.ProgrammingLanguage.C)
                {
                    //TODO: Send the result of this case to the center server.
                    return;
                }
                //TODO: SPJ
                Runner = null;
                GC.Collect();
                Runner = new Runner();
                Runner.Identity.UserName = Bll.ConfigHelper.UserName;
                Runner.Identity.Password = Bll.ConfigHelper.Password;
                Runner.RunnerInfo.CenaCoreDirectory = Environment.CurrentDirectory + "\\Core\\CenaPlus.Core.exe";
                Runner.RunnerInfo.TimeLimit = 2000;
                Runner.RunnerInfo.MemoryLimit = 128 * 1024;
                if (Task.Problem.Spj == null)
                {
                    Runner.RunnerInfo.WorkingDirectory = WorkDirectory;
                    if (!System.IO.File.Exists(WorkDirectory + "\\spj.exe"))
                    {
                        System.IO.File.Copy(Environment.CurrentDirectory + "\\Core\\CenaPlus.Core.Standard.exe", WorkDirectory + "\\spj.exe", true);
                    }
                    Runner.RunnerInfo.Cmd = String.Format("spj.exe {0} {1} {2}", Task.TestCaseID + ".out", Task.Record.ID + "\\" + Task.TestCaseID + ".user", Task.TestCaseID + ".in");
                    Runner.RunnerInfo.StdOutFile = Task.Record.ID + "\\" + Task.TestCaseID + ".validator";
                }
                else
                {
                    var CustomSPJ = WorkDirectory + "\\spj" + Task.Problem.ID + "\\Main";
                    switch (Task.Record.Language)
                    {
                        case Entity.ProgrammingLanguage.Java:
                            CustomSPJ += ".class";
                            break;
                        case Entity.ProgrammingLanguage.Python27:
                        case Entity.ProgrammingLanguage.Python33:
                            CustomSPJ += ".py";
                            break;
                        case Entity.ProgrammingLanguage.Ruby:
                            CustomSPJ += ".rb";
                            break;
                        default:
                            CustomSPJ += ".exe";
                            break;
                    }
                    if (!System.IO.File.Exists(CustomSPJ))
                    {
                        throw new Exception("Custom spj not found.");
                    }
                    Runner.RunnerInfo.WorkingDirectory = WorkDirectory + "\\spj" + Task.Problem.ID;
                    Runner.RunnerInfo.StdOutFile = "..\\" + Task.TestCaseID + ".validator";
                    Runner.RunnerInfo.Cmd = String.Format("{0} ..\\{1} ..\\{2}\\{3} ..\\{4}", System.IO.Path.GetFileName(CustomSPJ), Task.TestCaseID + ".out", Task.Record.ID, Task.Record.ID + "\\" + Task.TestCaseID + ".user", Task.TestCaseID + ".in");
                }
                Runner.Start();
                spjOutput = WorkDirectory + "\\" + Task.Record.ID + "\\" + Task.TestCaseID + ".validator";
                if (Runner.RunnerResult.ExitCode != 0 && Runner.RunnerResult.ExitCode != 1 && Runner.RunnerResult.ExitCode != 2 || Runner.RunnerResult.TimeUsed > 2000)
                {
                    //TODO: Send the result of this case to the center server.(validator error)
                    return;
                }
                else
                {
                    var Result = (Entity.RecordStatus)Runner.RunnerResult.ExitCode;
                    //TODO: Send the result of this case to the center server.
                    return;
                }
            }
            else if (Task.Type == Entity.TaskType.Compile)
            {
                CenaPlus.Judge.Compiler Compiler = new Compiler();
                Compiler.Identity.UserName = Bll.ConfigHelper.UserName;
                Compiler.Identity.Password = Bll.ConfigHelper.Password;
                Compiler.CompileInfo.Language = Task.Record.Language;
                Compiler.CompileInfo.TimeLimit = 3000;
                Compiler.CompileInfo.WorkingDirectory = WorkDirectory + "\\" + Task.Record.ID;
                Compiler.CompileInfo.CenaCoreDirectory = Environment.CurrentDirectory + "\\Core\\CenaPlus.Core.exe";
                Compiler.CompileInfo.Source = WorkDirectory + "\\" + Task.Record.ID + "\\Main";
                switch (Task.Record.Language)
                {
                    case Entity.ProgrammingLanguage.C:
                        Compiler.CompileInfo.Source += ".c";
                        break;
                    case Entity.ProgrammingLanguage.CXX:
                    case Entity.ProgrammingLanguage.CXX11:
                        Compiler.CompileInfo.Source += ".cpp";
                        break;
                    case Entity.ProgrammingLanguage.Pascal:
                        Compiler.CompileInfo.Source += ".pas";
                        break;
                    case Entity.ProgrammingLanguage.Java:
                        Compiler.CompileInfo.Source += ".java";
                        break;
                    case Entity.ProgrammingLanguage.Python27:
                    case Entity.ProgrammingLanguage.Python33:
                        Compiler.CompileInfo.Source += ".py";
                        break;
                    case Entity.ProgrammingLanguage.Ruby:
                        Compiler.CompileInfo.Source += ".rb";
                        break;
                }
                Compiler.Start();
                if (Compiler.CompileResult.CompileFailed)
                {
                    //TODO: Send the compile failed msg to the center server
                    //Compiler.CompileResult.CompilerOutput
                }
                else
                {
                    //TODO: Send the compile success msg to the center server
                    //check the spj version
                    if (Task.Problem.Spj != null && true)//if the spj not exists or need update
                    {
                        Compiler = null;
                        GC.Collect();
                        Compiler = new Compiler();
                        Compiler.CompileInfo.Source = WorkDirectory + "\\spj" + Task.Problem.ID + "\\Main";
                        switch (Task.Record.Language)
                        {
                            case Entity.ProgrammingLanguage.C:
                                Compiler.CompileInfo.Source += ".c";
                                break;
                            case Entity.ProgrammingLanguage.CXX:
                            case Entity.ProgrammingLanguage.CXX11:
                                Compiler.CompileInfo.Source += ".cpp";
                                break;
                            case Entity.ProgrammingLanguage.Pascal:
                                Compiler.CompileInfo.Source += ".pas";
                                break;
                            case Entity.ProgrammingLanguage.Java:
                                Compiler.CompileInfo.Source += ".java";
                                break;
                            case Entity.ProgrammingLanguage.Python27:
                            case Entity.ProgrammingLanguage.Python33:
                                Compiler.CompileInfo.Source += ".py";
                                break;
                            case Entity.ProgrammingLanguage.Ruby:
                                Compiler.CompileInfo.Source += ".rb";
                                break;
                        }
                        Compiler.Identity.UserName = Bll.ConfigHelper.UserName;
                        Compiler.Identity.Password = Bll.ConfigHelper.Password;
                        Compiler.CompileInfo.Language = Task.Record.Language;
                        Compiler.CompileInfo.TimeLimit = 3000;
                        Compiler.CompileInfo.WorkingDirectory = WorkDirectory + "\\spj" + Task.Problem.ID;
                        Compiler.CompileInfo.CenaCoreDirectory = Environment.CurrentDirectory + "\\Core\\CenaPlus.Core.exe";
                        Compiler.Start();
                        if (Compiler.CompileResult.CompileFailed)
                        { 
                            //TODO: Send the validator error msg to the center server.
                            return;
                        }
                    }
                }
            }
        }
    }
}