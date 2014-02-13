using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CenaPlus.Client.Local
{
    public static class Static
    {
        public static string SourceFileDirectory;
        public static string WorkingDirectory;
        public static int TimeLimit;
        public static int MemoryLimit;
        public static string FileName;
        public static string Extension;
        public static string SPJDirectory;
        public static Entity.ProgrammingLanguage Language;
        public static List<TestCase> TestCases;
    }
    public class TestCase
    {
        public int Index { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public string Title
        {
            get
            {
                return String.Format("Test case: #" + Index);
            }
        }
        public string Details
        {
            get
            {
                return String.Format(System.IO.Path.GetFileName(Input) + " / " + System.IO.Path.GetFileName(Output));
            }
        }
    }
}
