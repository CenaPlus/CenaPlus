using CenaPlus.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace CenaPlus.Network
{
    public interface IJudgeNodeCallback
    {
        [OperationContract]
        TestCase GetTestCase(int id);

        [OperationContract]
        byte[] GetInputHash(int testCaseID);

        [OperationContract]
        byte[] GetOutputHash(int testCaseID);
    }
}
