using CenaPlus.Entity;
using CenaPlus.Network;
using CenaPlus.Server.Bll;
using FirstFloor.ModernUI.Presentation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using System.Management;
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
        public static List<JudgeNodeInfo> JudgeNodes = new List<JudgeNodeInfo>();
        public static ContestManager ContestManager = new ContestManager();
        public static PushingManager PushingManager = new PushingManager();
        public static Judger Judger = new Judger();
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

        protected override void OnStartup(StartupEventArgs e)
        {
            //Theme Init
            AppearanceManager.Current.AccentColor = Color.FromRgb(0x76, 0x60, 0x8a);

            AppDomain.CurrentDomain.UnhandledException += (obj, evt) => GlobalExceptionHandler(evt.ExceptionObject as Exception);
       
            //Judge cores init
            int count = 0;
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                count += int.Parse(item["NumberOfCores"].ToString());
            }
            for (int i = 0; i < count; i++)
            {
                Judge.Core c = new Judge.Core();
                c.Index = i;
                c.CurrentTask = null;
                Judge.Env.Cores.Add(c);
            }
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            GlobalExceptionHandler(e.Exception);
        }

        private void GlobalExceptionHandler(Exception exception)
        {
            string message;
            if (exception is FaultException)
            {
                message = "The server refused our request. This is very likely to be a logical error in this program.";
            }
            else if (exception is CommunicationException)
            {
                message = "An error occured while communicating with the server";
            }
            else
            {
                message = "The application crashed due to an internal exception";
            }
            MessageBox.Show(message, "Crashed", MessageBoxButton.OK, MessageBoxImage.Error);

            try
            {
                Clipboard.SetText(exception.ToString());
                MessageBox.Show("The details of this crash is now in your clipboard. Please report them to us. Thanks.", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show(exception.ToString(), "Crash details", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            Environment.Exit(1);
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
