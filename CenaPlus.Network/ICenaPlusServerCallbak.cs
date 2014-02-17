using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using CenaPlus.Entity;

namespace CenaPlus.Network
{
    public interface ICenaPlusServerCallback
    {
        /// <summary>
        /// Be kicked out
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void Bye();

        /// <summary>
        /// Tell the client a question has been updated.
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void QuestionUpdated(Question question);

        /// <summary>
        /// Tell the client a record has been judged.
        /// </summary>
        /// <param name="record"></param>
        [OperationContract(IsOneWay = true)]
        void JudgeFinished(Record record);
    }
}
