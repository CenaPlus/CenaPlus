using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
namespace CenaPlus.Network
{
    public interface ICenaPlusServerCallback
    {
        /// <summary>
        /// Be kicked out
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void Bye();
    }
}
