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

namespace CenaPlus.Client.Remote
{
    /// <summary>
    /// Interaction logic for Hack.xaml
    /// </summary>
    public partial class Hack : UserControl
    {
        public Hack()
        {
            InitializeComponent();
        }

        private void HackModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HackModeComboBox.SelectedIndex == 0)
            {
                LanguageComboBox.Visibility = Visibility.Collapsed;
            }
            else if (HackModeComboBox.SelectedIndex == 1)
            {
                LanguageComboBox.Visibility = Visibility.Visible ;
            }
        }
    }
}
