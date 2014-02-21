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
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;

namespace CenaPlus.Client.Remote.Contest
{
    /// <summary>
    /// Interaction logic for ProblemGeneral.xaml
    /// </summary>
    public partial class ProblemGeneral : UserControl, IContent
    {
        private List<ProblemListBoxItem> ProblemListBoxItems = new List<ProblemListBoxItem>();
        private int contest_id;
        private Entity.ContestType Type;
        public ProblemGeneral()
        {
            InitializeComponent();
            Bll.ServerCallback.OnBeHacked += Refresh;
            Bll.ServerCallback.OnJudgeFinished += Refresh;
        }
        public void Refresh(int tmp)
        {
            if (Type != Entity.ContestType.Codeforces)
                btnLock.Visibility = Visibility.Collapsed;
            else
                btnLock.Visibility = Visibility.Visible;
            var list = from id in App.Server.GetProblemList(contest_id)
                       let q = App.Server.GetProblemTitle(id)
                       select new ProblemListBoxItem
                       {
                           Title = q.Title,
                           Status = q.Status,
                           Time = q.Time,
                           TimeLimit = q.TimeLimit,
                           MemoryLimit = q.MemoryLimit,
                           SpecialJudge = q.SpecialJudge,
                           ProblemID = q.ProblemID
                       };
            char i = 'A';
            ProblemListBoxItems.Clear();
            foreach (var item in list)
            {
                item.Number = i++;
                if (App.Server.GetLockStatus(item.ProblemID))
                    item.Title += " [LOCKED]";
                ProblemListBoxItems.Add(item);
            }
            ProblemListBox.Items.Refresh();
        }
        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
            contest_id = int.Parse(e.Fragment);
            var contest = App.Server.GetContest(contest_id);
            Type = contest.Type;
            ProblemListBox.ItemsSource = ProblemListBoxItems;
            Refresh(1);
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

        private void ProblemListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ProblemListBox.SelectedItem != null)
            {
                var frame = NavigationHelper.FindFrame(null, this);
                if (frame != null)
                {
                    frame.Source = new Uri("/Remote/Contest/Problem.xaml#" + (ProblemListBox.SelectedItem as ProblemListBoxItem).ProblemID, UriKind.Relative);
                }
            }
        }

        private void btnLock_Click(object sender, RoutedEventArgs e)
        {
            if (ProblemListBox.SelectedItem != null)
            {
                var item = ProblemListBox.SelectedItem as ProblemListBoxItem;
                if (item.Status.GetValueOrDefault() != Entity.ProblemGeneralStatus.Accepted)
                {
                    ModernDialog.ShowMessage("You should make your program pretest passed first.", "Error", MessageBoxButton.OK);
                }
                else
                {
                    App.Server.LockProblem(item.ProblemID);
                    ProblemListBoxItems[ProblemListBoxItems.FindIndex(x => x.ProblemID == item.ProblemID)].Title += " [LOCKED]";
                    ProblemListBox.Items.Refresh();
                    ModernDialog.ShowMessage("Problem has been locked.", "Message", MessageBoxButton.OK);
                }
            }
            else
            {
                btnLock.IsEnabled = false;
            }
        }

        private void ProblemListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProblemListBox.SelectedItem != null)
            {
                var item = ProblemListBox.SelectedItem as ProblemListBoxItem;
                if (item.Status.GetValueOrDefault() != Entity.ProblemGeneralStatus.Accepted)
                {
                    btnLock.IsEnabled = false;
                }
                else
                {
                    btnLock.IsEnabled = true;
                }
            }
            else
            {
                btnLock.IsEnabled = false;
            }
        }
    }
    public class ProblemListBoxItem : Entity.ProblemGeneral
    {
        public char Number { get; set; }
        public string Header
        {
            get
            {
                return String.Format("{0}: {1} {2}", Number, Title, Status == Entity.ProblemGeneralStatus.Hacked ? "[Hacked]" : "");
            }
        }
        public string Limits
        {
            get
            {
                return String.Format("Time limit: {0} ms / Memory limit: {1} MiB{2}", TimeLimit, MemoryLimit, SpecialJudge ? " / Special judge mode" : "");
            }
        }
        public string Details
        {
            get
            {
                if (Status == null)
                {
                    return "You have not tried this problem.";
                }
                else
                {
                    return String.Format("{0} @{1}", Status.ToString(), Time.ToShortTimeString());
                }
            }
        }
    }
}
