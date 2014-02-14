using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CenaPlus.Network;
using System.ServiceModel;
using CenaPlus.Entity;
using CenaPlus.Server.Judge;
using System.Threading;

namespace CenaPlus.Server.Bll
{
    [ServiceBehavior(UseSynchronizationContext = false)]
    public class JudgeNode : IJudgeNode
    {
        public static string Password { get; set; }

        private bool Authenticated { get; set; }

        public string GetVersion()
        {
            var version = typeof(IJudgeNode).Assembly.GetName().Version;
            return version.Major + "." + version.Minor;
        }

        public bool Authenticate(string password)
        {
            if (password == Password)
            {
                Authenticated = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Run(Task task)
        {
            Core freeCore = null;
            while (freeCore == null)
            {
                freeCore = Env.GetFreeCore();
                if (freeCore == null) Thread.Sleep(500);
            }

            freeCore.StartTask(task, Report);
        }

        private void Report()
        {
        }
    }
}
