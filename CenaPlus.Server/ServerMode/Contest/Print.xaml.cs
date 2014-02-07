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

namespace CenaPlus.Server.ServerMode.Contest
{
    /// <summary>
    /// Interaction logic for Print.xaml
    /// </summary>
    public partial class Print : UserControl
    {
        public List<ServerPrintRequestListBoxItem> ServerPrintRequestListBoxItems = new List<ServerPrintRequestListBoxItem>();
        public Print()
        {
            InitializeComponent();
            for (int i = 0; i < 10; i++)
            {
                ServerPrintRequestListBoxItem t = new ServerPrintRequestListBoxItem();
                t.Username = "yuno";
                t.Time = DateTime.Now;
                t.Copies = 3;
                t.Length = 123;
                ServerPrintRequestListBoxItems.Add(t);
            }
            PrintRequestListBox.ItemsSource = ServerPrintRequestListBoxItems;
        }

        private void PrintRequestListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gridPrintContent == null) return;
            if (PrintRequestListBox.SelectedItem == null)
            {
                gridPrintContent.Visibility = Visibility.Collapsed;
            }
            else
            {
                gridPrintContent.Visibility = Visibility.Visible;
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            ModernDialog.ShowMessage("Printing.", "Message", MessageBoxButton.OK);
        }
    }
    public class ServerPrintRequestListBoxItem
    {
        public string Username { get; set; }
        public int Copies { get; set; }
        public int Length { get; set; }
        public DateTime Time { get; set; }
        public string Details
        {
            get
            {
                return String.Format("Length: {0}B / Copies: {1} @{2}", Length, Copies, Time);
            }
        }
    }
}
