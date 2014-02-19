using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for Problem_Submit.xaml
    /// </summary>
    public partial class Submit : UserControl, IContent
    {
        private int problemID;

        public Submit()
        {
            InitializeComponent();
            lstLanguage.ItemsSource =  Enum.GetNames(typeof(ProgrammingLanguage));
            RichTextEditor.HighLightEdit.HighLight(txtCode);
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            problemID = int.Parse(e.Fragment);
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

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            int recordID = App.Server.Submit(problemID,  new TextRange(txtCode.Document.ContentStart, txtCode.Document.ContentEnd).Text , (ProgrammingLanguage)Enum.Parse(typeof(ProgrammingLanguage), (string)lstLanguage.SelectedItem));
            var problem = App.Server.GetProblem(problemID);
            var frame = NavigationHelper.FindFrame(null, this);
            if (frame != null)
            {
                Thread.Sleep(200);
                frame.Source = new Uri("/Remote/Contest/ProblemGeneral.xaml#" + problem.ContestID, UriKind.Relative);
            }
        }
    }
}
