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
using CenaPlus.Entity;

namespace CenaPlus.Client.Remote.Contest
{
    /// <summary>
    /// Interaction logic for Records.xaml
    /// </summary>
    public partial class Status : UserControl, IContent
    {
        public Status()
        {
            InitializeComponent();
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            var contestID = int.Parse(e.Fragment);
            var list = from id in App.Server.GetRecordList(contestID)
                       let r = App.Server.GetRecord(id)
                       select new RecordListItem
                       {
                           ID = r.ID,
                           Language = r.Language,
                           MemoryUsage = r.MemoryUsage,
                           ProblemTitle = r.ProblemTitle,
                           Status = r.Status,
                           SubmissionTime = r.SubmissionTime,
                           TimeUsage = r.TimeUsage,
                           UserNickName = r.UserNickName
                       };
            RecordListBox.ItemsSource = list;
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

        class RecordListItem : Record
        {
            private const string STATUS_LINE_FORMT = "User: {0} / {1} / {2} ms / {3} KB @{4}";//Please put the score in the string if it is OI format

            public string StatusColor
            {
                get
                {
                    // TODO: Prettify
                    switch (Status)
                    {
                        case RecordStatus.Accepted:
                            return "Green";
                        case RecordStatus.CompileError:
                            return "Orange";
                        case RecordStatus.Pending:
                            return "Indigo";
                        default:
                            return "Red";
                    }
                }
            }

            public string StatusLine
            {
                get
                {
                    return string.Format(STATUS_LINE_FORMT, UserNickName, ProblemTitle, TimeUsage, MemoryUsage / 1024, SubmissionTime);
                }
            }
        }

        private void RecordListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RecordListBox.SelectedItem != null)
            {
                var frame = NavigationHelper.FindFrame(null, this);
                if (frame != null)
                {
                    frame.Source = new Uri("/Remote/Contest/Hack.xaml#"+(RecordListBox.SelectedItem as RecordListItem).ID, UriKind.Relative);
                }
            }
        }
    }
}
