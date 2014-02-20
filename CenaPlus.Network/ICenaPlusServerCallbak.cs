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
        void JudgeFinished(Result result);

        /// <summary>
        /// Tell the client a standing item has been updated.
        /// </summary>
        /// <param name="contest_id"></param>
        /// <param name="si"></param>
        [OperationContract(IsOneWay = true)]
        void StandingsPush(int contest_id, Entity.StandingItem si);

        /// <summary>
        /// Tell the user his(her) program has been hacked.
        /// </summary>
        /// <param name="result"></param>
        [OperationContract(IsOneWay = true)]
        void BeHackedPush(HackResult result);

        /// <summary>
        /// Tell the user his(her) program has been hacked.
        /// </summary>
        /// <param name="result"></param>
        [OperationContract(IsOneWay = true)]
        void HackResultPush(HackResult result);

        [OperationContract(IsOneWay = true)]
        void NewRecord(Record record);

        [OperationContract(IsOneWay = true)]
        void NewQuestion(int question_id);

        [OperationContract(IsOneWay = true)]
        void UserLogin(int user_id);

        [OperationContract(IsOneWay = true)]
        void UserLogout(int user_id);

        [OperationContract(IsOneWay = true)]
        void NewPrint(int print_id);

        [OperationContract(IsOneWay = true)]
        void PrintDeleted(int print_id);

        [OperationContract(IsOneWay = true)]
        void PrintUpdated(int print_id);
    }
}
