using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Timers;
using System.Threading.Tasks;
using System.Windows;
using System.ServiceModel;
using CenaPlus.Client.Bll;
using CenaPlus.Network;
using FirstFloor.ModernUI.Windows.Controls;
using System.Windows.Threading;
namespace CenaPlus.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ICenaPlusServerChannel Server { get; set; }
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
                        try { Server.Abort(); }
                        catch { }
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
            AppDomain.CurrentDomain.UnhandledException += (obj, evt) => GlobalExceptionHandler(evt.ExceptionObject as Exception);
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
    }
}
