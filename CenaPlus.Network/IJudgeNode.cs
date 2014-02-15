using CenaPlus.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace CenaPlus.Network
{
    [ServiceContract(CallbackContract = typeof(IJudgeNodeCallback))]
    public interface IJudgeNode
    {
        [OperationContract]
        string GetVersion();

        [OperationContract]
        bool Authenticate(string password);

        [OperationContract]
        int GetFreeCoreCount();

        [OperationContract]
        TaskFeedback_Compile Compile(Problem problem, Record record);

        [OperationContract]
        TaskFeedback_Hack Hack(Problem problem, Record record, Hack hack);

        [OperationContract]
        TaskFeedback_Run Run(Problem problem, Record record, int testCaseID);
    }
}
