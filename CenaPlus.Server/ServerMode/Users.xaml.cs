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
using System.Security;
using System.Security.Cryptography;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using FirstFloor.ModernUI.Windows.Controls;
using CenaPlus.Entity;

namespace CenaPlus.Server.ServerMode
{
    /// <summary>
    /// Interaction logic for Users.xaml
    /// </summary>
    public partial class Users : UserControl, IContent
    {
        private List<UserListItem> userList;

        public Users()
        {
            InitializeComponent();
        }

        private void UserListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UserListBox.SelectedItem != null)
            {
                ProfileDisplay.Visibility = Visibility.Visible;
                var user = UserListBox.SelectedItem as UserListItem;
                btnDelete.IsEnabled = true;
                txtName.Text = user.Name;
                txtNickName.Text = user.NickName;
                txtPassword.Password = "";
                lstRole.SelectedIndex = (int)user.Role;
            }
            else
            {
                btnDelete.IsEnabled = false;
                ProfileDisplay.Visibility = Visibility.Hidden;
            }
        }

        private void btnBatchCreate_Click(object sender, RoutedEventArgs e)
        {
            var frame = NavigationHelper.FindFrame(null, this);
            if (frame != null)
            {
                frame.Source = new Uri("/ServerMode/CreateUsers.xaml", UriKind.Relative);
            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            var frame = NavigationHelper.FindFrame(null, this);
            if (frame != null)
            {
                frame.Source = new Uri("/ServerMode/CreateAUser.xaml", UriKind.Relative);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var user = UserListBox.SelectedItem as UserListItem;
            App.Server.UpdateUser(user.ID, txtName.Text, txtNickName.Text, txtPassword.Password == "" ? null : txtPassword.Password, null);
            user.Name = txtName.Text;
            user.NickName = txtNickName.Text;
            user.Role = (UserRole)lstRole.SelectedIndex;
            UserListBox.Items.Refresh();
            ModernDialog.ShowMessage("User profile saved", "Cena+", MessageBoxButton.OK);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            App.Server.DeleteUser((int)UserListBox.SelectedValue);
            userList.RemoveAt(UserListBox.SelectedIndex);
            UserListBox.Items.Refresh();
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
        }

        public void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            var list = from id in App.Server.GetUserList()
                       let u = App.Server.GetUser(id)
                       select new UserListItem
                       {
                           ID = u.ID,
                           Name = u.Name,
                           NickName = u.NickName,
                           Role = u.Role
                       };
            userList = new List<UserListItem>();
            foreach (var item in list) userList.Add(item);
            UserListBox.ItemsSource = userList;
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
        }

        class UserListItem : Entity.User
        {
            public string Gravatar
            {
                get
                {
                    return @"https://www.gravatar.com/avatar/159c4a0a78d0980aca8df9d781d1c755?d=https://www.SmartOJ.com/img/Non_Avatar.png&s=50";
                }
            }
            public string Profile
            {
                get
                {
                    return String.Format("{0} / {1} ({2})", Role, Name, NickName);
                }
            }
        }



    }
}
