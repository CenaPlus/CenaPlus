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
using mshtml;
using CenaPlus.Server.Bll;
using CenaPlus.Server.Dal;
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

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            if (host == null)
            {
                host = new CenaPlusServerHost(Convert.ToInt32(CenaPlusPortTextBox.Text), HostnameTextBox.Text);
                host.Open();
                btnTest.Content = "Click to stop";
            }
            else
            {
                host.Close();
                host = null;
                btnTest.Content = "Click to start";
            }
        }

        private void ModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ModeComboBox.SelectedIndex > 0)
            {
                spFullService.Visibility = Visibility.Collapsed;
                spJudgeNode.Visibility = Visibility.Visible;
            }
            else
            {
                spFullService.Visibility = Visibility.Visible;
                spJudgeNode.Visibility = Visibility.Collapsed;
            }
        }
    }
}
