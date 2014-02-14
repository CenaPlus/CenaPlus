using CenaPlus.Server.Bll;
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
using System.Net;
using FirstFloor.ModernUI.Windows.Controls;

namespace CenaPlus.Server.ServerMode.ServerSettings
{
    /// <summary>
    /// Interaction logic for Judgers.xaml
    /// </summary>
    public partial class Judgers : UserControl,IContent
    {
        private List<JudgeNodeListBoxItem> JudgeNodeListBoxItems = new List<JudgeNodeListBoxItem>();

        public Judgers()
        {
            InitializeComponent();
            JudgeNodeListBox.ItemsSource = JudgeNodeListBoxItems;
        }

        private void JudgeNodeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (JudgeNodeListBox.SelectedItem != null)
            {
                btnRemove.IsEnabled = true;
            }
            else
            {
                btnRemove.IsEnabled = false;
            }
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            IPAddress address;
            try
            {
                var addresses = Dns.GetHostAddresses(txtAddr.Text);
                address = addresses[0];
            }
            catch
            {
                ModernDialog.ShowMessage("Invalid server address", "Error", MessageBoxButton.OK);
                return;
            }

            int port;
            if (!int.TryParse(txtPort.Text, out port))
            {
                ModernDialog.ShowMessage("Port must be an integer", "Error", MessageBoxButton.OK);
                return;
            }

            if (port < 0 || port > 65535)
            {
                ModernDialog.ShowMessage("Port is not in the valid range", "Error", MessageBoxButton.OK);
                return;
            }

            IPEndPoint location = new IPEndPoint(address, port);
            var info = new JudgeNodeInfo()
            {
                Location = location,
                Name = location.ToString(),
                Password = txtPassword.Password
            };
            if (!info.IsOnline)
            {
                ModernDialog.ShowMessage("Cannot connect to this judge node", "Error", MessageBoxButton.OK);
                return;
            }
            if (!info.CheckPassword())
            {
                ModernDialog.ShowMessage("Password is not correct", "Error", MessageBoxButton.OK);
                return;
            }
            App.JudgeNodes.Add(info);
            Load();
            txtAddr.Text = "";
            txtPassword.Password = "";
            txtPort.Text = "";
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            App.JudgeNodes.RemoveAt(JudgeNodeListBox.SelectedIndex);
            Load();
        }

        private void Load()
        {
            var list = from n in App.JudgeNodes
                       select new JudgeNodeListBoxItem
                       {
                           Location = n.Location,
                           Name = n.Name,
                           Password = n.Password
                       };
            JudgeNodeListBoxItems.Clear();
            JudgeNodeListBoxItems.AddRange(list);
            JudgeNodeListBox.Items.Refresh();
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
        }

        public void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            Load();
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
        }

        class JudgeNodeListBoxItem : JudgeNodeInfo
        {
            public string Details
            {
                get
                {
                    return String.Format("{0}{1}", IsOnline ? "Online " : "", Location);
                }
            }
        }

    }

    
}
