using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CenaPlus.Network;
using CenaPlus.Entity;
namespace CenaPlus.Server.Bll
{
    class ServerCallback:ICenaPlusServerCallback
    {
        public void Bye()
        {

        }

        public void QuestionUpdated(Question q)
        {
        }

        public void JudgeFinished(Record record)
        { 
        }
    }
}
