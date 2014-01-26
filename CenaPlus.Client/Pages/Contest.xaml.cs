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
namespace CenaPlus.Client.Pages
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
            tabLeft.Links.Clear();
            tabLeft.Links.Add(new Link { DisplayName = "description", Source = new Uri("/Content/Contest_Description.xaml#" + id, UriKind.Relative) });
            tabLeft.Links.Add(new Link { DisplayName = "problems", Source = new Uri("/Pages/Contest_Problems.xaml#" + id, UriKind.Relative) });
            tabLeft.Links.Add(new Link { DisplayName = "rank list", Source = new Uri("/Content/Contest_RankList.xaml#" + id, UriKind.Relative) });
            tabLeft.Links.Add(new Link { DisplayName = "statistics", Source = new Uri("/Content/Contest_Statistics.xaml#" + id, UriKind.Relative) });
            tabLeft.Links.Add(new Link { DisplayName = "faq", Source = new Uri("/Content/Contest_FAQ.xaml#" + id, UriKind.Relative) });
            tabLeft.Links.Add(new Link { DisplayName = "print service", Source = new Uri("/Content/Contest_PrintService.xaml#" + id, UriKind.Relative) });
            tabLeft.SelectedSource = new Uri("/Content/Contest_Description.xaml#" + id, UriKind.Relative);
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
