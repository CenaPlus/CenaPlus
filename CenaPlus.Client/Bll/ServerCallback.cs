using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using CenaPlus.Network;
namespace CenaPlus.Client.Bll
{
    class ServerCallback:ICenaPlusServerCallback
    {
        public void Bye()
        {
            ModernDialog.ShowMessage("You are kicked out.", "Message", MessageBoxButton.OK);
            Application.Current.Shutdown();
        }
    }
}
