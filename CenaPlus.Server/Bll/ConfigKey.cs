using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CenaPlus.Server.Bll
{
    static class ConfigKey
    {
        public const string Circular = "circular";
        public const string DefaultCircular = "It works!";
        public static class Compiler
        {
            public const string C = "compiler.c";
            public const string DefaultC = "gcc -O2 -o {Binding Filename}.exe -DONLINE_JUDGE -Wall -lm --static -std=c99 {Binding Filename}.c";
            public const string CXX = "compiler.cxx";
            public const string DefaultCXX = "g++ -O2 -o {Binding Filename}.exe -DONLINE_JUDGE -Wall -lm --static -ansi {Binding Filename}.cpp";
            public const string Javac = "compiler.javac";
            public const string DefaultJavac = "javac Main.java";
            public const string Java ="compiler.java";
            public const string DefaultJava = "java Main";
            public const string Pascal = "compiler.pascal";
            public const string DefaultPascal = "fpc -O2 -dONLINE_JUDGE {Binding Filename}.pas";
            public const string Python27 = "compiler.python2";
            public const string DefaultPython27 = "python2.7 {Binding Filename}.py";
            public const string Python33 = "compiler.python3";
            public const string DefaultPython33 = "python3.3 {Binding Filename}.py";
            public const string Ruby = "compiler.ruby";
            public const string DefaultRuby = "ruby {Binding Filename}.rb";
        }
    }
}
