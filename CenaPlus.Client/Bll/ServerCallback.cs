using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using CenaPlus.Network;
using CenaPlus.Entity;
namespace CenaPlus.Client.Bll
{
    class ServerCallback : ICenaPlusServerCallback
    {
        public void Bye()
        {
            ModernDialog.ShowMessage("You are kicked out.", "Message", MessageBoxButton.OK);
            Environment.Exit(1);
        }

        public void QuestionUpdated(Question question)
        {
            string msg = string.Format("The status of '{0}' has been updated!", question.Description);
            ModernDialog.ShowMessage(msg, "Message", MessageBoxButton.OK);
        }
    }
}
