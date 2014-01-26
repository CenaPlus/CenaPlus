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
using FirstFloor.ModernUI.Windows.Controls;
using CenaPlus.Entity;
using CenaPlus.Client.Bll;
namespace CenaPlus.Client.Remote
{
    /// <summary>
    /// Interaction logic for Tests.xaml
    /// </summary>
    public partial class Tests : UserControl,IContent
    {
        public Tests()
        {
            InitializeComponent();
        }

        private void ContestListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var frame = NavigationHelper.FindFrame(null, this);
            if (frame != null)
            {
                frame.Source = new Uri("/Remote/Contest/Contest.xaml#" + ContestListBox.SelectedValue, UriKind.Relative);
            }
            ContestListBox.SelectedIndex = -1;
        }

        public class ContestList : CenaPlus.Entity.Contest
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
                       select new ContestList
                       {
                           ID = c.ID,
                           Title = c.Title,
                           StartTime = c.StartTime,
                           EndTime = c.EndTime,
                           Type = c.Type
                       };
            ContestListBox.ItemsSource = list;
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
        }
    }
}
