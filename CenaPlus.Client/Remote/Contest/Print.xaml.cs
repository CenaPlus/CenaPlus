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
using System.Windows.Shapes;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using FirstFloor.ModernUI.Windows.Controls;
using CenaPlus.Entity;

namespace CenaPlus.Client.Remote.Contest
{
    /// <summary>
    /// Interaction logic for Print.xaml
    /// </summary>
    public partial class Print : UserControl, IContent
    {
        private int contestID;
        private List<PrintRequestListItem> requestList = new List<PrintRequestListItem>();
        public Print()
        {
            InitializeComponent();
            PrintRequestListBox.ItemsSource = requestList;
        }

        private void PrintRequestListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PrintRequestListBox.SelectedItem != null)
            {
                var request = (PrintRequestListItem)PrintRequestListBox.SelectedItem;
                btnPrint.Visibility = System.Windows.Visibility.Collapsed;
                btnSave.Visibility = System.Windows.Visibility.Visible;
                if (request.Status == PrintRequestStatus.Pending)
                {
                    CancelPrintButton.IsEnabled = true;
                    btnSave.IsEnabled = true;
                    txtCopies.IsReadOnly = false;
                    PrintTextBox.IsReadOnly = false;
                }
                else
                {
                    CancelPrintButton.IsEnabled = false;
                    btnSave.IsEnabled = false;
                    txtCopies.IsReadOnly = true;
                    PrintTextBox.IsReadOnly = true;
                }
                PrintTextBox.Text = request.Content;
                txtCopies.Text = request.Copies.ToString();
            }
        }

        private void CancelPrintButton_Click(object sender, RoutedEventArgs e)
        {
            App.Server.DeletePrintRequest((int)PrintRequestListBox.SelectedValue);
            requestList.RemoveAt(PrintRequestListBox.SelectedIndex);
            PrintRequestListBox.Items.Refresh();
            ModernDialog.ShowMessage("The print request has been canceled.", "Message", MessageBoxButton.OK);
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            int copies;
            if (!int.TryParse(txtCopies.Text, out copies))
            {
                ModernDialog.ShowMessage("Copies should be an integer", "Error", MessageBoxButton.OK);
                return;
            }
            int id = App.Server.RequestPrinting(contestID, PrintTextBox.Text, copies);
            requestList.Add(new PrintRequestListItem
            {
                ID = id,
                Copies = copies,
                Content = PrintTextBox.Text,
                Time = DateTime.Now,
                Status = PrintRequestStatus.Pending
            });
            PrintRequestListBox.Items.Refresh();
            ModernDialog.ShowMessage("Your request has been accepted.", "Message", MessageBoxButton.OK);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int copies;
            if (!int.TryParse(txtCopies.Text, out copies))
            {
                ModernDialog.ShowMessage("Copies should be an integer", "Error", MessageBoxButton.OK);
                return;
            }
            App.Server.UpdatePrintRequest((int)PrintRequestListBox.SelectedValue, PrintTextBox.Text, copies, null);
            var request = requestList[PrintRequestListBox.SelectedIndex];
            request.Content = PrintTextBox.Text;
            request.Copies = copies;
            PrintRequestListBox.Items.Refresh();
            ModernDialog.ShowMessage("Your request has been saved.", "Message", MessageBoxButton.OK);
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            txtCopies.Text = "";
            PrintTextBox.Text = "";
            txtCopies.IsReadOnly = false;
            PrintTextBox.IsReadOnly = false;
            PrintRequestListBox.SelectedIndex = -1;
            CancelPrintButton.IsEnabled = false;
            btnPrint.Visibility = System.Windows.Visibility.Visible;
            btnSave.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            contestID = int.Parse(e.Fragment);
            var list = from id in App.Server.GetPrintRequestList(contestID)
                       let r = App.Server.GetPrintRequest(id)
                       select new PrintRequestListItem
                       {
                           ID = r.ID,
                           Copies = r.Copies,
                           Content = r.Content,
                           Time = r.Time,
                           Status = r.Status
                       };
            requestList.Clear();
            requestList.AddRange(list);
            PrintRequestListBox.Items.Refresh();
        }

        public void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
        }
        class PrintRequestListItem : PrintRequest
        {
            public string Details
            {
                get
                {
                    return String.Format("Length {0} B / {1} Copies / {2}", Content.Length, Copies, Status);
                }
            }
        }

    }
}
