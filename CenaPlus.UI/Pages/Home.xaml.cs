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
using FirstFloor.ModernUI.Windows.Controls;

namespace CenaPlus.UI.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public static bool Logged=true;
        public Home()
        {
            InitializeComponent();
            if (Logged)
            {
                var frame = NavigationHelper.FindFrame(null, this);
                if (frame != null)
                {
                    frame.Source = new Uri("/Content/Compiler.xaml", UriKind.Relative);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ModernDialog.ShowMessage("Cannot connect to the target server.", "Message", System.Windows.MessageBoxButton.OK);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var frame = NavigationHelper.FindFrame(null, this);
            if (frame != null)
            {
                frame.Source = new Uri("/Content/Compiler.xaml", UriKind.Relative);
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            if (Logged)
            {
                var frame = NavigationHelper.FindFrame(null, this);
                if (frame != null)
                {
                    //frame.Source = new Uri("/Pages/Main.xaml", UriKind.Relative);
                }
            }
        }
    }
}
