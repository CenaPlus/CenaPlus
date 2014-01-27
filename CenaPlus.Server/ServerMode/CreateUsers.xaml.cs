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
using FirstFloor.ModernUI.Windows.Controls;

namespace CenaPlus.Server.ServerMode
{
    /// <summary>
    /// Interaction logic for CreateUsers.xaml
    /// </summary>
    public partial class CreateUsers : UserControl
    {
        public CreateUsers()
        {
            InitializeComponent();
        }

        private void PasswordModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PasswordModeComboBox.SelectedIndex == 0)
            {
                DefinedPanel.Visibility = Visibility.Visible;
                RandomPanel.Visibility = Visibility.Collapsed;
                ConfirmButton.IsEnabled = true;
            }
            else if (PasswordModeComboBox.SelectedIndex == 1)
            {
                DefinedPanel.Visibility = Visibility.Collapsed;
                RandomPanel.Visibility = Visibility.Visible;
                ConfirmButton.IsEnabled = true;
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBox.Visibility = Visibility.Visible;
            ModernDialog.ShowMessage("Temporary users are created sucessful.", "Message", MessageBoxButton.OK);
        }
    }
}
