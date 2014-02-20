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
using CenaPlus.Server.Bll;
namespace CenaPlus.Server.ServerMode
{
    /// <summary>
    /// Interaction logic for Online.xaml
    /// </summary>
    public partial class Online : UserControl, IContent
    {
        public List<OnlineListItem> onlineList = new List<OnlineListItem>();

        public Online()
        {
            InitializeComponent();
            OnlineListBox.ItemsSource = onlineList;
            LocalCenaServer.UserLoggedIn += LocalCenaServer_UserLoggedIn;
            App.RemoteCallback.OnLogin += LocalCenaServer_UserLoggedIn;
            LocalCenaServer.UserLoggedOut += LocalCenaServer_UserLoggedOut;
            App.RemoteCallback.OnLogout += LocalCenaServer_UserLoggedOut;
        }

        void LocalCenaServer_UserLoggedOut(int userID)
        {
            var index = onlineList.FindIndex(i => i.ID == userID);
            if (index >= 0)
            {
                 Dispatcher.Invoke(new Action(() => {
                     onlineList.RemoveAt(index);
                     OnlineListBox.Items.Refresh();
                 }));
            }
        }

        void LocalCenaServer_UserLoggedIn(int userID)
        {
            Dispatcher.Invoke(new Action(() => {
                onlineList.Add(new OnlineListItem(App.Server.GetUser(userID)));
                OnlineListBox.Items.Refresh();
            }));
        }

        private void OnlineListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OnlineListBox.SelectedItem != null)
            {
                btnBanIP.IsEnabled = true;
                btnBanUser.IsEnabled = true;
                btnKick.IsEnabled = true;
            }
            else
            {
                btnBanIP.IsEnabled = false;
                btnBanUser.IsEnabled = false;
                btnKick.IsEnabled = false;
            }
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
        }

        public void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        private void btnKick_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)OnlineListBox.SelectedValue;
            App.Server.Kick(id);
            onlineList.RemoveAt(OnlineListBox.SelectedIndex);
            OnlineListBox.Items.Refresh();
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            var list = from id in App.Server.GetOnlineList()
                       let u = App.Server.GetUser(id)
                       select new OnlineListItem(u);
            onlineList.Clear();
            foreach (var item in list) onlineList.Add(item);

        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
        }


    }
    public class OnlineListItem : User
    {
        public OnlineListItem(User u)
        {
            ID = u.ID;
            IP = "Unknown";
            Name = u.Name;
            NickName = u.NickName;
        }

        public string IP { get; set; }
        public string Gravatar
        {
            get
            {
                return @"https://www.gravatar.com/avatar/159c4a0a78d0980aca8df9d781d1c755?d=https://www.SmartOJ.com/img/Non_Avatar.png&s=50";
            }
        }
        public string Title
        {
            get
            {
                return String.Format("{0}({1})", Name, NickName);
            }
        }
        public string Details
        {
            get
            {
                return String.Format("From: {0}", IP);
            }
        }
    }
}
