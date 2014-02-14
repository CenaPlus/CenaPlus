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

        // [OperationContract]
        // TypeFoo FuncBar(TypeBar argFoo);
    }
}
