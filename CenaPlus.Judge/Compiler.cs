using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using CenaPlus.Entity;
using System.Linq;

namespace CenaPlus.Judge
{
    public class Compiler
    {
        public CompileInfo CompileInfo = new CompileInfo();
        public Identity Identity = new Identity();
        private Runner Runner;
        public static string GetExtension(ProgrammingLanguage Language)
        {
            switch (Language)
            { 
                case ProgrammingLanguage.C:
                    return ".c";
                case ProgrammingLanguage.CXX:
                    return ".cpp";
                case ProgrammingLanguage.CXX11:
                    return ".cpp";
                case ProgrammingLanguage.Java:
                    return ".java";
                case ProgrammingLanguage.Pascal:
                    return ".pas";
                case ProgrammingLanguage.Python27:
                    return ".py";
                case ProgrammingLanguage.Python33:
                    return ".py";
                case ProgrammingLanguage.Ruby:
                    return ".rb";
                default:
                    return null;
            }
        }
        public static readonly Entity.ProgrammingLanguage[] NeedCompile = 
        { 
            Entity.ProgrammingLanguage.C,
            Entity.ProgrammingLanguage.CXX,
            Entity.ProgrammingLanguage.CXX11,
            Entity.ProgrammingLanguage.Java,
            Entity.ProgrammingLanguage.Pascal
        };
        public static readonly Entity.ProgrammingLanguage[] RunEXE = 
        { 
            Entity.ProgrammingLanguage.C,
            Entity.ProgrammingLanguage.CXX,
            Entity.ProgrammingLanguage.CXX11,
            Entity.ProgrammingLanguage.Pascal
        };
        public void Start()
        {
            if (!System.IO.Directory.Exists(CompileInfo.WorkingDirectory))
            {
                System.IO.Directory.CreateDirectory(CompileInfo.WorkingDirectory);
            }
            File.WriteAllText(CompileInfo.WorkingDirectory.TrimEnd('\\') + "\\Main" + GetExtension(CompileInfo.Language), CompileInfo.Source);
            if (CenaPlus.Judge.Compiler.NeedCompile.Contains(CompileInfo.Language))
            {
                Runner = new Runner();
                Runner.RunnerInfo.CenaCoreDirectory = CompileInfo.CenaCoreDirectory;
                Runner.RunnerInfo.Cmd = CompileInfo.Arguments;
                Runner.RunnerInfo.HighPriorityTime = 1000;
                Runner.RunnerInfo.MemoryLimit = 64 * 1024;
                Runner.RunnerInfo.StdErrFile = CompileInfo.WorkingDirectory.TrimEnd('\\') + "\\Compile.err";
                Runner.RunnerInfo.StdOutFile = CompileInfo.WorkingDirectory.TrimEnd('\\') + "\\Compile.out";
                Runner.RunnerInfo.TimeLimit = 3000;
                Runner.RunnerInfo.WorkingDirectory = CompileInfo.WorkingDirectory;
                Runner.Identity = Identity;
                Runner.RunnerInfo.KillProcessTree = true;
                Runner.Start();
            }

        }
        public CompileResult CompileResult
        {
            get 
            {
                CompileResult CompileResult = new CompileResult();
                if (Runner.RunnerResult.ExitCode != 0)
                    CompileResult.CompileFailed = true;
                CompileResult.CompilerOutput = File.ReadAllText(Runner.RunnerInfo.StdErrFile) + File.ReadAllText(Runner.RunnerInfo.StdOutFile);
                if (Runner.RunnerResult.TimeUsed > Runner.RunnerInfo.TimeLimit)
                {
                    CompileResult.CompileFailed = true;
                    CompileResult.CompilerOutput = "Cena+: Compiler time limit exceeded.";
                }
                return CompileResult;
            }
        }
    }
    public class CompileInfo
    {
        public string Source { get; set; }
        public Entity.ProgrammingLanguage Language;
        public string Arguments { get; set; }
        public string WorkingDirectory { get; set; }
        public string CenaCoreDirectory = "CenaPlus.Core.exe";
        public int TimeLimit { get; set; }
        //public string Identification { get; set; }
    }
    public class CompileResult
    {
        public int TimeUsed { get; set; }
        public int MemoryUsed { get; set; }
        public string CompilerOutput { get; set; }
        public bool CompileFailed { get; set; }
    }
}
