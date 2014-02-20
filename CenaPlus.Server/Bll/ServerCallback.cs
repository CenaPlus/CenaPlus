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

        public void Bye()
        {

        }

        public void QuestionUpdated(Question q)
        {
        }

        public void JudgeFinished(Result result)
        {
            //TODO: Refreshing
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
    }
}
