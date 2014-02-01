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
using CenaPlus.Server.Bll;
using CenaPlus.Server.Dal;
namespace CenaPlus.Server.ServerMode
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        private CenaPlusServerHost host;

        public Home()
        {
            InitializeComponent();
        }

        private void lstMySqlMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstMySqlMode.SelectedIndex == 1)
            {
                lblMySQLAddr.Visibility = System.Windows.Visibility.Visible;
                lblMySQLPort.Visibility = System.Windows.Visibility.Visible;
                txtMySQLAddr.Visibility = System.Windows.Visibility.Visible;
                txtMySQLPort.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                lblMySQLAddr.Visibility = System.Windows.Visibility.Collapsed;
                lblMySQLPort.Visibility = System.Windows.Visibility.Collapsed;
                txtMySQLAddr.Visibility = System.Windows.Visibility.Collapsed;
                txtMySQLPort.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void btnStartLocal_Click(object sender, RoutedEventArgs e)
        {
            string serverName = txtServerName.Text;

            int localPort;
            if (!int.TryParse(txtLocalPort.Text, out localPort))
            {
                ModernDialog.ShowMessage("Local port must be an integer", "Error", MessageBoxButton.OK);
                return;
            }

            /*
            if (lstMySqlMode.SelectedIndex == -1)
            {
                ModernDialog.ShowMessage("Please select mysql mode", "Error", MessageBoxButton.OK);
                return;
            }

            string connectionString;
            if (lstMySqlMode.SelectedIndex == 0)
            {
                StartEmbeddedMySQL();
                connectionString = "Server = localhost; Port = 3311; Database = cenaplus; Uid = root;";
            }
            else
            {
                ModernDialog.ShowMessage("Not Supported","",MessageBoxButton.OK);
                return;
            }
            App.ConnectionString = connectionString;
             * */

            host = new CenaPlusServerHost(localPort, serverName);
            host.Open();
        }

        private void StartEmbeddedMySQL()
        {
            string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string mySqlPath = Path.Combine(appPath, "mysql");

            bool mySqlRunning = false;
            if (File.Exists(Path.Combine(mySqlPath, "data\\mysqld.pid")))
            {
                mySqlRunning = true;
                // Try to connect mysql
                try
                {
                    Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    sock.Connect(new IPEndPoint(IPAddress.Loopback, 3311));
                    sock.Close();
                }
                catch
                {
                    mySqlRunning = false;
                    File.Delete(Path.Combine(mySqlPath, "data\\mysqld.pid"));
                }
            }

            if (!mySqlRunning)
            {
                Process.Start(new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    FileName = Path.Combine(mySqlPath, "bin\\mysqld.exe"),
                    WorkingDirectory = mySqlPath,
                    WindowStyle = ProcessWindowStyle.Hidden
                });
            }
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
