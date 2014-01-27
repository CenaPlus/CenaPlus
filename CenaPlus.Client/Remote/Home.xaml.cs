using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
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
using FirstFloor.ModernUI.Windows.Controls;
using CenaPlus.Client.Bll;
namespace CenaPlus.Client.Remote
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl,IContent
    {
        public Home()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            IPAddress ip;
            var addresses = Dns.GetHostAddresses(txtServerAddr.Text);
            if (addresses.Length == 0)
            {
                ModernDialog.ShowMessage("Invalid IP address", "Error", MessageBoxButton.OK);
                return;
            }
            else
            {
                ip = addresses[0];
            }

            int port;
            if (!int.TryParse(txtServerPort.Text, out port))
            {
                ModernDialog.ShowMessage("Invalid port", "Error", MessageBoxButton.OK);
                return;
            }

            CenaPlusServerProxy server;
            try
            {
                server = new Bll.CenaPlusServerProxy(new IPEndPoint(ip, port), new Bll.ServerCallback());
            }
            catch
            {
                ModernDialog.ShowMessage("Connection to " + ip + ":" + port + " failed.", "Error", MessageBoxButton.OK);
                return;
            }

            if (!server.Authenticate(txtUserName.Text, txtPassword.Password))
            {
                ModernDialog.ShowMessage("Incorrect user name or password.", "Error", MessageBoxButton.OK);
                return;
            }

            App.Server = server;
            App.HeartBeatTimer.Start();

            var frame = NavigationHelper.FindFrame(null, this);
            if (frame != null)
            {
                frame.Source = new Uri("/Remote/Profile.xaml", UriKind.Relative);
            }
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
        }

        public void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            if (App.Server != null)
            {
                var frame = NavigationHelper.FindFrame(null, this);
                if (frame != null)
                {
                    frame.Source = new Uri("/Remote/Profile.xaml", UriKind.Relative);
                }
            }
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
        }
    }
}
