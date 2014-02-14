using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace CenaPlus.Server.Judge
{
    public static class Env
    {
        public static List<Core> Cores = new List<Core>();
    }
    public class Core
    {
        private void ExecuteTask()
        {
            TaskHelper th = new TaskHelper();
            th.Task = currenttask;
            th.Start();
            currenttask = null;
        }
        private Entity.Task currenttask;
        public Entity.Task CurrentTask 
        {
            get
            {
                return currenttask;
            }
            set
            {
                if (currenttask == null)
                    currenttask = value;
                else
                    throw new Exception("The core is working.");
                Thread t = new Thread(ExecuteTask);
                t.Start();
            }
        }
        public CoreStatus Status
        { 
            get 
            {
                if (CurrentTask == null)
                    return CoreStatus.Free;
                else return CoreStatus.Working;
            }
        }
        public int Index { get; set; }
        //display only
        public string Title
        {
            get 
            {
                return String.Format("Core #{0}", Index);
            }
        }
        public string StatusStr 
        {
            get
            {
                return "Status: "+Status.ToString();
            }
        }
        public string TaskType
        {
            get
            {
                if (CurrentTask == null) return "Task: N/A";
                else
                {
                    switch (CurrentTask.Type)
                    { 
                        case Entity.TaskType.Compile:
                            return "Task: Compile R" + CurrentTask.Record.ID;
                        case Entity.TaskType.Run:
                            return "Task: Run R" + CurrentTask.Record.ID;
                        case Entity.TaskType.Hack:
                            return "Task: Hack R" + CurrentTask.Hack.ID;
                        default: 
                            return "Task: N/A";
                    }
                }
            }
        }
    }
    public enum CoreStatus { Free, Working };
}
