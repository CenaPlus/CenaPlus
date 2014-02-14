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
        public readonly CenaPlus.Judge.Identity Identity = new Identity() { UserName = Bll.ConfigHelper.UserName, Password = Bll.ConfigHelper.Password };

        private void Run()
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
            Runner.Identity = Identity;
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
            if (mem > Task.Problem.MemoryLimit * 1024)
            {
                //TODO: Send the result of this case to the center server.
                return;
            }
            if (Task.Record.Language == Entity.ProgrammingLanguage.C && Runner.RunnerResult.ExitCode != 0 && Runner.RunnerResult.ExitCode != 1 || Runner.RunnerResult.ExitCode != 0 && Task.Record.Language != Entity.ProgrammingLanguage.C)
            {
                //TODO: Send the result of this case to the center server.
                return;
            }
            //spj
            Runner = null;
            GC.Collect();
            Runner = new Runner();
            Runner.Identity = Identity;
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
                if (CenaPlus.Judge.Compiler.RunEXE.Contains((Entity.ProgrammingLanguage)Task.Problem.ValidatorLanguage))
                {
                    Runner.RunnerInfo.Cmd = "Main.exe";
                }
                else
                {
                    Runner.RunnerInfo.Cmd = GetCommandLine((Entity.ProgrammingLanguage)Task.Problem.ValidatorLanguage);
                }
                Runner.RunnerInfo.Cmd += String.Format(" ..\\{0} ..\\{1}\\{2} ..\\{3}",Task.TestCaseID + ".out", Task.Record.ID, Task.Record.ID + "\\" + Task.TestCaseID + ".user", Task.TestCaseID + ".in");
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
        private string GetCommandLine(Entity.ProgrammingLanguage Language)
        {
            switch (Language)
            {
                case Entity.ProgrammingLanguage.C:
                    return App.Server.GetConfig(Bll.ConfigKey.Compiler.C) ?? Bll.ConfigKey.Compiler.DefaultC;
                case Entity.ProgrammingLanguage.CXX:
                    return App.Server.GetConfig(Bll.ConfigKey.Compiler.CXX) ?? Bll.ConfigKey.Compiler.DefaultCXX;
                //TODO: Fix CE
                //case Entity.ProgrammingLanguage.CXX11:
                //    return App.Server.GetConfig(Bll.ConfigKey.Compiler.CXX11) ?? Bll.ConfigKey.Compiler.DefaultCXX11;
                case Entity.ProgrammingLanguage.Java:
                    return App.Server.GetConfig(Bll.ConfigKey.Compiler.Java) ?? Bll.ConfigKey.Compiler.DefaultJava;
                case Entity.ProgrammingLanguage.Pascal:
                    return App.Server.GetConfig(Bll.ConfigKey.Compiler.Pascal) ?? Bll.ConfigKey.Compiler.DefaultPascal;
                case Entity.ProgrammingLanguage.Python27:
                    return App.Server.GetConfig(Bll.ConfigKey.Compiler.Python27) ?? Bll.ConfigKey.Compiler.DefaultPython27;
                case Entity.ProgrammingLanguage.Python33:
                    return App.Server.GetConfig(Bll.ConfigKey.Compiler.C) ?? Bll.ConfigKey.Compiler.DefaultPython33;
                case Entity.ProgrammingLanguage.Ruby:
                    return App.Server.GetConfig(Bll.ConfigKey.Compiler.C) ?? Bll.ConfigKey.Compiler.DefaultRuby;
                default: return null;
            }
        }
        private void Compile()
        {
            CenaPlus.Judge.Compiler Compiler = new Compiler();
            Compiler.Identity = Identity;
            Compiler.CompileInfo.Language = Task.Record.Language;
            Compiler.CompileInfo.Arguments = GetCommandLine(Compiler.CompileInfo.Language);
            Compiler.CompileInfo.TimeLimit = 3000;
            Compiler.CompileInfo.WorkingDirectory = WorkDirectory + "\\" + Task.Record.ID;
            Compiler.CompileInfo.CenaCoreDirectory = Environment.CurrentDirectory + "\\Core\\CenaPlus.Core.exe";
            Compiler.CompileInfo.Source = Task.Record.Code;
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
                    Compiler.CompileInfo.Source = Task.Problem.Spj;
                    Compiler.Identity = Identity;
                    Compiler.CompileInfo.Language = (Entity.ProgrammingLanguage)Task.Problem.SpjLanguage;
                    Compiler.CompileInfo.Arguments = GetCommandLine(Compiler.CompileInfo.Language);
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
                //check the std version
                if (Task.Problem.Std != null && true)//if the std not exists or need update
                {
                    Compiler = null;
                    GC.Collect();
                    Compiler = new Compiler();
                    Compiler.CompileInfo.Source = Task.Problem.Std;
                    Compiler.Identity = Identity;
                    Compiler.CompileInfo.Language = (Entity.ProgrammingLanguage)Task.Problem.StdLanguage;
                    Compiler.CompileInfo.Arguments = GetCommandLine(Compiler.CompileInfo.Language);
                    Compiler.CompileInfo.TimeLimit = 3000;
                    Compiler.CompileInfo.WorkingDirectory = WorkDirectory + "\\std" + Task.Problem.ID;
                    Compiler.CompileInfo.CenaCoreDirectory = Environment.CurrentDirectory + "\\Core\\CenaPlus.Core.exe";
                    Compiler.Start();
                    if (Compiler.CompileResult.CompileFailed)
                    {
                        //TODO: Send the std error msg to the center server.
                        return;
                    }
                }

                //check the range validator version
                if (Task.Problem.Validator != null && true)//if the range validato not exists or need update
                {
                    Compiler = null;
                    GC.Collect();
                    Compiler = new Compiler();
                    Compiler.CompileInfo.Source = Task.Problem.Validator;
                    Compiler.Identity = Identity;
                    Compiler.CompileInfo.Language = (Entity.ProgrammingLanguage)Task.Problem.ValidatorLanguage;
                    Compiler.CompileInfo.Arguments = GetCommandLine(Compiler.CompileInfo.Language);
                    Compiler.CompileInfo.TimeLimit = 3000;
                    Compiler.CompileInfo.WorkingDirectory = WorkDirectory + "\\range" + Task.Problem.ID;
                    Compiler.CompileInfo.CenaCoreDirectory = Environment.CurrentDirectory + "\\Core\\CenaPlus.Core.exe";
                    Compiler.Start();
                    if (Compiler.CompileResult.CompileFailed)
                    {
                        //TODO: Send the range validato error msg to the center server.
                        return;
                    }
                }
            }
        }
        private void Hack()
        {
            if (System.IO.Directory.Exists(WorkDirectory + "\\hack" + Task.Hack.ID))
            {
                System.IO.Directory.CreateDirectory(WorkDirectory + "\\hack" + Task.Hack.ID);
            }
            Entity.TestCase HackData = new Entity.TestCase();
            if (Task.Hack.DatamakerLanguage != null)
            {
                CenaPlus.Judge.Compiler Compiler = new Compiler();
                Compiler.CompileInfo.Source = Task.Hack.DataOrDatamaker;
                Compiler.Identity = Identity;
                Compiler.CompileInfo.Language = (Entity.ProgrammingLanguage)Task.Hack.DatamakerLanguage;
                Compiler.CompileInfo.Arguments = GetCommandLine(Compiler.CompileInfo.Language);
                Compiler.CompileInfo.TimeLimit = 3000;
                Compiler.CompileInfo.WorkingDirectory = WorkDirectory + "\\hack" + Task.Hack.ID;
                Compiler.CompileInfo.CenaCoreDirectory = Environment.CurrentDirectory + "\\Core\\CenaPlus.Core.exe";
                Compiler.Start();
                if (Compiler.CompileResult.CompileFailed)
                {
                    //TODO: Send the data maker error to center server
                    return;
                }
                else
                {
                    CenaPlus.Judge.Runner Runner = new Runner();
                    Runner.Identity = Compiler.Identity;
                    Runner.RunnerInfo.CenaCoreDirectory = Environment.CurrentDirectory + "\\Core\\CenaPlus.Core.exe";
                    Runner.RunnerInfo.TimeLimit = Task.Problem.TimeLimit;
                    Runner.RunnerInfo.MemoryLimit = (int)(Task.Problem.MemoryLimit * 1024);
                    Runner.RunnerInfo.StdOutFile = WorkDirectory + "\\hack" + Task.Hack.ID + "\\HackData.txt";
                    Runner.RunnerInfo.WorkingDirectory = WorkDirectory + "\\hack" + Task.Hack.ID;
                    Runner.RunnerInfo.HighPriorityTime = 1000;
                    if (CenaPlus.Judge.Compiler.RunEXE.Contains(Compiler.CompileInfo.Language))
                    {
                        Runner.RunnerInfo.Cmd = "Main.exe";
                    }
                    else
                    {
                        Runner.RunnerInfo.Cmd = GetCommandLine(Compiler.CompileInfo.Language);
                    }
                    Runner.Start();
                    if (Runner.RunnerResult.ExitCode != 0 || Runner.RunnerResult.TimeUsed > Task.Problem.TimeLimit)
                    {
                        //TODO: Send the data maker error to center server
                        return;
                    }
                    else
                    {
                        HackData.Input = System.IO.File.ReadAllBytes(WorkDirectory + "\\hack" + Task.Hack.ID + "\\HackData.txt");
                    }
                }
            }
            else
            {
                HackData.Input = System.Text.Encoding.Default.GetBytes(Task.Hack.DataOrDatamaker);//TODO: Default Encode?
                System.IO.File.WriteAllBytes(WorkDirectory + "\\hack" + Task.Hack.ID + "\\HackData.txt", HackData.Output);
            }

            if (!System.IO.File.Exists(WorkDirectory + "\\" + Task.Record.ID + "\\Main" + CenaPlus.Judge.Compiler.GetExtension(Task.Record.Language)))//这里的Record是被Hack的Record
            {
                Compile();
            }
            else if (Task.Problem.Spj != null && System.IO.File.Exists(WorkDirectory + "\\spj" + Task.Problem.ID + "\\Main" + CenaPlus.Judge.Compiler.GetExtension((Entity.ProgrammingLanguage)Task.Problem.SpjLanguage)) == false)
            {
                Compile();
            }
            else if (Task.Problem.Std != null && System.IO.File.Exists(WorkDirectory + "\\std" + Task.Problem.ID + "\\Main" + CenaPlus.Judge.Compiler.GetExtension((Entity.ProgrammingLanguage)Task.Problem.StdLanguage)) == false)
            {
                Compile();
            }
            else if (Task.Problem.Std != null && System.IO.File.Exists(WorkDirectory + "\\range" + Task.Problem.ID + "\\Main" + CenaPlus.Judge.Compiler.GetExtension((Entity.ProgrammingLanguage)Task.Problem.ValidatorLanguage)) == false)
            {
                Compile();
            }

            //TODO: Hack Logic
            //data range validation
            CenaPlus.Judge.Runner runRange = new Runner();
            runRange.Identity = Identity;
            runRange.RunnerInfo.CenaCoreDirectory = Environment.CurrentDirectory + "\\Core\\CenaPlus.Core.exe";
            runRange.RunnerInfo.APIHook = Environment.CurrentDirectory + "\\Core\\CenaPlus.Core.Defender.dll";
            runRange.RunnerInfo.HighPriorityTime = 1000;
            runRange.RunnerInfo.TimeLimit = Task.Problem.TimeLimit;
            runRange.RunnerInfo.StdInFile = "..\\hack" + Task.Hack.ID + "\\HackData.txt";
            runRange.RunnerInfo.WorkingDirectory = WorkDirectory + "\\range" + Task.Problem.ID;
            if (CenaPlus.Judge.Compiler.RunEXE.Contains((Entity.ProgrammingLanguage)Task.Problem.ValidatorLanguage))
            {
                runRange.RunnerInfo.Cmd = "Main.exe";
            }
            else
            {
                runRange.RunnerInfo.Cmd = GetCommandLine((Entity.ProgrammingLanguage)Task.Problem.ValidatorLanguage);
            }
            runRange.Start();
            if (runRange.RunnerResult.ExitCode != 0)
            {
                //TODO: return data out of range to the center server.
                return;
            }
            //run std
            CenaPlus.Judge.Runner runStd = new Runner();
            runStd.Identity = Identity;
            runStd.RunnerInfo.CenaCoreDirectory = Environment.CurrentDirectory + "\\Core\\CenaPlus.Core.exe";
            runStd.RunnerInfo.APIHook = Environment.CurrentDirectory + "\\Core\\CenaPlus.Core.Defender.dll";
            runStd.RunnerInfo.HighPriorityTime = 1000;
            runStd.RunnerInfo.TimeLimit = Task.Problem.TimeLimit;
            runStd.RunnerInfo.StdInFile = "..\\hack" + Task.Hack.ID + "\\HackData.txt";
            runStd.RunnerInfo.StdOutFile = "..\\hack" + Task.Hack.ID + "\\StdOutput.txt";
            runStd.RunnerInfo.WorkingDirectory = WorkDirectory + "\\std" + Task.Problem.ID;
            if (CenaPlus.Judge.Compiler.RunEXE.Contains((Entity.ProgrammingLanguage)Task.Problem.ValidatorLanguage))
            {
                runStd.RunnerInfo.Cmd = "Main.exe";
            }
            else
            {
                runStd.RunnerInfo.Cmd = GetCommandLine((Entity.ProgrammingLanguage)Task.Problem.ValidatorLanguage);
            }
            runStd.Start();
            if (runStd.RunnerResult.ExitCode != 0 || runStd.RunnerResult.TimeUsed > Task.Problem.TimeLimit)
            {
                //TODO: return Unsuccessful Hacking Attempt to the center server.
                return;
            }
            //run hackee
            CenaPlus.Judge.Runner runHackee = new Runner();
            runHackee.Identity = Identity;
            runHackee.RunnerInfo.CenaCoreDirectory = Environment.CurrentDirectory + "\\Core\\CenaPlus.Core.exe";
            runHackee.RunnerInfo.APIHook = Environment.CurrentDirectory + "\\Core\\CenaPlus.Core.Defender.dll";
            runHackee.RunnerInfo.HighPriorityTime = 1000;
            runHackee.RunnerInfo.TimeLimit = Task.Problem.TimeLimit;
            runHackee.RunnerInfo.StdInFile = "..\\hack" + Task.Hack.ID + "\\HackData.txt";
            runHackee.RunnerInfo.StdOutFile = "..\\hack" + Task.Hack.ID + "\\HackeeOutput.txt";
            runHackee.RunnerInfo.WorkingDirectory = WorkDirectory + "\\" + Task.Record.ID;
            if (CenaPlus.Judge.Compiler.RunEXE.Contains((Entity.ProgrammingLanguage)Task.Problem.ValidatorLanguage))
            {
                runHackee.RunnerInfo.Cmd = "Main.exe";
            }
            else
            {
                runHackee.RunnerInfo.Cmd = GetCommandLine((Entity.ProgrammingLanguage)Task.Problem.ValidatorLanguage);
            }
            runHackee.Start();
            HackData.Output = System.IO.File.ReadAllBytes(WorkDirectory + "\\hack" + Task.Hack.ID + "\\StdOutput.txt");
            if (runHackee.RunnerResult.ExitCode != 0 || runHackee.RunnerResult.TimeUsed > Task.Problem.TimeLimit)
            {
                //TODO: return Sucessful Hacking Attempt to the center server.
                //HackData
                //TODO: Insert the case into database.
                return;
            }
            else
            {
                CenaPlus.Judge.Runner runSpj = new Runner();
                runSpj.Identity = Identity;
                runSpj.RunnerInfo.CenaCoreDirectory = Environment.CurrentDirectory + "\\Core\\CenaPlus.Core.exe";
                runSpj.RunnerInfo.APIHook = Environment.CurrentDirectory + "\\Core\\CenaPlus.Core.Defender.dll";
                runSpj.RunnerInfo.HighPriorityTime = 1000;
                runSpj.RunnerInfo.TimeLimit = Task.Problem.TimeLimit;
                //runSpj.RunnerInfo.StdInFile = "..\\hack" + Task.Hack.ID + "\\HackData.txt";
                //runSpj.RunnerInfo.StdOutFile = "..\\hack" + Task.Hack.ID + "\\StdOutput.txt";
                runSpj.RunnerInfo.WorkingDirectory = WorkDirectory + "\\spj" + Task.Problem.ID;
                if (CenaPlus.Judge.Compiler.RunEXE.Contains((Entity.ProgrammingLanguage)Task.Problem.ValidatorLanguage))
                {
                    runSpj.RunnerInfo.Cmd = "Main.exe";
                }
                else
                {
                    runSpj.RunnerInfo.Cmd = GetCommandLine((Entity.ProgrammingLanguage)Task.Problem.ValidatorLanguage);
                }
                runSpj.RunnerInfo.Cmd += String.Format(" {0} {1} {2}", "..\\hack" + Task.Hack.ID + "\\StdOutput.txt", "..\\hack" + Task.Hack.ID + "\\UserOutput.txt", "..\\hack" + Task.Hack.ID + "\\HackData.txt");
                runSpj.Start();
                if (runSpj.RunnerResult.ExitCode < 0 || runSpj.RunnerResult.ExitCode > 3 || runSpj.RunnerResult.TimeUsed > Task.Problem.TimeLimit)
                {
                    //TODO: return Validator Error to the center server.
                    return;
                }
                else
                {
                    if (runSpj.RunnerResult.ExitCode == 0)
                    {
                        //TODO: return unsucessful Hacking Attempt to the center server.
                        return;
                    }
                    else
                    {
                        //TODO: return Sucessful Hacking Attempt to the center server.
                        //HackData
                        //TODO: Insert the case into database.
                        return;
                    }
                }
            }
        }
        public void Start()
        {
            if (Task == null) throw new Exception("Task not found.");
            if (Task.Type == Entity.TaskType.Run)
            {
                Run();
            }
            else if (Task.Type == Entity.TaskType.Compile)
            {
                Compile();
            }
            else if (Task.Type == Entity.TaskType.Hack)
            {
                Hack();
            }
        }
    }
}