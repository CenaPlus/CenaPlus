using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Presentation;
using Microsoft.Win32;
using CenaPlus.Entity;
namespace CenaPlus.Server.ServerMode.Contest.Problem
{
    /// <summary>
    /// Interaction logic for TestCases.xaml
    /// </summary>
    public partial class TestCases : UserControl, IContent
    {
        private int problemID;
        private List<TestCaseListItem> TestCasesListItems = new List<TestCaseListItem>();
        public TestCases()
        {
            InitializeComponent();
            TestCasesListView.ItemsSource = TestCasesListItems;
            cbType.ItemsSource = Enum.GetNames(typeof(TestCaseType));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)TestCasesListView.SelectedValue;
            App.Server.DeleteTestCase(id);
            for (int i = TestCasesListView.SelectedIndex + 1; i < TestCasesListItems.Count; i++)
            {
                TestCasesListItems[i].Index--;
            }
            TestCasesListItems.RemoveAt(TestCasesListView.SelectedIndex);
            TestCasesListView.Items.Refresh();
        }

        private void btnBatch_Click(object sender, RoutedEventArgs e)
        {
            TestCasesListView.SelectedIndex = -1;
            SinglePanel.Visibility = Visibility.Collapsed;
            BatchPanel.Visibility = Visibility.Visible;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            SinglePanel.Visibility = Visibility.Visible;
            BatchPanel.Visibility = Visibility.Collapsed;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            int id = App.Server.CreateTestCase(problemID, new byte[0], new byte[0], TestCaseType.Pretest);
            TestCasesListItems.Add(new TestCaseListItem
            {
                Index = TestCasesListItems.Count,
                ID = id,
                InputPreview = "",
                InputSize = 0,
                OutputPreview = "",
                OutputSize = 0,
                Type = TestCaseType.Pretest
            });
            TestCasesListView.Items.Refresh();
            TestCasesListView.SelectedIndex = TestCasesListItems.Count - 1;
        }

        private void TestCasesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TestCasesListView.SelectedItem == null)
            {
                SinglePanel.Visibility = Visibility.Collapsed;
                BatchPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                SinglePanel.Visibility = Visibility.Visible;
                BatchPanel.Visibility = Visibility.Collapsed;
                var testCase = (TestCase)TestCasesListView.SelectedItem;
                txtInputPreview.Text = testCase.InputPreview;
                txtOutputPreview.Text = testCase.OutputPreview;
                cbType.SelectedIndex = (int)testCase.Type;
                txtInputFileName.Text = "";
                txtOutputFileName.Text = "";
            }
        }
        private void btnInputFileDialog_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog() { Title = "Select Input File" };
            if (dialog.ShowDialog() != true)
                return;

            txtInputFileName.Text = dialog.FileName;

            char[] buffer = new char[100];
            int length;
            using (var reader = File.OpenText(dialog.FileName))
            {
                length = reader.Read(buffer, 0, buffer.Length);
            }
            txtInputPreview.Text = new string(buffer, 0, length);
        }
        private void btnOutputFileDialog_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog() { Title = "Select Output File" };
            if (dialog.ShowDialog() != true)
                return;

            txtOutputFileName.Text = dialog.FileName;

            char[] buffer = new char[100];
            int length;
            using (var reader = File.OpenText(dialog.FileName))
            {
                length = reader.Read(buffer, 0, buffer.Length);
            }
            txtOutputPreview.Text = new string(buffer, 0, length);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            byte[] input = txtInputFileName.Text == "" ? null : File.ReadAllBytes(txtInputFileName.Text);
            byte[] output = txtOutputFileName.Text == "" ? null : File.ReadAllBytes(txtOutputFileName.Text);
            App.Server.UpdateTestCase((int)TestCasesListView.SelectedValue, input, output, (TestCaseType)cbType.SelectedIndex);

            var t = (TestCaseListItem)TestCasesListView.SelectedItem;
            if (input != null)
            {
                t.InputSize = input.Length;
                t.InputPreview = Encoding.UTF8.GetString(input.Take(100).ToArray());
            }
            if (output != null)
            {
                t.OutputSize = output.Length;
                t.OutputPreview = Encoding.UTF8.GetString(output.Take(100).ToArray());
            }
            TestCasesListView.Items.Refresh();
            txtInputFileName.Text = "";
            txtOutputFileName.Text = "";
            ModernDialog.ShowMessage("Saved", "Message", MessageBoxButton.OK);
        }


        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            problemID = int.Parse(e.Fragment);
            var list = from id in App.Server.GetTestCaseList(problemID)
                       let t = App.Server.GetTestCase(id)
                       select new TestCaseListItem
                       {
                           ID = t.ID,
                           InputPreview = t.InputPreview,
                           InputSize = t.InputSize,
                           OutputPreview = t.OutputPreview,
                           OutputSize = t.OutputSize,
                           Type = t.Type
                       };
            TestCasesListItems.Clear();
            TestCasesListItems.AddRange(list);
            for (int i = 0; i < TestCasesListItems.Count; i++)
            {
                TestCasesListItems[i].Index = i;
            }
            TestCasesListView.Items.Refresh();

            var contestID = App.Server.GetProblem(problemID).ContestID;
            if (new[] { ContestType.Codeforces, ContestType.TopCoder }.Contains(App.Server.GetContest(contestID).Type))
            {
                lblType.Visibility = System.Windows.Visibility.Visible;
                cbType.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                lblType.Visibility = System.Windows.Visibility.Collapsed;
                cbType.Visibility = System.Windows.Visibility.Collapsed;
            }
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

        class TestCaseListItem : TestCase
        {
            public int Index { get; set; }
            public string InputLength { get { return InputSize + " b"; } }
            public string OutputLength { get { return OutputSize + " b"; } }
        }



    }

}
