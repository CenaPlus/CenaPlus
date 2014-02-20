using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CenaPlus.Entity;

namespace CenaPlus.Client.Remote.Contest
{
    /// <summary>
    /// Interaction logic for HackFinishedPush.xaml
    /// </summary>
    public partial class HackFinishedPush : UserControl
    {
        public HackFinishedPush(Entity.HackResult result)
        {
            InitializeComponent();
            tbHackID.Text = result.HackID.ToString();
            tbHacker.Text = result.HackerUserNickName;
            tbDefender.Text = result.DefenderUserNickName;
            tbProblem.Text = result.ProblemTitle;
            tbStatus.Text = result.Status.ToString();
            tbTime.Text = result.Time.ToString();
            switch (result.Status)
            {
                case HackStatus.BadData:
                case HackStatus.DatamakerError:
                    tbStatus.Foreground = new SolidColorBrush(Colors.Orange);
                    break;
                case HackStatus.Failure:
                    tbStatus.Foreground = new SolidColorBrush(Colors.Red);
                    break;
                case HackStatus.Success:
                    tbStatus.Foreground = new SolidColorBrush(Colors.Green);
                    break;
                default:
                    tbStatus.Foreground = new SolidColorBrush(Colors.SkyBlue);
                    break;
            }
        }
    }
}
