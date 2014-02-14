using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CenaPlus.Entity
{
    public abstract class TaskFeedback
    {
    }

    public class TaskFeedback_Compile : TaskFeedback
    {
        public Entity.RecordStatus RecordStatus;
        public string CompilerOutput;//编译器输出内容
        public int RecordID;//记录ID
    }
    public class TaskFeedback_Run : TaskFeedback
    {
        public Entity.RecordStatus RecordStatus;//该测试点的结果
        public int RecordID;//记录ID
        public int TestCaseID;//该测试点ID
        public int TimeUsage;//时间消耗
        public int MemUsage;//空间消耗
    }
    public class TaskFeedback_Hack : TaskFeedback
    {
        public Entity.HackStatus HackStatus;//Hack状态
        public int HackID;//HackID
        public string CompilerOutput;
        public Entity.TestCase HackData;//如果Hack成功返回数据
    }
}
