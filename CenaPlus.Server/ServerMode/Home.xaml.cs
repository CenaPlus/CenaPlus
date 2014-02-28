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
    public partial class Home : UserControl
    {
        private CenaPlusServerHost host;
        private const string ConnectionStr = "Server=localhost;Port={0};Database={1};Uid={2};Password={3};Charset=utf8;";

        public Home()
        {
            InitializeComponent();
            txtServerName.Text = ConfigHelper.ServerName;
            txtLocalPort.Text = ConfigHelper.ServerPort.ToString();
        }

        private void lstMySqlMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lblMySQLAddr == null) return;
            if (lstMySqlMode.SelectedIndex == 1)
            {
                lblMySQLAddr.Visibility = System.Windows.Visibility.Visible;
                lblMySQLPort.Visibility = System.Windows.Visibility.Visible;
                lblMySQLDBName.Visibility = System.Windows.Visibility.Visible;
                lblMySQLUsername.Visibility = System.Windows.Visibility.Visible;
                lblMySQLPassword.Visibility = System.Windows.Visibility.Visible;
                txtMySQLAddr.Visibility = System.Windows.Visibility.Visible;
                txtMySQLPort.Visibility = System.Windows.Visibility.Visible;
                txtMySQLDBName.Visibility = System.Windows.Visibility.Visible;
                txtMySQLUsername.Visibility = System.Windows.Visibility.Visible;
                txtMySQLPassword.Visibility = System.Windows.Visibility.Visible;
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
            ConfigHelper.ServerName = txtServerName.Text;
            ConfigHelper.ServerPort = Convert.ToInt32(txtLocalPort.Text);

            string serverName = txtServerName.Text;

            int localPort;
            if (!int.TryParse(txtLocalPort.Text, out localPort))
            {
                ModernDialog.ShowMessage("Local port must be an integer", "Error", MessageBoxButton.OK);
                return;
            }

            if (lstMySqlMode.SelectedIndex == -1)
            {
                ModernDialog.ShowMessage("Please select mysql mode", "Error", MessageBoxButton.OK);
                return;
            }

            string connectionString;
            if (lstMySqlMode.SelectedIndex == 0)
            {
                StartEmbeddedMySQL();
                connectionString = "Server = localhost; Port = 3311; Database = cenaplus; Uid = root; Charset=utf8";
            }
            else
            {
                connectionString = String.Format(ConnectionStr,
                    txtMySQLPort.Text,
                    txtMySQLDBName.Text,
                    txtMySQLUsername.Text,
                    txtMySQLPassword.Password);
            }
            App.ConnectionString = connectionString;

            App.contestmanager.ScheduleAll();

            App.judger.RecordJudgeComplete += App.pushingmanager.JudgeFinished;
            App.judger.HackJudgeComplete += App.pushingmanager.HackFinished;
            App.judger.StartJudgeAllPending();

            LocalCenaServer.ContestModified += App.contestmanager.Reschedule;
            LocalCenaServer.ContestDeleted += App.contestmanager.RemoveSchedule;
            LocalCenaServer.NewRecord += App.pushingmanager.NewRecord;
            LocalCenaServer.NewRecord += App.judger.JudgeRecord;
            LocalCenaServer.NewHack += App.judger.JudgeHack;
            LocalCenaServer.RecordRejudged += App.judger.JudgeRecord;

            host = new CenaPlusServerHost(localPort, serverName);
            host.Open();

            App.Server = new LocalCenaServer { CurrentUser = new FakeSystemUser() };

            if (chkStartJudgeNode.IsChecked == true)
            {
                Random rnd = new Random();
                int port = rnd.Next(2000, 10000);
                Bll.JudgeNode.Password = rnd.NextDouble().ToString();
                var judgeNodeHost = new JudgeNodeHost(port, serverName + " Local");
                judgeNodeHost.Open();

                App.judgenodes.Add(new JudgeNodeInfo
                {
                    Location = new IPEndPoint(IPAddress.Loopback, port),
                    Name = serverName + " Local",
                    Password = Bll.JudgeNode.Password
                });
            }

            var frame = NavigationHelper.FindFrame(null, this);
            if (frame != null)
            {
                frame.Source = new Uri("/ServerMode/Online.xaml", UriKind.Relative);
            }
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
    }
}
