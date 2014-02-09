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

namespace CenaPlus.Client.Local
{
    /// <summary>
    /// Interaction logic for Configuration.xaml
    /// </summary>
    public partial class Configuration : UserControl
    {
        public List<TestCasesListBoxItem> TestCasesListBoxItems = new List<TestCasesListBoxItem>();
        public Configuration()
        {
            InitializeComponent();
            //find test cases
            string[] files = System.IO.Directory.GetFiles(Static.SourceFileDirectory, "*.in");
            List<TestCase> TestCases = new List<TestCase>();
            int i = 0;
            foreach (string file in files)
            {
                if (System.IO.File.Exists(System.IO.Path.GetDirectoryName(file) + "\\" + System.IO.Path.GetFileNameWithoutExtension(file) + ".ans"))
                {
                    TestCase t = new TestCase();
                    t.Index = i++;
                    t.Input = file;
                    t.Output = System.IO.Path.GetDirectoryName(file) + "\\" + System.IO.Path.GetFileNameWithoutExtension(file) + ".ans";
                    TestCases.Add(t);
                }
                else if (System.IO.File.Exists(System.IO.Path.GetDirectoryName(file) + "\\" + System.IO.Path.GetFileNameWithoutExtension(file) + ".out"))
                {
                    TestCase t = new TestCase();
                    t.Index = i++;
                    t.Input = file;
                    t.Output = System.IO.Path.GetDirectoryName(file) + "\\" + System.IO.Path.GetFileNameWithoutExtension(file) + ".out";
                    TestCases.Add(t);
                }
            }
            Static.TestCases = TestCases;
            TestCasesListBoxItems.Clear();
            foreach (var tc in TestCases)
            {
                TestCasesListBoxItem t = new TestCasesListBoxItem(tc);
                TestCasesListBoxItems.Add(t);
            }
            TestCasesListBox.ItemsSource = TestCasesListBoxItems;
        }
    }
    public class TestCasesListBoxItem: TestCase
    {
        public TestCasesListBoxItem(TestCase TestCase)
        {
            Index = TestCase.Index;
            Input = TestCase.Input;
            Output = TestCase.Output;
        }
        public string Title
        {
            get
            {
                return String.Format("Test case: #" + Index);
            }
        }
        public string Details
        {
            get
            {
                return String.Format(System.IO.Path.GetFileName(Input) + " / " + System.IO.Path.GetFileName(Output));
            }
        }
    }
}
