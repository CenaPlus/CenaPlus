using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CenaPlus.Entity
{
    public static class DetectLanguage
    {
        public static ProgrammingLanguage? PathToLanguage(string path)
        {
            var ext = System.IO.Path.GetExtension(path);
            switch (ext)
            { 
                case ".c":
                    return ProgrammingLanguage.C;
                case ".cpp":
                    return ProgrammingLanguage.CXX;
                case ".pas":
                    return ProgrammingLanguage.Pascal;
                case ".java":
                    return ProgrammingLanguage.Java;
                case ".py":
                    return ProgrammingLanguage.Python33;
                case ".rb":
                    return ProgrammingLanguage.Ruby;
                default: return null;
            }
        }
    }
}
