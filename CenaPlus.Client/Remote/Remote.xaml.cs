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

namespace CenaPlus.Client.Remote
{
    /// <summary>
    /// Interaction logic for Remote.xaml
    /// </summary>
    public partial class Remote : UserControl
    {
        public List<ServerListItem> ServerListItems = new List<ServerListItem>();
        public Remote()
        {
            InitializeComponent();
            //Official address
            ServerListItem t = new ServerListItem();
            t.Title = "Cena+ Official Server";
            t.Online = 100;
            t.IP = "www.cenaplus.org";
            t.Port = 9999;
            t.Ping = 1;
            ServerListItems.Add(t);
            ServerListBox.ItemsSource = ServerListItems;
        }

        private void ServerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ServerListBox.SelectedItem != null)
            {
                IdentificationPanel.IsEnabled = true;
                LoginButton.IsEnabled = true;
                UsernameBox.IsEnabled = true;
                PasswordBox.IsEnabled = true;
                IdentificationTextBlock.Text = "Identification";
            }
            else
            {
                IdentificationPanel.IsEnabled = false;
                LoginButton.IsEnabled = false;
                UsernameBox.IsEnabled = false;
                PasswordBox.IsEnabled = false;
                IdentificationTextBlock.Text = "Please select a server.";
            } 
        }
    }
    public class ServerListItem
    {
        public string Title { get; set; }
        public int Ping { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public int Online { get; set; }
        public string Details
        {
            get 
            {
                return String.Format("{0}:{1} / ping {2}ms / {3} online user(s)", IP, Port, Ping, Online);
            }
        }
    }
}
