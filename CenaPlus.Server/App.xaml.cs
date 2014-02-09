using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Timers;
using System.Collections.Concurrent;
using FirstFloor.ModernUI.Presentation;
using CenaPlus.Network;
using CenaPlus.Server.Bll;
using CenaPlus.Entity;
namespace CenaPlus.Server
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string ConnectionString;
        public static ICenaPlusServer Server;
        public static Dictionary<int, LocalCenaServer> Clients = new Dictionary<int, LocalCenaServer>();
        public static Timer HeartBeatTimer { get; set; }

        static App()
        {
            HeartBeatTimer = new Timer(10 * 1000);
            HeartBeatTimer.Elapsed += delegate
            {
                if (Server != null)
                {
                    try
                    {
                        Server.GetVersion();
                        HeartBeatTimer.Start();
                    }
                    catch
                    {
                        Server = null;
                        HeartBeatTimer.Stop();
                        MessageBox.Show("Disconnected from the server", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Environment.Exit(1);
                    }
                }
                else
                {
                    HeartBeatTimer.Stop();
                }
            };
            HeartBeatTimer.AutoReset = false;
        }
        #region Global Events
        public event Action<Record> NewRecord;
        #endregion

        protected override void OnStartup(StartupEventArgs e)
        {
            //Theme Init
            AppearanceManager.Current.AccentColor = Color.FromRgb(0x76, 0x60, 0x8a);
            AppearanceManager.Current.ThemeSource = AppearanceManager.DarkThemeSource;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            string appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string mySqlPath = Path.Combine(appPath, "mysql");
            Process.Start(new ProcessStartInfo
            {
                Arguments = "--user=root --port=3311 shutdown",
                CreateNoWindow = true,
                FileName = Path.Combine(mySqlPath, "bin\\mysqladmin.exe"),
                WorkingDirectory = mySqlPath,
                WindowStyle = ProcessWindowStyle.Hidden
            });
        }
    }
}
