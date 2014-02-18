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
using FirstFloor.ModernUI.Windows;

namespace CenaPlus.Client.Remote.Contest
{
    /// <summary>
    /// Interaction logic for Standings.xaml
    /// </summary>
    public partial class Standings : UserControl, IContent
    {
        private List<StandingItem> StandingItems = new List<StandingItem>();
        public void Sort()//排名变化了执行这个更新，最好排名数据是一个静态的，客户端收到了服务器推送的排名更新，后台就更新了，打开到这个页面自动就能看到，就是全部排名只加载一次，之后推送变化值。
        {
            StandingItems.Sort((x, y) => x.MainKey == y.MainKey ? x.SecKey - y.SecKey : y.MainKey - x.MainKey);
            for (int i = 0; i < StandingItems.Count; i++)
            {
                StandingItems[i].Rank = i + 1;
            }
            dgStandings.Items.Refresh();
        }
        public Standings()
        {
            InitializeComponent();
        }

        private void dgStandings_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            IList<DataGridCellInfo> selectedcells = e.AddedCells;
            if (selectedcells.Count == 1 && selectedcells[0].Column.Header.ToString().Trim(' ').Length == 1 && selectedcells[0].Column.Header.ToString().Trim(' ')[0] >= 'A' && selectedcells[0].Column.Header.ToString().Trim(' ')[0] <= 'Z')
                btnHack.IsEnabled = true;
            else
                btnHack.IsEnabled = false;
        }

        private void btnHack_Click(object sender, RoutedEventArgs e)
        {
        }

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
            dgStandings.Columns.Clear();
            var contest = App.Server.GetContest(int.Parse(e.Fragment));
            dgStandings.Columns.Add(new DataGridTextColumn() { Header = "    #", Width = 60, ElementStyle = Resources["dgCell"] as Style, Binding = new Binding("Rank") });
            dgStandings.Columns.Add(new DataGridTextColumn() { Header = "    Who", Width = 100, ElementStyle = Resources["dgCell"] as Style, Binding = new Binding("Competitor") });
            switch(contest.Type)
            {
                case ContestType.ACM:
                    dgStandings.Columns.Add(new DataGridTextColumn() { Header = "    AC", Width = 80, ElementStyle = Resources["dgCell"] as Style, Binding = new Binding("MainKey") });
                    dgStandings.Columns.Add(new DataGridTextColumn() { Header = "    Penalty", Width = 100, ElementStyle = Resources["dgCell"] as Style, Binding = new Binding("SecDisplay") });
                    break;
                case ContestType.OI:
                    dgStandings.Columns.Add(new DataGridTextColumn() { Header = "    Pts.", Width = 80, ElementStyle = Resources["dgCell"] as Style, Binding = new Binding("MainKey") });
                    dgStandings.Columns.Add(new DataGridTextColumn() { Header = "    Time", Width = 95, ElementStyle = Resources["dgCell"] as Style, Binding = new Binding("SecDisplay") });
                    break;
                case ContestType.Codeforces:
                case ContestType.TopCoder:
                    dgStandings.Columns.Add(new DataGridTextColumn() { Header = "    Pts.", Width = 80, ElementStyle = Resources["dgCell"] as Style, Binding = new Binding("MainKey") });
                    dgStandings.Columns.Add(new DataGridTextColumn() { Header = "    Hack", Width = 95, ElementStyle = Resources["dgCell"] as Style, Binding = new Binding("SecDisplay") });
                    break;
            }
            var problems = App.Server.GetProblemList(contest.ID);
            for (int i = 0; i < problems.Count; i++)
            {
                dgStandings.Columns.Add(new DataGridTextColumn() { Header = "    " + (char)(i + 'A'), Width = 80, ElementStyle = Resources["dgCell"] as Style, Binding = new Binding(String.Format("Display.[{0}]", i)) });
            }
            if (Bll.StandingsCache.Standings[contest.ID] == null)
            {
                Bll.StandingsCache.Standings[contest.ID] = App.Server.GetStandings(contest.ID);
            }
            StandingItems = Bll.StandingsCache.Standings[contest.ID] as List<Entity.StandingItem>;
            Sort();
            dgStandings.ItemsSource = StandingItems;
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
}
