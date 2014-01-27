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
        public Tests()
        {
            InitializeComponent();
        }

        private void ModifyButton_Click(object sender, RoutedEventArgs e)
        {
            var frame = NavigationHelper.FindFrame(null, this);
            if (frame != null)
            {
                frame.Source = new Uri("/ServerMode/Contest/Main.xaml", UriKind.Relative);
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
}
