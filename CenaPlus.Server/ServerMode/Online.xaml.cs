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

namespace CenaPlus.Server.ServerMode
{
    /// <summary>
    /// Interaction logic for Online.xaml
    /// </summary>
    public partial class Online : UserControl
    {
        public List<OnlineListItem> OnlineListItems = new List<OnlineListItem>();
        public Online()
        {
            InitializeComponent();
            for (int i = 0; i < 10; i++)
            {
                OnlineListItem t = new OnlineListItem();
                t.IP = "127.0.0.1:8888";
                t.Username = "GasaiYuno";
                t.Nickname = "Gasai Yuno";
                OnlineListItems.Add(t);
            }
            OnlineListBox.ItemsSource = OnlineListItems;
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
    }
    public class OnlineListItem
    {
        public string Username { get; set; }
        public string Nickname { get; set; }
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
                return String.Format("{0}({1})", Username, Nickname);
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
