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
using CenaPlus.Entity;
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
                FixedPanel.Visibility = Visibility.Visible;
                RandomPanel.Visibility = Visibility.Collapsed;
                ConfirmButton.IsEnabled = true;
            }
            else if (PasswordModeComboBox.SelectedIndex == 1)
            {
                FixedPanel.Visibility = Visibility.Collapsed;
                RandomPanel.Visibility = Visibility.Visible;
                ConfirmButton.IsEnabled = true;
            }
        }

        private string RandomString(Random rand, int length)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                int ch = rand.Next(26 + 26 + 10);
                if (ch < 26) sb.Append((char)(ch + 'A'));
                else if (ch < 26 + 26) sb.Append((char)(ch - 26 + 'a'));
                else sb.Append((char)(ch - 26 - 26 + '0'));
            }
            return sb.ToString();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            int count;
            if (!int.TryParse(txtCount.Text, out count))
            {
                ModernDialog.ShowMessage("Count must be an integer", "Error", MessageBoxButton.OK);
                return;
            }

            int numberLength;
            if (!int.TryParse(txtNumberLength.Text, out numberLength))
            {
                ModernDialog.ShowMessage("Postfix number length must be an integer", "Error", MessageBoxButton.OK);
                return;
            }

            int passwordLength;
            if (!int.TryParse(txtPasswordLength.Text, out passwordLength))
            {
                ModernDialog.ShowMessage("Password length must be an integer", "Error", MessageBoxButton.OK);
                return;
            }

            string nameFormat = string.Format("{{0}}{{1:D{0}}}", numberLength);
            string prefix = txtPrefix.Text;
            string fixedPassword = txtPassword.Text;

            StringBuilder sb = new StringBuilder();

            if (PasswordModeComboBox.SelectedIndex == 0)//Fixed
            {
                for (int i = 0; i < count; i++)
                {
                    var name = string.Format(nameFormat, prefix, i+1);
                    App.Server.CreateUser(name, name, fixedPassword, UserRole.Competitor);
                    sb.AppendFormat("{0} {1}\n", name, fixedPassword);
                }
            }
            else
            {
                var rand = new Random();
                for (int i = 0; i < count; i++)
                {
                    var name = string.Format(nameFormat, prefix, i+1);
                    string password = RandomString(rand, passwordLength);
                    App.Server.CreateUser(name, name, password, UserRole.Competitor);
                    sb.AppendFormat("{0} {1}\n", name, password);
                }
            }
            ResultTextBox.Text = sb.ToString();
            ModernDialog.ShowMessage("Temporary users are created sucessful.", "Message", MessageBoxButton.OK);
        }
    }
}
