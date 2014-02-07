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

namespace CenaPlus.Server.ServerMode.Contest.Problem
{
    /// <summary>
    /// Interaction logic for Problem.xaml
    /// </summary>
    public partial class Problem : UserControl, IContent
    {
        public Problem()
        {
            InitializeComponent();
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            int id = int.Parse(e.Fragment);
            tabTop.Links.Clear();
            tabTop.Links.Add(new Link { DisplayName = "General", Source = new Uri("/ServerMode/Contest/Problem/General.xaml#" + id, UriKind.Relative) });
            tabTop.Links.Add(new Link { DisplayName = "Content", Source = new Uri("/ServerMode/Contest/Problem/Content.xaml#" + id, UriKind.Relative) });
            tabTop.Links.Add(new Link { DisplayName = "Test cases", Source = new Uri("/ServerMode/Contest/Problem/TestCases.xaml#" + id, UriKind.Relative) });
            tabTop.Links.Add(new Link { DisplayName = "SPJ", Source = new Uri("/ServerMode/Contest/Problem/Code.xaml#field=SPJ&id=" + id, UriKind.Relative) });
            tabTop.Links.Add(new Link { DisplayName = "Range validator", Source = new Uri("/ServerMode/Contest/Problem/Code.xaml#field=RangeValidator&id=" + id, UriKind.Relative) });
            tabTop.Links.Add(new Link { DisplayName = "Std source", Source = new Uri("/ServerMode/Contest/Problem/Code.xaml#field=StdSource&id=" + id, UriKind.Relative) });
            tabTop.SelectedSource = new Uri("/ServerMode/Contest/Problem/General.xaml#" + id, UriKind.Relative);
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
