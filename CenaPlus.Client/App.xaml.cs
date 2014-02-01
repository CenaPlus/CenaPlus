using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;
using CenaPlus.Client.Bll;

namespace CenaPlus.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static CenaPlusServerProxy Server { get; set; }
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
                    }
                    catch
                    {
                        try { Server.Abort(); }
                        catch { }
                        Server = null;
                        HeartBeatTimer.Stop();
                        MessageBox.Show("Disconnected from the server", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Application.Current.Shutdown();
                    }
                }
                else
                {
                    HeartBeatTimer.Stop();
                }
            };
            HeartBeatTimer.AutoReset = true;
        }
    }
}
