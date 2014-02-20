using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;
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
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    new ModernDialog
                    {
                        Title = "Q&A",
                        Content = new CenaPlus.Client.Remote.Contest.AnswerPush(question)
                    }.ShowDialog();
                }));
            });
        }
        public void JudgeFinished(Result result)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    new ModernDialog
                    {
                        Title = "Your program has a new status",
                        Content = new CenaPlus.Client.Remote.Contest.ResultPush(result)
                    }.ShowDialog();
                }));
            });
        }

        public void StandingsPush(int contest_id, Entity.StandingItem si)
        {
            if (Bll.StandingsCache.Standings[contest_id] == null)
            {
                System.Threading.Tasks.Task.Factory.StartNew(() => {
                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        App.Server.GetStandings(contest_id);
                    }));
                });
            }
            else
                StandingsCache.UpdateSingleUser(contest_id, si);
        }

        public void BeHackedPush(HackResult result)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    new ModernDialog
                    {
                        Title = "Your program has been hacked",
                        Content = new CenaPlus.Client.Remote.Contest.HackPush(result)
                    }.ShowDialog();
                }));
            });
        }
        public void HackResultPush(HackResult result)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    new ModernDialog
                    {
                        Title = "Your program has been hacked",
                        Content = new CenaPlus.Client.Remote.Contest.HackPush(result)
                    }.ShowDialog();
                }));
            });
        }
        public void NewRecord(Entity.Record record)
        { 
        }
    }
}
