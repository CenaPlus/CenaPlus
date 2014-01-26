using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstFloor.ModernUI.Windows.Controls;
using CenaPlus.Network;
namespace CenaPlus.Client.Bll
{
    class ServerCallback:ICenaPlusServerCallback
    {
        public void PopAd(string ad)
        {
            ModernDialog.ShowMessage(ad, "Advertisement", System.Windows.MessageBoxButton.OK);
        }
    }
}
