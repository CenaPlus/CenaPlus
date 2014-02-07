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
        private int problemID;

        public Submit()
        {
            InitializeComponent();
            lstLanguage.ItemsSource =  Enum.GetNames(typeof(ProgrammingLanguage)));
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            problemID = int.Parse(e.Fragment);
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

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            int recordID = App.Server.Submit(problemID, txtCode.Text, (ProgrammingLanguage)Enum.Parse(typeof(ProgrammingLanguage), (string)lstLanguage.SelectedItem));
            MessageBox.Show("TODO: navigate to record #" + recordID);
        }
    }
}
