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

namespace CenaPlus.Server.ServerMode.Contest
{
    /// <summary>
    /// Interaction logic for Status.xaml
    /// </summary>
    public partial class Status : UserControl, IContent
    {
        private List<StatusListViewItem> StatusListViewItems = new List<StatusListViewItem>();
        public Status()
        {
            InitializeComponent();
            RichTextEditor.HighLightEdit.HighLight(txtCode);
            StatusListView.ItemsSource = StatusListViewItems;
        }
        private void StatusListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusListView.SelectedItem == null)
            {
                DetailsPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                DetailsPanel.Visibility = Visibility.Visible;
                var record = (Record)StatusListView.SelectedItem;
                txtCode.Document.Blocks.Clear();
                txtCode.Document.Blocks.Add(new Paragraph(new Run(record.Code)));
                txtLanguage.Text = record.Language.ToString();
                txtMemoryUsage.Text = record.MemoryUsage == null ? "?" : (record.MemoryUsage / 1024).ToString() + " KiB";
                txtProblemTitle.Text = record.ProblemTitle;
                txtStatus.Text = record.Status.ToString();
                txtTimeUsage.Text = record.TimeUsage == null ? "?" : record.TimeUsage.ToString() + " ms";
                txtUserNickName.Text = record.UserNickName;
            }
        }
        private void btnRejudge_Click(object sender, RoutedEventArgs e)
        {
            var id = (int)StatusListView.SelectedValue;
            var idx = StatusListView.SelectedIndex;
            App.Server.Rejudge(id);
            Record r = App.Server.GetRecord(id);
            StatusListViewItems[idx] = new StatusListViewItem
            {
                ID = r.ID,
                Language = r.Language,
                MemoryUsage = r.MemoryUsage,
                TimeUsage = r.TimeUsage,
                ProblemTitle = r.ProblemTitle,
                Status = r.Status,
                SubmissionTime = r.SubmissionTime,
                UserNickName = r.UserNickName,
                Code = r.Code
            };
            StatusListView.Items.Refresh();
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            int contestID = int.Parse(e.Fragment);
            var list = from id in App.Server.GetRecordList(contestID)
                       let r = App.Server.GetRecord(id)
                       select new StatusListViewItem
                       {
                           ID = r.ID,
                           Language = r.Language,
                           MemoryUsage = r.MemoryUsage,
                           TimeUsage = r.TimeUsage,
                           ProblemTitle = r.ProblemTitle,
                           Status = r.Status,
                           SubmissionTime = r.SubmissionTime,
                           UserNickName = r.UserNickName,
                           Code = r.Code
                       };
            StatusListViewItems.Clear();
            foreach (var item in list) StatusListViewItems.Add(item);
            StatusListView.Items.Refresh();
            ModernDialog.ShowMessage("Rejudging", "Message", MessageBoxButton.OK);
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
        class StatusListViewItem : Record
        {
        }
    }

}
