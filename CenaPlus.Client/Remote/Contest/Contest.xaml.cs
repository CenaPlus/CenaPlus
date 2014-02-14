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
namespace CenaPlus.Client.Remote.Contest
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
            var contest = App.Server.GetContest(id);

            tabLeft.Links.Clear();
            tabLeft.Links.Add(new Link { DisplayName = "description", Source = new Uri("/Remote/Contest/Description.xaml#" + id, UriKind.Relative) });
            tabLeft.Links.Add(new Link { DisplayName = "problems", Source = new Uri("/Remote/Contest/Problems.xaml#" + id, UriKind.Relative) });
            tabLeft.Links.Add(new Link { DisplayName = "standings", Source = new Uri("/Remote/Contest/Standings.xaml#" + id, UriKind.Relative) });
            tabLeft.Links.Add(new Link { DisplayName = "status", Source = new Uri("/Remote/Contest/Status.xaml#" + id, UriKind.Relative) });
            if (contest.Type != Entity.ContestType.OI)
            {
                tabLeft.Links.Add(new Link { DisplayName = "statistics", Source = new Uri("/Remote/Contest/Statistics.xaml#" + id, UriKind.Relative) });
            }
            tabLeft.Links.Add(new Link { DisplayName = "Q&A", Source = new Uri("/Remote/Contest/Questions.xaml#" + id, UriKind.Relative) });
            if (contest.PrintingEnabled)
            {
                tabLeft.Links.Add(new Link { DisplayName = "print service", Source = new Uri("/Remote/Contest/Print.xaml#" + id, UriKind.Relative) });
            }
            tabLeft.SelectedSource = new Uri("/Remote/Contest/Description.xaml#" + id, UriKind.Relative);
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
