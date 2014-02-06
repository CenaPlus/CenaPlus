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
using CenaPlus.Entity;
namespace CenaPlus.Server.ServerMode.Contest
{
    /// <summary>
    /// Interaction logic for ProblemList.xaml
    /// </summary>
    public partial class ProblemList : UserControl, IContent
    {
        private List<ProblemListItem> ProblemListItems = new List<ProblemListItem>();
        private int contestID;

        public ProblemList()
        {
            InitializeComponent();
            ProblemListView.ItemsSource = ProblemListItems;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            int id = App.Server.CreateProblem(contestID, "New Problem", "", 0);
            var frame = NavigationHelper.FindFrame(null, this);
            if (frame != null)
            {
                frame.Source = new Uri("/ServerMode/Contest/Problem/Problem.xaml#" + id, UriKind.Relative);
            }
        }

        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            var frame = NavigationHelper.FindFrame(null, this);
            if (frame != null)
            {
                frame.Source = new Uri("/ServerMode/Contest/Problem/Problem.xaml", UriKind.Relative);
            }
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            contestID = int.Parse(e.Fragment);
            var list = from id in App.Server.GetProblemList(contestID)
                       let p = App.Server.GetProblem(id)
                       select new ProblemListItem
                       {
                           ID = p.ID,
                           Title = p.Title,
                           Score = p.Score,
                           TestCasesCount = p.TestCasesCount
                       };
            ProblemListItems.Clear();
            foreach (var item in list) ProblemListItems.Add(item);
            ProblemListView.Items.Refresh();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ProblemListView.SelectedIndex != -1)
            {
                int id = (int)ProblemListView.SelectedValue;
                App.Server.DeleteProblem(id);
                ProblemListItems.RemoveAt(ProblemListView.SelectedIndex);
                ProblemListView.Items.Refresh();
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
        class ProblemListItem : Entity.Problem
        {
        }


    }
}
