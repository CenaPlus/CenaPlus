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

        public Home()
        {
            InitializeComponent();
        }

        private void lstMySqlMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lblMySQLAddr == null) return;
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
        private void JudgeFinished(int record_id)
        {
            using (DB db = new DB())
            {
                Record record = (from r in db.Records
                            where r.ID == record_id
                            select r).FirstOrDefault();
                Result re = new Result() 
                { 
                    StatusID=record.ID,
                    Status = record.Status,
                    TimeUsage = record.TimeUsage,
                    MemoryUsage = record.MemoryUsage,
                    UserID = record.UserID,
                    SubmissionTime = record.SubmissionTime,
                    UserNickName = record.UserNickName,
                    Detail = record.Detail,
                    Language = record.Language,
                    ProblemTitle = record.Problem.Title
                };
                var Type = record.Problem.Contest.Type;
                if (Type == ContestType.OI) return;
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    LocalCenaServer client;
                    if (App.Clients.TryGetValue(record.UserID, out client))
                    {
                        client.Callback.JudgeFinished(re);
                    }
                });
                if (StandingsCache.Standings[record.Problem.ContestID] == null)
                {
                    StandingsCache.Rebuild(record.Problem.ContestID);
                }
                var userindex= (StandingsCache.Standings[record.Problem.ContestID] as List<Entity.StandingItem>).FindIndex(x=>x.UserID == record.UserID);
                if (userindex == -1)
                {
                    StandingsCache.UpdateSingleUser(record.UserID, record.Problem.ContestID, (from p in db.Problems where p.ContestID == record.Problem.ContestID select p.ID).ToList());
                }
                else
                {
                    StandingsCache.UpdateSingleDetail(record.UserID, record.ProblemID, record.Problem.ContestID, Type);
                }
                foreach (var c in App.Clients.Values)
                {
                    System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                        c.Callback.StandingsPush(record.Problem.ContestID, (StandingsCache.Standings[record.Problem.ContestID] as List<Entity.StandingItem>).Find(x => x.UserID == record.UserID));
                    });
                }
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
                ModernDialog.ShowMessage("Not Supported", "", MessageBoxButton.OK);
                return;
            }
            App.ConnectionString = connectionString;

            var contestManager = new ContestManager();
            contestManager.ScheduleAll();

            var judger = new Judger();
            judger.RecordJudgeComplete += JudgeFinished;
            judger.StartJudgeAllPending();

            LocalCenaServer.ContestModified += contestManager.Reschedule;
            LocalCenaServer.ContestDeleted += contestManager.RemoveSchedule;
            LocalCenaServer.NewRecord += judger.JudgeRecord;
            LocalCenaServer.NewHack += judger.JudgeHack;
            LocalCenaServer.RecordRejudged += judger.JudgeRecord;

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

                App.JudgeNodes.Add(new JudgeNodeInfo
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
                App.Server = CenaPlusServerChannelFactory.CreateChannel(new IPEndPoint(address, port), new Bll.ServerCallback());
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

            App.HeartBeatTimer.Start();

            var frame = NavigationHelper.FindFrame(null, this);
            if (frame != null)
            {
                frame.Source = new Uri("/ServerMode/Online.xaml", UriKind.Relative);
            }
        }
    }
}
