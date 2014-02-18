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
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;

namespace CenaPlus.Client.Remote.Contest
{
    /// <summary>
    /// Interaction logic for ProblemGeneral.xaml
    /// </summary>
    public partial class ProblemGeneral : UserControl, IContent
    {
        private List<ProblemListBoxItem> ProblemListBoxItems = new List<ProblemListBoxItem>();
        public ProblemGeneral()
        {
            InitializeComponent();
        }

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
            int contest_id = int.Parse(e.Fragment);
            var list = from id in App.Server.GetProblemList(contest_id)
                       let q = App.Server.GetProblemTitle(id)
                       select new ProblemListBoxItem
                       {
                           Title = q.Title,
                           Status = q.Status,
                           Time = q.Time,
                           TimeLimit = q.TimeLimit,
                           MemoryLimit = q.MemoryLimit,
                           SpecialJudge = q.SpecialJudge,
                           ProblemID = q.ProblemID
                       };
            char i = 'A';
            ProblemListBoxItems.Clear();
            foreach (var item in list)
            {
                item.Number = i++;
                ProblemListBoxItems.Add(item);
            }
            ProblemListBox.ItemsSource = ProblemListBoxItems;
        }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {

        }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {

        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            
        }

        private void ProblemListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ProblemListBox.SelectedItem != null)
            {
                var frame = NavigationHelper.FindFrame(null, this);
                if (frame != null)
                {
                    frame.Source = new Uri("/Remote/Contest/Problem.xaml#" + (ProblemListBox.SelectedItem as ProblemListBoxItem).ProblemID, UriKind.Relative);
                }
            }
        }
    }
    public class ProblemListBoxItem: Entity.ProblemGeneral
    {
        public char Number { get; set; }
        public string Header 
        {
            get
            {
                return String.Format("{0}: {1} {2}", Number, Title, Status == Entity.ProblemGeneralStatus.Hacked ? "[Hacked]" : "");
            }
        }
        public string Limits
        {
            get
            {
                return String.Format("Time limit: {0} ms / Memory limit: {1} KiB{2}", TimeLimit, MemoryLimit, SpecialJudge?" / Special judge mode":"");
            }
        }
        public string Details 
        {
            get 
            {
                if (Status == null)
                {
                    return "You have not tried this problem.";
                }
                else 
                {
                    return String.Format("{0} @{1}", Status.ToString(), Time.ToShortTimeString());
                }
            }
        }
    }
}
