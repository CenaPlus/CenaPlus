using System;
using System.Diagnostics;
using System.Globalization;
using System.Management;

namespace CenaPlus.Judge
{
    public class ProcessesTreeKiller
    {
        public void FindAndKillProcess(int id)
        {
            killProcess(id);
        }

        public void FindAndKillProcess(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if ((clsProcess.ProcessName.StartsWith(name, StringComparison.CurrentCulture)) || (clsProcess.MainWindowTitle.StartsWith(name, StringComparison.CurrentCulture)))
                    killProcess(clsProcess.Id);
            }
        }

        private bool killProcess(int pid)
        {
            Process[] procs = Process.GetProcesses();
            for (int i = 0; i < procs.Length; i++)
            {
                if (getParentProcess(procs[i].Id) == pid)
                    killProcess(procs[i].Id);
            }

            try
            {
                Process myProc = Process.GetProcessById(pid);
                myProc.Kill();
            }
            // process already quited
            catch (ArgumentException)
            {
                ;
            }

            return true;
        }

        private int getParentProcess(int Id)
        {
            int parentPid = 0;
            using (ManagementObject mo = new ManagementObject("win32_process.handle='" + Id.ToString(CultureInfo.InvariantCulture) + "'"))
            {
                try
                {
                    mo.Get();
                }
                catch (ManagementException)
                {
                    return -1;
                }
                parentPid = Convert.ToInt32(mo["ParentProcessId"], CultureInfo.InvariantCulture);
            }
            return parentPid;
        }
    }
}
