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

namespace CenaPlus.Server.ServerMode
{
    /// <summary>
    /// Interaction logic for Tests.xaml
    /// </summary>
    public partial class Tests : UserControl, IContent
    {
        private List<ContestListItem> contestList = new List<ContestListItem>();

        public Tests()
        {
            InitializeComponent();
            ContestListBox.ItemsSource = contestList;
        }

        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            var frame = NavigationHelper.FindFrame(null, this);
            if (frame != null)
            {
                frame.Source = new Uri("/ServerMode/Contest/Contest.xaml#" + ContestListBox.SelectedValue, UriKind.Relative);
            }
        }

        private void ContestListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ContestListBox.SelectedIndex != -1)
            {
                var frame = NavigationHelper.FindFrame(null, this);
                if (frame != null)
                {
                    frame.Source = new Uri("/ServerMode/Contest/Contest.xaml#" + ContestListBox.SelectedValue, UriKind.Relative);
                }
            }
        }

        private void ContestListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ContestListBox.SelectedItem != null)
            {
                btnModify.IsEnabled = true;
                btnDelete.IsEnabled = true;
            }
            else
            {
                btnModify.IsEnabled = false;
                btnDelete.IsEnabled = false;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)ContestListBox.SelectedValue;
            App.Server.DeleteContest(id);
            contestList.RemoveAt(ContestListBox.SelectedIndex);
            ContestListBox.Items.Refresh();
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
        }

        public void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            var list = from id in App.Server.GetContestList()
                       let c = App.Server.GetContest(id)
                       select new ContestListItem
                       {
                           ID = c.ID,
                           StartTime = c.StartTime,
                           EndTime = c.EndTime,
                           Title = c.Title,
                           Type = c.Type
                       };
            contestList.Clear();
            foreach (var item in list) contestList.Add(item);
            ContestListBox.Items.Refresh();
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
        }

        class ContestListItem : CenaPlus.Entity.Contest
        {
            private const string DetailTemplate = "{0} UTC / {1} hrs / {2} Format";
            public string Detail
            {
                get
                {
                    return String.Format(DetailTemplate, StartTime, (Duration.TotalSeconds / 60 / 60).ToString("0.0"), Type);
                }
            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            int id = App.Server.CreateContest("New Contest", "Insert description Here.", DateTime.Now.AddHours(12), DateTime.Now.AddHours(14), Entity.ContestType.OI, false);
            var frame = NavigationHelper.FindFrame(null, this);
            if (frame != null)
            {
                frame.Source = new Uri("/ServerMode/Contest/Contest.xaml#" + id, UriKind.Relative);
            }
        }


    }
}
