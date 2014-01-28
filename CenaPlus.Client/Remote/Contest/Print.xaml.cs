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

namespace CenaPlus.Client.Remote.Contest
{
    /// <summary>
    /// Interaction logic for Print.xaml
    /// </summary>
    public partial class Print : UserControl
    {
        public List<PrintRequestListItem> PrintRequestList = new List<PrintRequestListItem>();
        public Print()
        {
            InitializeComponent();
            for (int i = 1; i <= 10; i++)
            {
                PrintRequestListItem t = new PrintRequestListItem();
                t.ID = i;
                t.Status = PrintStatus.Pending;
                t.Content = "shabi";
                t.Time = DateTime.Now;
                t.Copies = i % 3 + 1;
                PrintRequestList.Add(t);
            }
            PrintRequestListBox.ItemsSource = PrintRequestList;
        }

        private void PrintRequestListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PrintRequestListBox.SelectedItem != null)
            {
                CancelPrintButton.IsEnabled = true;
            }
            else
            {
                CancelPrintButton.IsEnabled = false;
            }
        }

        private void PrintTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PrintRequestListBox.SelectedIndex = -1;
            CancelPrintButton.IsEnabled = false;
        }

        private void CancelPrintButton_Click(object sender, RoutedEventArgs e)
        {
            ModernDialog.ShowMessage("Your request has been canceled.", "Message", MessageBoxButton.OK);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ModernDialog.ShowMessage("Your request has been accepted.", "Message", MessageBoxButton.OK);
        }
    }
    public class PrintRequestListItem
    {
        public int ID { get; set; }
        public int Copies { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public PrintStatus Status { get; set; }
        public string Details
        {
            get
            {
                return String.Format("Length {0}B / {1} Copies / {2}", Content.Length, Copies, Status);
            }
        }
    }
    public enum PrintStatus { Pending, Printed };
}
