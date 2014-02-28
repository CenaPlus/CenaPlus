using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Printing;
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
using System.IO;
using System.Printing;
using System.Windows.Xps;
using CenaPlus.Server.Bll;

namespace CenaPlus.Server.ServerMode.Contest
{
    /// <summary>
    /// Interaction logic for Print.xaml
    /// </summary>
    public partial class Print : UserControl, IContent
    {
        private List<PrintRequestListItem> requestList = new List<PrintRequestListItem>();
        private int contestID;
        public Print()
        {
            InitializeComponent();
            PrintRequestListBox.ItemsSource = requestList;
            LocalCenaServer.NewPrintRequest += this.NewPrintRequest;
            App.RemoteCallback.OnNewPrint += this.NewPrintRequest;
            LocalCenaServer.PrintRequestUpdated += this.PrintRequestUpdated;
            App.RemoteCallback.OnPrintUpdated += this.PrintRequestUpdated;
            LocalCenaServer.PrintRequestDeleted += this.PrintRequestDeleted;
            App.RemoteCallback.OnPrintDeleted += this.PrintRequestDeleted;
        }
        public void PrintRequestDeleted(int request_id)
        {
            var requestindex = requestList.FindIndex(x => x.ID == request_id);
            if (requestindex == -1) return;
            Dispatcher.Invoke(new Action(() => {
                requestList.RemoveAt(requestindex);
                PrintRequestListBox.Items.Refresh();
            }));
        }
        public void PrintRequestUpdated(int request_id)
        {
            var requestindex = requestList.FindIndex(x => x.ID == request_id);
            if (requestindex == -1) return;
            var r = App.Server.GetPrintRequest(request_id);
            if (r == null) return;
            if (r.ContestID != contestID) return;
            var item = new PrintRequestListItem
            {
                ID = r.ID,
                Copies = r.Copies,
                Content = r.Content,
                Time = r.Time,
                Status = r.Status,
                UserNickName = r.UserNickName
            };
            Dispatcher.Invoke(new Action(() =>
            {
                requestList[requestindex] = item;
                PrintRequestListBox.Items.Refresh();
            }));
        }
        public void NewPrintRequest(int request_id)
        {
            var r = App.Server.GetPrintRequest(request_id);
            var item = new PrintRequestListItem
            {
                ID = r.ID,
                Copies = r.Copies,
                Content = r.Content,
                Time = r.Time,
                Status = r.Status,
                UserNickName = r.UserNickName
            };
            Dispatcher.Invoke(new Action(() =>
            {
                requestList.Add(item);
                PrintRequestListBox.Items.Refresh();
            }));
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
                var request = (PrintRequestListItem)PrintRequestListBox.SelectedItem;
                txtPrintContent.Text = request.Content;
                txtCopies.Text = request.Copies.ToString();
            }
        }

        private void btnSavePrint_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)PrintRequestListBox.SelectedValue;

            if (App.Server.GetPrintRequest(id).Status != PrintRequestStatus.Pending)
            {
                ModernDialog.ShowMessage("This material has been already printed.", "Message", MessageBoxButton.OK);
                return;
            }

            int copies;
            if (!int.TryParse(txtCopies.Text, out copies))
            {
                ModernDialog.ShowMessage("Copies should be an integer", "Error", MessageBoxButton.OK);
                return;
            }

            App.Server.UpdatePrintRequest(id, txtPrintContent.Text, copies, PrintRequestStatus.Printing);

            var request = (PrintRequestListItem)PrintRequestListBox.SelectedItem;
            request.Copies = copies;
            request.Content = txtPrintContent.Text;

            string docName = string.Format("{1}@{2}(len:{0})", request.Content.Length, request.UserNickName, request.Time.ToShortTimeString());

            var user = App.Server.GetUser(request.UserID);
            PrintPlaintext(user.Name + "(" + user.NickName + ")\r\n" + request.Content, docName, request.Copies);

            App.Server.UpdatePrintRequest(id, null, null, PrintRequestStatus.Done);

            var r = App.Server.GetPrintRequest(id);
            requestList[PrintRequestListBox.SelectedIndex] = new PrintRequestListItem
            {
                ID = r.ID,
                Copies = r.Copies,
                Content = r.Content,
                Time = r.Time,
                Status = r.Status,
                UserNickName = r.UserNickName
            };
            PrintRequestListBox.Items.Refresh();
            ModernDialog.ShowMessage("Printing finished", "Message", MessageBoxButton.OK);
        }

        private void PrintPlaintext(string text, string docName, int copies)
        {
            // Clone the source document's content into a new FlowDocument.
            FlowDocument flowDocumentCopy = new FlowDocument();
            flowDocumentCopy.Blocks.Add(new Paragraph(new Run(text)));

            // Create a XpsDocumentWriter object, open a Windows common print dialog.
            // This methods returns a ref parameter that represents information about the dimensions of the printer media.
            PrintDocumentImageableArea ia = null;
            XpsDocumentWriter docWriter = PrintQueue.CreateXpsDocumentWriter(ref ia);

            if (docWriter == null || ia == null)
                throw new NullReferenceException();

            DocumentPaginator paginator = ((IDocumentPaginatorSource)flowDocumentCopy).DocumentPaginator;

            // Change the PageSize and PagePadding for the document to match the CanvasSize for the printer device.
            paginator.PageSize = new Size(ia.MediaSizeWidth, ia.MediaSizeHeight);
            Thickness pagePadding = flowDocumentCopy.PagePadding;
            flowDocumentCopy.PagePadding = new Thickness(
                    Math.Max(ia.OriginWidth, pagePadding.Left),
                    Math.Max(ia.OriginHeight, pagePadding.Top),
                    Math.Max(ia.MediaSizeWidth - (ia.OriginWidth + ia.ExtentWidth), pagePadding.Right),
                    Math.Max(ia.MediaSizeHeight - (ia.OriginHeight + ia.ExtentHeight), pagePadding.Bottom));
            flowDocumentCopy.ColumnWidth = double.PositiveInfinity;

            PrintDialog dialog = new PrintDialog();
            for (int i = 0; i < copies; i++) dialog.PrintDocument(paginator, "asfd");
        }

        private void btnReject_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)PrintRequestListBox.SelectedValue;
            App.Server.UpdatePrintRequest(id, null, null, PrintRequestStatus.Rejected);
            requestList[PrintRequestListBox.SelectedIndex].Status = PrintRequestStatus.Rejected;
            PrintRequestListBox.Items.Refresh();
            ModernDialog.ShowMessage("Done!", "Message", MessageBoxButton.OK);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)PrintRequestListBox.SelectedValue;

            int copies;
            if (!int.TryParse(txtCopies.Text, out copies))
            {
                ModernDialog.ShowMessage("Copies should be an integer", "Error", MessageBoxButton.OK);
                return;
            }

            App.Server.UpdatePrintRequest(id, txtPrintContent.Text, copies, null);
            var r = App.Server.GetPrintRequest(id);
            requestList[PrintRequestListBox.SelectedIndex] = new PrintRequestListItem
            {
                ID = r.ID,
                Copies = r.Copies,
                Content = r.Content,
                Time = r.Time,
                Status = r.Status,
                UserNickName = r.UserNickName
            };
            PrintRequestListBox.Items.Refresh();
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
                           Status = r.Status,
                           UserNickName = r.UserNickName
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
                    return String.Format("Length: {0}B / Copies: {1} @{2} / {3}", Content.Length, Copies, Time, Status);
                }
            }
        }
    }
}