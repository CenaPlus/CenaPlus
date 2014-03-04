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
        public static event Action<int> OnJudgeFinished;
        public static event Action<int> OnBeHacked;
        public static event Action<int> OnRebuildStandings;
        public static event Action<HackResult> OnHackFinished;
        public static event Action<int, StandingItem> OnStandingPushed;
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
            try
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
                if (OnJudgeFinished != null)
                {
                    System.Threading.Tasks.Task.Factory.StartNew(() => OnJudgeFinished(result.StatusID));
                }
            }
            catch { }
        }
        private void GetStandingsProc(object cid)
        {
            Bll.StandingsCache.Standings[Convert.ToInt32(cid)] = App.Server.GetStandings(Convert.ToInt32(cid));
        }
        public void StandingsPush(int contest_id, Entity.StandingItem si)
        {
            if (Bll.StandingsCache.Standings[contest_id] == null)
            {
                Thread t = new Thread(GetStandingsProc);
                t.Start(contest_id);
            }
            else
                StandingsCache.UpdateSingleUser(contest_id, si);
            if (OnStandingPushed != null)
            {
                System.Threading.Tasks.Task.Factory.StartNew(() => OnStandingPushed(contest_id, si));
            }
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
            if (OnBeHacked != null)
            {
                System.Threading.Tasks.Task.Factory.StartNew(() => OnBeHacked(result.RecordID));
            }
        }
        public void HackResultPush(HackResult result)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                if (result.HackerUserID == App.Server.GetProfile().ID)
                {
                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        new ModernDialog
                        {
                            Title = "Your hack action has a new status.",
                            Content = new CenaPlus.Client.Remote.Contest.HackFinishedPush(result)
                        }.ShowDialog();
                    }));
                }
            });
            if (OnHackFinished != null)
            {
                System.Threading.Tasks.Task.Factory.StartNew(() => OnHackFinished(result));
            }
        }
        public void NewRecord(Entity.Record record)
        { 
        }
        public void NewPrint(int print_id)
        {
        }
        public void NewQuestion(int question_id)
        {
        }
        public void QuestionUpdated(int question_id)
        { 
        }
        public void PrintDeleted(int print_id)
        {
        }
        public void PrintUpdated(int print_id)
        {
        }
        public void UserLogin(int user_id)
        {
        }
        public void UserLogout(int user_id)
        {
        }
        public void RebuildStandings(int contest_id, List<StandingItem> standings)
        {
            Bll.StandingsCache.Standings[contest_id] = standings;
            if (OnRebuildStandings != null)
            {
                System.Threading.Tasks.Task.Factory.StartNew(() => OnRebuildStandings(contest_id));
            }
        }
    }
}
