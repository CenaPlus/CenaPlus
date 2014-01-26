using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Net.Sockets;
using System.Net;
namespace CenaPlus.Server
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            string appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
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

        protected override void OnExit(ExitEventArgs e)
        {
            string appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string mySqlPath = Path.Combine(appPath, "mysql");
            Process.Start(new ProcessStartInfo
            {
                Arguments="--user=root --port=3311 shutdown",
                CreateNoWindow = true,
                FileName = Path.Combine(mySqlPath, "bin\\mysqladmin.exe"),
                WorkingDirectory = mySqlPath,
                WindowStyle = ProcessWindowStyle.Hidden
            });
        }
    }
}
