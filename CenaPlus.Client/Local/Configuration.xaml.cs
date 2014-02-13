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
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;

namespace CenaPlus.Client.Local
{
    /// <summary>
    /// Interaction logic for Configuration.xaml
    /// </summary>
    public partial class Configuration : UserControl, IContent
    {
        public Configuration()
        {
            InitializeComponent();
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

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (txtSpecialJudge.Text != String.Empty && System.IO.File.Exists(txtSpecialJudge.Text))
            {
                ModernDialog.ShowMessage("Special Judger not found.", "Error", MessageBoxButton.OK);
            }
            //Begin compile
            Static.WorkingDirectory = Environment.CurrentDirectory.ToString()+"\\Temp";
            if (System.IO.Directory.Exists(Static.WorkingDirectory))
                System.IO.Directory.Delete(Static.WorkingDirectory, true);
            System.IO.Directory.CreateDirectory(Static.WorkingDirectory);
            //System.IO.File.Copy(Environment.CurrentDirectory.ToString() + "\\Core\\CenaPlus.Core.exe", Static.WorkingDirectory + "\\CenaPlus.Core.exe");
            Static.TimeLimit = Convert.ToInt32(txtTimeLimit.Text);
            Static.MemoryLimit = Convert.ToInt32(txtMemoryLimit.Text);
            if (CenaPlus.Judge.Compiler.NeedCompile.Contains(Static.Language))
            {
                CenaPlus.Judge.Compiler Compiler = new Judge.Compiler();
                switch (Static.Language)
                {
                    case Entity.ProgrammingLanguage.C:
                        Compiler.CompileInfo.Arguments = "gcc -O2 -o Main.exe -DONLINE_JUDGE -Wall -lm --static --std=c99 -fno-asm Main.c";
                        break;
                    case Entity.ProgrammingLanguage.CXX:
                    case Entity.ProgrammingLanguage.CXX11:
                        Compiler.CompileInfo.Arguments = "g++ -O2 -o Main.exe -DONLINE_JUDGE -Wall -lm --static --std=c++98 -fno-asm Main.cpp";
                        break;
                    case Entity.ProgrammingLanguage.Java:
                        Compiler.CompileInfo.Arguments = "javac Main.java";
                        break;
                    case Entity.ProgrammingLanguage.Pascal:
                        Compiler.CompileInfo.Arguments = "fpc -O2 -dONLINE_JUDGE Main.pas";
                        break;
                }
                Compiler.CompileInfo.Language = Static.Language;
                Compiler.CompileInfo.Source = System.IO.File.ReadAllText(Static.SourceFileDirectory + "\\" + Static.FileName + Static.Extension);
                Compiler.CompileInfo.TimeLimit = 1000;
                Compiler.CompileInfo.CenaCoreDirectory = Environment.CurrentDirectory.ToString() + "\\Core\\CenaPlus.Core.exe";
                Compiler.CompileInfo.WorkingDirectory = Static.WorkingDirectory;
                Compiler.Start();
                if (Compiler.CompileResult.CompileFailed)
                {
                    ModernDialog.ShowMessage(Compiler.CompileResult.CompilerOutput, "Compile Error", MessageBoxButton.OK);
                    var frame = NavigationHelper.FindFrame(null, this);
                    if (frame != null)
                    {
                        frame.Source = new Uri("/Local/Local.xaml", UriKind.Relative);
                    }
                    return;
                }
                else
                {
                    var frame = NavigationHelper.FindFrame(null, this);
                    if (frame != null)
                    {
                        frame.Source = new Uri("/Local/Result.xaml", UriKind.Relative);
                    }
                    return;
                }
            }
        }

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {

        }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {

        }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
            //find test cases
            string[] files = System.IO.Directory.GetFiles(Static.SourceFileDirectory, "*.in");
            int i = 0;
            Static.TestCases = new List<TestCase>();
            Static.TestCases.Clear();
            foreach (string file in files)
            {
                if (System.IO.Path.GetFileName(file).ToLower() == "spj.exe")
                {
                    txtSpecialJudge.Text = file;
                }
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

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {

        }
    }
}
