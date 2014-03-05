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
using CenaPlus.Server.Bll;

namespace CenaPlus.Server.ServerMode.Contest
{
    /// <summary>
    /// Interaction logic for Status.xaml
    /// </summary>
    public partial class Status : UserControl, IContent
    {
        private List<StatusListViewItem> StatusListViewItems = new List<StatusListViewItem>();
        private List<StatusListViewItem> QueryItems = new List<StatusListViewItem>();
        private int contestID;
        public Status()
        {
            InitializeComponent();
            RichTextEditor.HighLightEdit.HighLight(txtCode);
            StatusListView.ItemsSource = StatusListViewItems;
            LocalCenaServer.NewRecord += id => this.NewRecord(App.Server.GetRecord(id));
            App.RemoteCallback.OnNewRecord += this.NewRecord;
            App.judger.RecordJudgeComplete += this.RecordUpdated;
            App.RemoteCallback.OnRecordUpdated += this.RecordUpdated;
        }
        public void NewRecord(Record r)
        {
            if (r == null) return;
            if (App.Server.GetProblemRelatedContest(r.ProblemID) != contestID) return;
            var item = new StatusListViewItem
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
            Dispatcher.Invoke(new Action(() =>
            {
                StatusListViewItems.Insert(0, item);
                StatusListView.Items.Refresh();
            }));
        }
        public void RecordUpdated(int record_id)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() => {
                var r = App.Server.GetRecord(record_id);
                if (r == null) return;
                int recordindex = StatusListViewItems.FindIndex(x => x.ID == record_id);
                if (recordindex == -1) return;
                var item = new StatusListViewItem
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
                Dispatcher.Invoke(new Action(() =>
                {
                    StatusListViewItems[recordindex] = item;
                    StatusListView.Items.Refresh();
                }));
                recordindex = QueryItems.FindIndex(x => x.ID == record_id);
                if (recordindex == -1) return;
                Dispatcher.Invoke(new Action(() =>
                {
                    QueryItems[recordindex] = item;
                    StatusListView.Items.Refresh();
                }));
            });
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
                tbDetail.Text = record.Detail;
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
        public void Query()
        { 
            bool Filter_Problem = false, Filter_Status = false, Filter_User = false;
            if (lstProblem.SelectedIndex != 0) Filter_Problem = true;
            if (lstStatus.SelectedIndex != 0) Filter_Status = true;
            if (!string.IsNullOrEmpty(txtName.Text)) Filter_User = true;
            StatusListView.ItemsSource = (from r in StatusListViewItems
                                          where r.ProblemTitle == (Filter_Problem ? lstProblem.SelectedItem.ToString() : r.ProblemTitle)
                                          && r.Status.ToString() == (Filter_Status ? lstStatus.SelectedItem.ToString() : r.Status.ToString())
                                          && r.UserNickName == (Filter_User ? txtName.Text : r.UserNickName)
                                          select r).ToList();
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            contestID = int.Parse(e.Fragment);
            var pids = App.Server.GetProblemList(contestID);
            lstProblem.Items.Add("All");
            foreach (var pid in pids)
            {
                lstProblem.Items.Add(App.Server.GetProblem(pid).Title);
            }
            lstProblem.Items.Refresh();
            lstProblem.SelectedIndex = 0;
            var statusstr = Enum.GetNames(typeof(Entity.RecordStatus));
            lstStatus.Items.Add("All");
            foreach (var status in statusstr)
            {
                lstStatus.Items.Add(status);
            }
            lstStatus.Items.Refresh();
            lstStatus.SelectedIndex = 0;
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
                           Detail = r.Detail == null ? null : r.Detail.Trim('\r').Trim('\n'),
                           SubmissionTime = r.SubmissionTime,
                           UserNickName = r.UserNickName,
                           Code = r.Code
                       };
            StatusListViewItems.Clear();
            foreach (var item in list) StatusListViewItems.Add(item);
            StatusListView.Items.Refresh();
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
        private void btnRejudgeAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < StatusListViewItems.Count(); i++)
            {
                StatusListViewItems[i].Status = RecordStatus.Running;
            }
            StatusListView.Items.Refresh();
            App.Server.RejudgeAll(contestID);
            ModernDialog.ShowMessage("Rejudging.", "Message", MessageBoxButton.OK);
        }

        private void btnRejudgeAllAC_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < StatusListViewItems.Count(); i++)
            {
                if (StatusListViewItems[i].Status == RecordStatus.Accepted)
                    StatusListViewItems[i].Status = RecordStatus.Running;
            }
            StatusListView.Items.Refresh();
            App.Server.SystemTest(contestID);
            ModernDialog.ShowMessage("System testing.", "Message", MessageBoxButton.OK);
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            Query();
        }

        private void btnRejudgePage_Click(object sender, RoutedEventArgs e)
        {
            bool Filter_Problem = false, Filter_Status = false, Filter_User = false;
            if (lstProblem.SelectedIndex != 0) Filter_Problem = true;
            if (lstStatus.SelectedIndex != 0) Filter_Status = true;
            if (!string.IsNullOrEmpty(txtName.Text)) Filter_User = true;
            QueryItems.Clear();
            QueryItems = (from r in StatusListViewItems
                                          where r.ProblemTitle == (Filter_Problem ? lstProblem.SelectedItem.ToString() : r.ProblemTitle)
                                          && r.Status.ToString() == (Filter_Status ? lstStatus.SelectedItem.ToString() : r.Status.ToString())
                                          && r.UserNickName == (Filter_User ? txtName.Text : r.UserNickName)
                                          select r).ToList();
            for (int i = 0; i < QueryItems.Count(); i++)
            {
                QueryItems[i].Status = RecordStatus.Pending;
            }
            StatusListView.ItemsSource = QueryItems;
            App.Server.RejudgeSet((from id in QueryItems select id.ID).ToList());
        }
    }
}
