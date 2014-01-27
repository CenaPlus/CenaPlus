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
    public partial class Records : UserControl, IContent
    {
        public Records()
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
                           UserName = r.UserName
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
            private const string STATUS_LINE_FORMT = "User: {0} / {1} / {2} ms / {3} KB @{4}";

            public string StatusColor
            {
                get
                {
                    // TODO: Prettify
                    switch (Status)
                    {
                        case RecordStatus.Accepted:
                            return "Green";
                        default:
                            return "Red";
                    }
                }
            }

            public string StatusLine
            {
                get
                {
                    return string.Format(STATUS_LINE_FORMT, UserName, ProblemTitle, TimeUsage, MemoryUsage / 1024, SubmissionTime);
                }
            }
        }
    }
}
