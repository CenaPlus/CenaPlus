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
            public const string DefaultC = "gcc -O2 -o Main.exe -DONLINE_JUDGE -Wall -lm --static -std=c99 Main.c";
            public const string CXX = "compiler.cxx";
            public const string DefaultCXX = "g++ -O2 -o Main.exe -DONLINE_JUDGE -Wall -lm --static -std=c++98  -ansi Main.cpp";
            public const string CXX11 = "compiler.cxx";
            public const string DefaultCXX11 = "g++ -O2 -o Main.exe -DONLINE_JUDGE -Wall -lm --static -std=c++11  -ansi Main.cpp";
            public const string Javac = "compiler.javac";
            public const string DefaultJavac = "javac Main.java";
            public const string Java ="compiler.java";
            public const string DefaultJava = "java Main";
            public const string Pascal = "compiler.pascal";
            public const string DefaultPascal = "fpc -O2 -dONLINE_JUDGE Main.pas";
            public const string Python27 = "compiler.python2";
            public const string DefaultPython27 = "python2.7 Main.py";
            public const string Python33 = "compiler.python3";
            public const string DefaultPython33 = "python3.3 Main.py";
            public const string Ruby = "compiler.ruby";
            public const string DefaultRuby = "ruby Main.rb";
        }
    }
}
