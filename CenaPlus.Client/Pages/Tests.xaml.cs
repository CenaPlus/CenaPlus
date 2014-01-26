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
using FirstFloor.ModernUI.Windows.Controls;
using CenaPlus.Entity;
using CenaPlus.Client.Bll;
namespace CenaPlus.Client.Pages
{
    /// <summary>
    /// Interaction logic for Tests.xaml
    /// </summary>
    public partial class Tests : UserControl
    {
        public Tests()
        {
            InitializeComponent();
        }

        private void ContestListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var frame = NavigationHelper.FindFrame(null, this);
            if (frame != null)
            {
                frame.Source = new Uri("/Pages/Contest.xaml?id=" + ContestListBox.SelectedValue, UriKind.Relative);
            }
            ContestListBox.SelectedIndex = -1;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var list = from id in Foobar.Server.GetContestList()
                       let c = Foobar.Server.GetContest(id)
                       select new ContestList
                       {
                           ID = c.ID,
                           Title = c.Title,
                           StartTime = c.StartTime,
                           EndTime = c.EndTime,
                           Type = c.Type
                       };
            ContestListBox.ItemsSource = list;
        }

        public class ContestList : CenaPlus.Entity.Contest
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
}
