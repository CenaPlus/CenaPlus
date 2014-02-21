using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CenaPlus.Network;
using CenaPlus.Entity;
namespace CenaPlus.Server.Bll
{
    public class ServerCallback : ICenaPlusServerCallback
    {
        public event Action<Record> OnNewRecord;
        public event Action <int> OnRecordUpdated;
        public event Action<int> OnNewQuestion;
        public event Action<int> OnQuestionUpdated;
        public event Action<int> OnLogin;
        public event Action<int> OnLogout;
        public event Action<int> OnNewPrint;
        public event Action<int> OnPrintDeleted;
        public event Action<int> OnPrintUpdated;

        public void Bye()
        {

        }

        public void QuestionUpdated(Question q)
        {
            if (OnQuestionUpdated != null)
                System.Threading.Tasks.Task.Factory.StartNew(() => OnQuestionUpdated(q.ID));
        }
        public void NewQuestion(int question_id)
        {
            if (OnNewQuestion != null)
                System.Threading.Tasks.Task.Factory.StartNew(() => OnQuestionUpdated(question_id));
        }

        public void JudgeFinished(Result result)
        {
            if(OnRecordUpdated!=null)
                System.Threading.Tasks.Task.Factory.StartNew(() => OnRecordUpdated(result.StatusID));
        }

        public void StandingsPush(int contest_id, Entity.StandingItem si)
        {
        }

        public void BeHackedPush(HackResult result)
        {
        }

        public void HackResultPush(HackResult result)
        {
        }

        public void NewRecord(Record record)
        {
            if (OnNewRecord != null)
                System.Threading.Tasks.Task.Factory.StartNew(() => OnNewRecord(record));
        }
        public void UserLogin(int user_id)
        {
            if (OnLogin != null)
                System.Threading.Tasks.Task.Factory.StartNew(() => OnLogin(user_id));
        }
        public void UserLogout(int user_id)
        {
            if (OnLogout != null)
                System.Threading.Tasks.Task.Factory.StartNew(() => OnLogout(user_id));
        }
        public void NewPrint(int print_id)
        {
            if (OnNewPrint != null)
                System.Threading.Tasks.Task.Factory.StartNew(() => OnNewPrint(print_id));
        }
        public void PrintDeleted(int print_id)
        {
            if (OnPrintDeleted != null)
                System.Threading.Tasks.Task.Factory.StartNew(() => OnPrintDeleted(print_id));
        }
        public void PrintUpdated(int print_id)
        {
            if (OnPrintUpdated != null)
                System.Threading.Tasks.Task.Factory.StartNew(() => OnPrintUpdated(print_id));
        }
        public void RebuildStandings(int contest_id, List<StandingItem> standings)
        {
            Bll.StandingsCache.Standings[contest_id] = standings;
        }
    }
}
