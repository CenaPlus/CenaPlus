using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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

namespace CenaPlus.Client.Remote.Contest
{
    /// <summary>
    /// Interaction logic for Problem.xaml
    /// </summary>
    public partial class Problem : UserControl, IContent
    {
        private int problemID;

        public Problem()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            var frame = NavigationHelper.FindFrame(null, this);
            if (frame != null)
            {
                frame.Source = new Uri("/Remote/Contest/Submit.xaml#" + problemID, UriKind.Relative);
            }
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            problemID = int.Parse(e.Fragment);
            var problem = App.Server.GetProblem(problemID);
            txtTitle.Text = problem.Title;
            var wholePage = new TextRange(txtContent.Document.ContentStart, txtContent.Document.ContentEnd);
            using (MemoryStream mem = new MemoryStream(Encoding.UTF8.GetBytes(problem.Content)))
            {
                wholePage.Load(mem, DataFormats.Rtf);
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
