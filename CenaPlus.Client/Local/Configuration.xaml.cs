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
using FirstFloor.ModernUI.Windows.Controls;

namespace CenaPlus.Client.Local
{
    /// <summary>
    /// Interaction logic for Configuration.xaml
    /// </summary>
    public partial class Configuration : UserControl
    {
        public Configuration()
        {
            InitializeComponent();
            //find test cases
            string[] files = System.IO.Directory.GetFiles(Static.SourceFileDirectory, "*.in");
            int i = 0;
            Static.TestCases = new List<TestCase>();
            Static.TestCases.Clear();
            foreach (string file in files)
            {
                if (System.IO.File.Exists(System.IO.Path.GetDirectoryName(file) + "\\" + System.IO.Path.GetFileNameWithoutExtension(file) + ".ans"))
                {
                    TestCase t = new TestCase();
                    t.Index = i++;
                    t.Input = file;
                    t.Output = System.IO.Path.GetDirectoryName(file) + "\\" + System.IO.Path.GetFileNameWithoutExtension(file) + ".ans";
                    Static.TestCases.Add(t);
                }
                else if (System.IO.File.Exists(System.IO.Path.GetDirectoryName(file) + "\\" + System.IO.Path.GetFileNameWithoutExtension(file) + ".out"))
                {
                    TestCase t = new TestCase();
                    t.Index = i++;
                    t.Input = file;
                    t.Output = System.IO.Path.GetDirectoryName(file) + "\\" + System.IO.Path.GetFileNameWithoutExtension(file) + ".out";
                    Static.TestCases.Add(t);
                }
            }
            TestCasesListBox.ItemsSource = Static.TestCases;
        }

        private void TestCasesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TestCasesListBox.SelectedItem == null)
            {
                spDetails.Visibility = Visibility.Collapsed;
            }
            else
            {
                spDetails.Visibility = Visibility.Visible;
                txtInput.Text = (TestCasesListBox.SelectedItem as TestCase).Input;
                txtOutput.Text = (TestCasesListBox.SelectedItem as TestCase).Output;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (System.IO.File.Exists(txtInput.Text) && System.IO.File.Exists(txtOutput.Text))
            {
                Static.TestCases[TestCasesListBox.SelectedIndex].Input = txtInput.Text;
                Static.TestCases[TestCasesListBox.SelectedIndex].Output = txtOutput.Text;
                TestCasesListBox.Items.Refresh();
            }
            else
            {
                ModernDialog.ShowMessage("Files not found.", "Error", MessageBoxButton.OK);
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            Static.TestCases.Remove(TestCasesListBox.SelectedItem as TestCase);
            TestCasesListBox.Items.Refresh();
        }
    }
}
