using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using CenaPlus.Entity;

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
        public void Start()
        {
            File.WriteAllText(CompileInfo.WorkingDirectory.TrimEnd('\\') + "\\Main" + GetExtension(CompileInfo.Language), CompileInfo.Source);
            Runner = new Runner();
            Runner.RunnerInfo.Cmd = CompileInfo.Arguments;
            Runner.RunnerInfo.HighPriorityTime = 1000;
            Runner.RunnerInfo.MemoryLimit = 64 * 1024;
            Runner.RunnerInfo.StdErrFile = CompileInfo.WorkingDirectory.TrimEnd('\\') + "\\Compile.err";
            Runner.RunnerInfo.StdOutFile = CompileInfo.WorkingDirectory.TrimEnd('\\') + "\\Compile.out";
            Runner.RunnerInfo.TimeLimit = 3000;
            Runner.RunnerInfo.WorkingDirectory = CompileInfo.WorkingDirectory;
            Runner.Identity = Identity;
            Runner.Start();
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
