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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FirstFloor.ModernUI.Windows.Navigation;

namespace CenaPlus.Server.ServerMode.Contest
{
    /// <summary>
    /// Interaction logic for ProblemList.xaml
    /// </summary>
    public partial class ProblemList : UserControl
    {
        public List<ProblemListItem> ProblemListItems = new List<ProblemListItem>();
        public ProblemList()
        {
            InitializeComponent();
            for (char i = 'A'; i <= 'J'; i++)
            {
                ProblemListItem t = new ProblemListItem();
                t.Number = i + "";
                t.Point = 1500;
                t.TestCasesCount = 10;
                ProblemListItems.Add(t);
            }
            ProblemListView.ItemsSource = ProblemListItems;
        }
        public class ProblemListItem
        {
            public string Number { get; set; }
            public string Title { get; set; }
            public int TestCasesCount { get; set; }
            public int Point { get; set; }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Create a default record, and then turn to the modify mode;
            var frame = NavigationHelper.FindFrame(null, this);
            if (frame != null)
            {
                frame.Source = new Uri("/ServerMode/Contest/Problem/Problem.xaml", UriKind.Relative);
            }
        }
    }
}
