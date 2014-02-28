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
using System.Diagnostics;
using System.Configuration;
using System.Windows.Navigation;
using System.Net;
using System.Net.Sockets;
using System.IO;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;
using CenaPlus.Server.Bll;
using CenaPlus.Server.Dal;
using CenaPlus.Network;
using System.ServiceModel;
using CenaPlus.Entity;
namespace CenaPlus.Server.ServerMode
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class RemoteManage : UserControl
    {
        private CenaPlusServerHost host;

        public RemoteManage()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            IPAddress address;
            try
            {
                address = Dns.GetHostAddresses(txtAddr.Text)[0];
            }
            catch
            {
                ModernDialog.ShowMessage("Incorrect server address", "Error", MessageBoxButton.OK);
                return;
            }

            int port;
            if (!int.TryParse(txtRemotePort.Text, out port))
            {
                ModernDialog.ShowMessage("Port must be an integer", "Error", MessageBoxButton.OK);
                return;
            }

            try
            {
                App.Server = CenaPlusServerChannelFactory.CreateChannel(new IPEndPoint(address, port), App.RemoteCallback);
            }
            catch (Exception err)
            {
                ModernDialog.ShowMessage("Connection failed", "Error", MessageBoxButton.OK);
                MessageBox.Show(err.ToString());
                return;
            }

            try
            {
                if (!App.Server.Authenticate(txtAccount.Text, txtPassword.Password))
                {
                    ModernDialog.ShowMessage("Incorrect user name or password", "Error", MessageBoxButton.OK);
                    return;
                }
            }
            catch (FaultException<AlreadyLoggedInError>)
            {
                ModernDialog.ShowMessage("Your account is already online", "Error", MessageBoxButton.OK);
                return;
            }

            if (App.Server.GetProfile().Role < UserRole.Manager)
            {
                ModernDialog.ShowMessage("This account does not have management access", "Error", MessageBoxButton.OK);
                return;
            }

            App.Server.ChangeToServerMode();

            App.HeartBeatTimer.Start();

            var frame = NavigationHelper.FindFrame(null, this);
            if (frame != null)
            {
                frame.Source = new Uri("/ServerMode/Online.xaml", UriKind.Relative);
            }
        }
    }
}
