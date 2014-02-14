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
            //contestID
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
