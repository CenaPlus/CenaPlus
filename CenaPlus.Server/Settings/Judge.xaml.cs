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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FirstFloor.ModernUI.Windows.Controls;

namespace CenaPlus.Server.Settings
{
    /// <summary>
    /// Interaction logic for Judge.xaml
    /// </summary>
    public partial class Judge : UserControl
    {
        public Judge()
        {
            InitializeComponent();
            txtPath.Text = Bll.ConfigHelper.WorkingDirectory;
            txtWinUser.Text = Bll.ConfigHelper.UserName;
            txtWinPwd.Text = Bll.ConfigHelper.Password;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtWinUser.Text == String.Empty || txtWinPwd.Text == String.Empty)
            {
                ModernDialog.ShowMessage("Win user cannot be null.", "Error", MessageBoxButton.OK);
                return;
            }
            Bll.ConfigHelper.WorkingDirectory = txtPath.Text;
            Bll.ConfigHelper.UserName = txtWinUser.Text;
            Bll.ConfigHelper.Password = txtWinPwd.Text;
            ModernDialog.ShowMessage("Configuration has been saved.", "Message", MessageBoxButton.OK);
        }
    }
}
