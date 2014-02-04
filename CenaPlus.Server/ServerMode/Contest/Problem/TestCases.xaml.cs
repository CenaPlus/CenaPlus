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

namespace CenaPlus.Server.ServerMode.Contest.Problem
{
    /// <summary>
    /// Interaction logic for TestCases.xaml
    /// </summary>
    public partial class TestCases : UserControl
    {
        public List<TestCasesListItem> TestCasesListItems = new List<TestCasesListItem>();
        public TestCases()
        {
            InitializeComponent();
            for (int i = 0; i < 10; i++)
            {
                TestCasesListItem t = new TestCasesListItem();
                t.Index = i;
                t.InputSize = "1 MB";
                t.OutputSize = "234 KB";
                t.Status = TestCaseStatus.SystemTest;
                TestCasesListItems.Add(t);
            }
            TestCasesListView.ItemsSource = TestCasesListItems;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            TestCasesListView.SelectedIndex = -1;
            SinglePanel.Visibility = Visibility.Collapsed;
            BatchPanel.Visibility = Visibility.Collapsed;
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
            //TODO: Create first and turn into the edit mode
            TestCasesListView.SelectedIndex = -1;//turn to the created index
            SinglePanel.Visibility = Visibility.Visible;
            BatchPanel.Visibility = Visibility.Collapsed;
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
            }
        }
    }
    public class TestCasesListItem
    {
        public int Index { get; set; }
        public string InputSize { get; set; }
        public string OutputSize { get; set; }
        public TestCaseStatus Status { get; set; }
    }
    public enum TestCaseStatus { Pretest, SystemTest}
}
