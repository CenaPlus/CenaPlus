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
using System.Security;
using System.Security.Cryptography;
using CenaPlus.Entity;

namespace CenaPlus.Server.ServerMode
{
    /// <summary>
    /// Interaction logic for Users.xaml
    /// </summary>
    public partial class Users : UserControl
    {
        public List<UserListItem> UserList = new List<UserListItem>();
        public Users()
        {
            InitializeComponent();
            for (int i = 1; i <= 10; i++)
            {
                UserListItem t = new UserListItem();
                t.ID = i;
                t.Name = "shabi#"+i;
                t.Role = UserRole.Competitor;
                UserList.Add(t);
            }
            UserListBox.ItemsSource = UserList;
        }

        private void UserListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UserListBox.SelectedItem != null)
            {
                ProfileDisplay.Visibility = Visibility.Visible;
                NameTextBox.Text = (UserListBox.SelectedItem as UserListItem).Name;
            }
            else
            {
                ProfileDisplay.Visibility = Visibility.Hidden;
            }
        }
    }

    public class UserListItem : Entity.User
    {
        public string Gravatar
        {
            get 
            {
                return @"https://www.gravatar.com/avatar/159c4a0a78d0980aca8df9d781d1c755?d=https://www.SmartOJ.com/img/Non_Avatar.png&s=50";
                //return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Gravatar, "MD5").ToLower();
            }
        }
        public string Profile
        {
            get 
            {
                return String.Format("{0} / {1} / Email: {2}", Role.ToString(), "Qiqihar University", "???");
            }
        }
    }
}
