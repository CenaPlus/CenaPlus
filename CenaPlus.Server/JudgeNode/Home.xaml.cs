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
using FirstFloor.ModernUI.Windows.Navigation;

namespace CenaPlus.Server.JudgeNode
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            var frame = NavigationHelper.FindFrame(null, this);
            if (frame != null)
            {
                frame.Source = new Uri("/JudgeNode/Status.xaml", UriKind.Relative);
            }
        }
    }
}
