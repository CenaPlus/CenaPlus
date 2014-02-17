using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using CenaPlus.Network;
using CenaPlus.Entity;
namespace CenaPlus.Client.Bll
{
    class ServerCallback : ICenaPlusServerCallback
    {
        public void Bye()
        {
            ModernDialog.ShowMessage("You are kicked out.", "Message", MessageBoxButton.OK);
            Environment.Exit(1);
        }

        public void QuestionUpdated(Question question)
        {
            new ModernDialog
            {
                Title = "Q&A",
                Content = new CenaPlus.Client.Remote.Contest.AnswerPush(question)
            }.ShowDialog();
        }

        public void JudgeFinished(Record record)
        {
            new ModernDialog
            {
                Title = "Your program has a new status",
                Content = new CenaPlus.Client.Remote.Contest.ResultPush(record)
            }.ShowDialog();
        }
    }
}
