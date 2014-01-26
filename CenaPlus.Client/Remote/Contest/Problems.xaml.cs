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
using CenaPlus.Client.Bll;
namespace CenaPlus.Client.Remote.Contest
{
    /// <summary>
    /// Interaction logic for Contest_Problems.xaml
    /// </summary>
    public partial class Problems : UserControl, IContent
    {
        public Problems()
        {
            InitializeComponent();
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            ProblemTab.Links.Clear();

            int contestID = int.Parse(e.Fragment);
            var ids = App.Server.GetProblemList(contestID);

            if (ids.Count != 0)
            {
                for (int i = 0; i < ids.Count; i++)
                {
                    char c = (char)('A' + i);
                    ProblemTab.Links.Add(new Link { DisplayName = c.ToString(), Source = new Uri("/Remote/Contest/Problem.xaml#" + ids[i], UriKind.Relative) });
                }
                ProblemTab.SelectedSource = new Uri("/Remote/Contest/Problem.xaml#" + ids[0], UriKind.Relative);
            }
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
