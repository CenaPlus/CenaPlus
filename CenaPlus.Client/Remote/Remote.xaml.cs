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
using System.Net.Sockets;
using System.Net;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;
using CenaPlus.Network;
using CenaPlus.Client.Bll;
using System.ServiceModel;

namespace CenaPlus.Client.Remote
{
    /// <summary>
    /// Interaction logic for Remote.xaml
    /// </summary>
    public partial class Remote : UserControl, IContent
    {
        private ServerDiscoverer discoverer;
        private HashSet<EndPoint> foundServers;

        public Remote()
        {
            InitializeComponent();
        }

        private int GetDelay(IPEndPoint addr)
        {
            var start = DateTime.Now;
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    client.Connect(addr);
                }
            }
            catch
            {
                return 999999;
            }
            var end = DateTime.Now;
            return (int)(end - start).TotalMilliseconds;
        }

        private void ServerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ServerListBox.SelectedItem != null)
            {
                IdentificationPanel.IsEnabled = true;
                btnLogin.IsEnabled = true;
                txtUserName.IsEnabled = true;
                txtPassword.IsEnabled = true;
                IdentificationTextBlock.Text = "Sign in to " + (ServerListBox.SelectedItem as ServerListItem).Name;
            }
            else
            {
                IdentificationPanel.IsEnabled = false;
                btnLogin.IsEnabled = false;
                txtUserName.IsEnabled = false;
                txtPassword.IsEnabled = false;
                IdentificationTextBlock.Text = "Choose a server from the left...";
            }
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            IPAddress address;
            try
            {
                var addresses = Dns.GetHostAddresses(txtAddress.Text);
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

            IPEndPoint endpoint = new IPEndPoint(address, port);
            if (foundServers.Contains(endpoint))
            {
                ModernDialog.ShowMessage("This server is already in the list", "Error", MessageBoxButton.OK);
                return;
            }

            foundServers.Add(endpoint);
            ServerListBox.Items.Add(new ServerListItem
            {
                Name = txtAddress.Text,
                Location = endpoint,
                Delay = GetDelay(endpoint)
            });
            ServerListBox.Items.Refresh();

            txtAddress.Text = "";
            txtPort.Text = "";
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var serverItem = ServerListBox.SelectedItem as ServerListItem;

            CenaPlusServerProxy server;
            try
            {
                server = new Bll.CenaPlusServerProxy(serverItem.Location, new Bll.ServerCallback());
            }
            catch
            {
                ModernDialog.ShowMessage("Connection to " + serverItem.Location + " failed.", "Error", MessageBoxButton.OK);
                return;
            }

            try
            {
                if (!server.Authenticate(txtUserName.Text, txtPassword.Password))
                {
                    ModernDialog.ShowMessage("Incorrect user name or password.", "Error", MessageBoxButton.OK);
                    return;
                }
            }
            catch (FaultException<AlreadyLoggedInError>)
            {
                ModernDialog.ShowMessage("This account is online.", "Error", MessageBoxButton.OK);
                return;
            }

            App.Server = server;
            App.HeartBeatTimer.Start();

            var frame = NavigationHelper.FindFrame(null, this);
            if (frame != null)
            {
                frame.Source = new Uri("/Remote/Home.xaml", UriKind.Relative);
            }
        }
        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
        }

        public void OnNavigatedFrom(NavigationEventArgs e)
        {
            discoverer.Stop();
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            foundServers = new HashSet<EndPoint>();
            discoverer = new ServerDiscoverer();
            ServerListBox.Items.Clear();

            discoverer.FoundServer += (svr) =>
            {
                if (!foundServers.Contains(svr.Location))
                {
                    foundServers.Add(svr.Location);
                    var delay = GetDelay(svr.Location);

                    Dispatcher.Invoke(new Action(() =>
                    {
                        ServerListBox.Items.Add(new ServerListItem
                        {
                            Name = svr.Name,
                            Location = svr.Location,
                            Delay = delay
                        });
                        ServerListBox.Items.Refresh();
                    }));
                }
            };
            discoverer.Start();
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
        }


        private class ServerListItem : IntranetServer
        {
            public int Delay { get; set; }

            public string Details
            {
                get
                {
                    return String.Format("{0} / Delay: {1} ms / Online: 0", Location, Delay);
                }
            }
        }

    }

}
