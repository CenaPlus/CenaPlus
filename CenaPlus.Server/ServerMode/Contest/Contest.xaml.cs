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
using FirstFloor.ModernUI.Presentation;

namespace CenaPlus.Server.ServerMode.Contest
{
    /// <summary>
    /// Interaction logic for Contest.xaml
    /// </summary>
    public partial class Contest : UserControl, IContent
    {
        public Contest()
        {
            InitializeComponent();
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            int id = int.Parse(e.Fragment);
            lstLinks.Links.Clear();
            lstLinks.Links.Add(new Link { DisplayName = "General", Source = new Uri("/ServerMode/Contest/General.xaml#" + id, UriKind.Relative) });
            lstLinks.Links.Add(new Link { DisplayName = "Description", Source = new Uri("/ServerMode/Contest/Description.xaml#" + id, UriKind.Relative) });
            lstLinks.Links.Add(new Link { DisplayName = "Problems", Source = new Uri("/ServerMode/Contest/ProblemList.xaml#" + id, UriKind.Relative) });
            lstLinks.Links.Add(new Link { DisplayName = "Status", Source = new Uri("/ServerMode/Contest/Status.xaml#" + id, UriKind.Relative) });
            lstLinks.Links.Add(new Link { DisplayName = "Standings", Source = new Uri("/ServerMode/Contest/Standings.xaml#" + id, UriKind.Relative) });
            var contest = App.Server.GetContest(id);
            if (contest.Type == Entity.ContestType.Codeforces || contest.Type == Entity.ContestType.TopCoder)
                lstLinks.Links.Add(new Link { DisplayName = "Hacks", Source = new Uri("/ServerMode/Contest/HackList.xaml#" + id, UriKind.Relative) });
            lstLinks.Links.Add(new Link { DisplayName = "Questions", Source = new Uri("/ServerMode/Contest/Questions.xaml#" + id, UriKind.Relative) });
            lstLinks.Links.Add(new Link { DisplayName = "Print Requests", Source = new Uri("/ServerMode/Contest/Print.xaml#" + id, UriKind.Relative) });
            lstLinks.SelectedSource = new Uri("/ServerMode/Contest/General.xaml#" + id, UriKind.Relative);
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
    }
}
