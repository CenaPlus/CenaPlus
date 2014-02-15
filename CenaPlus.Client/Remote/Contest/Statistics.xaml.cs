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
using System.Windows.Shapes;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using CenaPlus.Entity;

namespace CenaPlus.Client.Remote.Contest
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Statistics : UserControl,IContent
    {
        private List<StatisticsListItem> statisticsList = new List<StatisticsListItem>();

        public Statistics()
        {
            InitializeComponent();
            StatisticsListView.ItemsSource = statisticsList;
        }
        
        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            int contestID = int.Parse(e.Fragment);
            var list = from id in App.Server.GetProblemList(contestID)
                       let s = App.Server.GetProblemStatistics(id)
                       select new StatisticsListItem
                       {
                           AC = s.AC,
                           CE = s.CE,
                           MLE = s.MLE,
                           ProblemTitle = s.ProblemTitle,
                           RE = s.RE,
                           SE = s.SE,
                           TLE = s.TLE,
                           VE = s.VE,
                           WA = s.WA
                       };
            statisticsList.Clear();
            statisticsList.AddRange(list);
            StatisticsListView.Items.Refresh();
        }

        public void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
        }

        class StatisticsListItem : ProblemStatistics
        {
        }

    }
    
}
