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
namespace CenaPlus.Client.Remote.Contest
{
    /// <summary>
    /// Interaction logic for Problem_Submit.xaml
    /// </summary>
    public partial class Submit : UserControl, IContent
    {
        public Submit()
        {
            InitializeComponent();
            foreach (string lang in Enum.GetNames(typeof(ProgrammingLanguage)))
            {
                lstLanguage.Items.Add(new ListBoxItem { Content = lang });
            }
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {

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
