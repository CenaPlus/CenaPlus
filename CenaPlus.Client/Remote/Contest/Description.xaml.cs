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
using System.IO;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using CenaPlus.Client.Bll;
namespace CenaPlus.Client.Remote.Contest
{
    /// <summary>
    /// Interaction logic for Contest_Description.xaml
    /// </summary>
    public partial class Description : UserControl, IContent
    {
        public Description()
        {
            InitializeComponent();
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            int id = int.Parse(e.Fragment);
            Entity.Contest contest = Foobar.Server.GetContest(id);
            var wholeRange = new TextRange(txtDescription.Document.ContentStart, txtDescription.Document.ContentEnd);
            using (MemoryStream mem = new MemoryStream(Encoding.UTF8.GetBytes(contest.Description)))
            {
                wholeRange.Load(mem, DataFormats.Rtf);
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
