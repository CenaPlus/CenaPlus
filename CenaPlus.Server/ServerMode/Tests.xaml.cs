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

namespace CenaPlus.Server.ServerMode
{
    /// <summary>
    /// Interaction logic for Tests.xaml
    /// </summary>
    public partial class Tests : UserControl
    {
        public List<ContestListItem> ContestListItems = new List<ContestListItem>();
        public Tests()
        {
            InitializeComponent();
            for (int i = 0; i < 10; i++)
            {
                ContestListItem t = new ContestListItem();
                t.StartTime = Convert.ToDateTime("2014-2-3 10:00");
                t.EndTime = Convert.ToDateTime("2014-2-3 14:00");
                t.Type = Entity.ContestType.Codeforces;
                t.Title = "Cena+ Round #" + (i + 1).ToString();
                ContestListItems.Add(t);
            }
            ContestListBox.ItemsSource = ContestListItems;
        }

        private void ModifyButton_Click(object sender, RoutedEventArgs e)
        {
            var frame = NavigationHelper.FindFrame(null, this);
            if (frame != null)
            {
                frame.Source = new Uri("/ServerMode/Contest/Contest.xaml#" + (ContestListBox.SelectedItem as ContestListItem).ID, UriKind.Relative);
            }
        }

        private void ContestListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ContestListBox.SelectedItem != null)
            {
                ModifyButton.IsEnabled = true;
                DeleteButton.IsEnabled = true;
            }
            else
            {
                ModifyButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
            }
        }
    }
    public class ContestListItem : CenaPlus.Entity.Contest
    {
        private const string DetailTemplate = "{0} UTC / {1} hrs / {2} Format";
        public string Detail
        {
            get
            {
                return String.Format(DetailTemplate, StartTime, (Duration.TotalSeconds / 60 / 60).ToString("0.0"), Type);
            }
        }
    }
}
