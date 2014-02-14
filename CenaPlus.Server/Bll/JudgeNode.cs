using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CenaPlus.Network;

namespace CenaPlus.Server.Bll
{
    public class JudgeNode : IJudgeNode
    {
        public static string Password { get; set; }

        public string GetVersion()
        {
            var version = typeof(IJudgeNode).Assembly.GetName().Version;
            return version.Major + "." + version.Minor;
        }
    }
}
