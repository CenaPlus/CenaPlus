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
        /// Pop an advertisement.
        /// </summary>
        /// <param name="ad"></param>
        [OperationContract(IsOneWay = true)]
        void PopAd(string ad);
    }
}
