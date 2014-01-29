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
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;
using CenaPlus.Entity;
namespace CenaPlus.Server.ServerMode
{
    /// <summary>
    /// Interaction logic for CreateAUser.xaml
    /// </summary>
    public partial class CreateAUser : UserControl
    {
        public CreateAUser()
        {
            InitializeComponent();
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtNickName.Text = txtName.Text;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Password != txtPasswordConfirm.Password)
            {
                ModernDialog.ShowMessage("Password and confirmation do not match", "Error", MessageBoxButton.OK);
            }
            App.Server.CreateUser(txtName.Text, txtNickName.Text, txtPassword.Password, UserRole.Competitor);

            var frame = NavigationHelper.FindFrame(null, this);
            if (frame != null)
            {
                frame.Source = new Uri("/ServerMode/Users.xaml", UriKind.Relative);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var frame = NavigationHelper.FindFrame(null, this);
            if (frame != null)
            {
                frame.Source = new Uri("/ServerMode/Users.xaml", UriKind.Relative);
            }
        }
    }
}
