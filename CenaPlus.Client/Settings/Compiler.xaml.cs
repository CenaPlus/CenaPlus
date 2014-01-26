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
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;

namespace CenaPlus.Client.Settings
{
    /// <summary>
    /// Interaction logic for Compiler.xaml
    /// </summary>
    public partial class Compiler : UserControl
    {
        public Compiler()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ModernDialog.ShowMessage("The compiler arguments have been saved.", "Message", System.Windows.MessageBoxButton.OK);
        }
    }
}
