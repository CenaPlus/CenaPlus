using CenaPlus.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace CenaPlus.Network
{
    [ServiceContract]
    public interface IJudgeNode
    {
        [OperationContract]
        string GetVersion();

        [OperationContract]
        bool Authenticate(string password);

        [OperationContract]
        void Run(Task task);
    }
}
