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
using FirstFloor.ModernUI.Windows;

namespace CenaPlus.Client.Remote.Contest
{
    /// <summary>
    /// Interaction logic for HackList.xaml
    /// </summary>
    public partial class HackList : UserControl, IContent
    {
        private List<HackListBoxItem> HackListBoxItems = new List<HackListBoxItem>();
        private int contest_id;
        public HackList()
        {
            InitializeComponent();
            lstHack.ItemsSource = HackListBoxItems;
            Bll.ServerCallback.OnHackFinished += this.Refresh;
        }
        public void Refresh(Entity.HackResult result)
        {
            var hackindex = HackListBoxItems.FindIndex(x => x.HackID == result.HackID);
            var item = new HackListBoxItem()
            {
                ProblemTitle = result.ProblemTitle,
                HackerUserNickName = result.HackerUserNickName,
                DefenderUserNickName = result.DefenderUserNickName,
                HackID = result.HackID,
                RecordID = result.RecordID,
                Status = result.Status,
                Time = result.Time
            };
            Dispatcher.Invoke(new Action(() =>
            {
                if (hackindex == -1)
                {
                    HackListBoxItems.Add(item);
                }
                else
                {
                    HackListBoxItems[hackindex] = item;
                }
                lstHack.Items.Refresh();
            }));
        }
        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
            contest_id = int.Parse(e.Fragment);
            var ids = App.Server.GetHackList(contest_id);
            HackListBoxItems.Clear();
            foreach (var hid in ids)
            {
                var hack = App.Server.GetHackGeneral(hid);
                var item = new HackListBoxItem()
                {
                    ProblemTitle = hack.ProblemTitle,
                    HackerUserNickName = hack.HackerUserNickName,
                    DefenderUserNickName = hack.DefenderUserNickName,
                    HackID = hack.HackID,
                    RecordID = hack.RecordID,
                    Status = hack.Status,
                    Time = hack.Time
                };
                HackListBoxItems.Add(item);
            }
            lstHack.Items.Refresh();
        }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
        }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {
        }
    }
    public class HackListBoxItem: Entity.HackResult
    {
        public string Title
        {
            get 
            {
                return String.Format("{0} -> {1}", HackerUserNickName, DefenderUserNickName);
            }
        }
        public string Details
        {
            get
            {
                return String.Format("{0} @{1}", Status.ToString(), Time);
            }
        }
        public string Color
        {
            get
            {
                switch (Status)
                { 
                    case Entity.HackStatus.BadData:
                    case Entity.HackStatus.DatamakerError:
                        return "Orange";
                    case Entity.HackStatus.Failure:
                        return "Red";
                    case Entity.HackStatus.Success:
                        return "Green";
                    default:
                        return "SkyBlue";
                }
            }
        }
    }
}
