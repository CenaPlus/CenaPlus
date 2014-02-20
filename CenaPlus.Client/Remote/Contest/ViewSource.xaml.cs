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
using FirstFloor.ModernUI.Windows.Navigation;

namespace CenaPlus.Client.Remote.Contest
{
    /// <summary>
    /// Interaction logic for ViewSource.xaml
    /// </summary>
    public partial class ViewSource : UserControl, IContent
    {
        private int contest_id;
        public ViewSource()
        {
            InitializeComponent();
            RichTextEditor.HighLightEdit.HighLight(txtSource);
        }

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
            var record_id = int.Parse(e.Fragment);
            var record = App.Server.GetRecord(record_id);
            contest_id = App.Server.GetProblemRelatedContest(record.ProblemID);
            txtSource.Document.Blocks.Clear();
            txtSource.Document.Blocks.Add(new Paragraph(new Run(record.Code)));
        }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
        }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {
        }
    }
}
