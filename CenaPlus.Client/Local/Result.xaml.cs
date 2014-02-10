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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;

namespace CenaPlus.Client.Local
{
    /// <summary>
    /// Interaction logic for Result.xaml
    /// </summary>
    public partial class Result : UserControl, IContent
    {
        public List<ResultListBoxItem> ResultListBoxItems;
        public Result()
        {
            InitializeComponent();
        }
        public void Judge(object JudgeTask)
        {
            var jt = JudgeTask as ResultListBoxItem;
            CenaPlus.Judge.Runner Runner = new Judge.Runner();
            switch (Static.Language)
            { 
                case Entity.ProgrammingLanguage.Java:
                    Runner.RunnerInfo.Cmd = "java Main";
                    break;
                case Entity.ProgrammingLanguage.Python27:
                case Entity.ProgrammingLanguage.Python33:
                    Runner.RunnerInfo.Cmd = "python Main.py";
                    break;
                case Entity.ProgrammingLanguage.Ruby:
                    Runner.RunnerInfo.Cmd = "ruby Main.rb";
                    break;
                default:
                    Runner.RunnerInfo.Cmd = "Main.exe";
                    break;
            }
            Runner.RunnerInfo.HighPriorityTime = 1000;
            Runner.RunnerInfo.MemoryLimit = Static.MemoryLimit;
            Runner.RunnerInfo.StdInFile = jt.TestCase.Input;
            Runner.RunnerInfo.StdOutFile = Static.WorkingDirectory + "\\" + jt.TestCase.Index + ".user.out";
            Runner.RunnerInfo.TimeLimit = Static.TimeLimit;
            Runner.RunnerInfo.WorkingDirectory = Static.WorkingDirectory;
            Runner.RunnerInfo.CenaCoreDirectory = Environment.CurrentDirectory + "\\Core\\CenaPlus.Core.exe";
            Runner.Start();
            jt.Time = Runner.RunnerResult.TimeUsed;
            jt.Memory = Runner.RunnerResult.PagedSize;
            if (Static.Language == Entity.ProgrammingLanguage.Java)
                jt.Memory = Runner.RunnerResult.WorkingSetSize;
            if (Runner.RunnerResult.ExitCode != 0 && (Static.Language != Entity.ProgrammingLanguage.C && Runner.RunnerResult.ExitCode != 1))
            {
                if (jt.Time > Static.TimeLimit)
                {
                    jt.Status = Entity.RecordStatus.TimeLimitExceeded;
                }
                else
                {
                    jt.Status = Entity.RecordStatus.RuntimeError;
                }
            }
            else
            {
                if (jt.Memory > Static.MemoryLimit)
                {
                    jt.Status = Entity.RecordStatus.MemoryLimitExceeded;
                }
                else
                { 
                    // TODO: Validate
                    Runner = null;
                    GC.Collect();
                    Runner = new Judge.Runner();
                    Runner.RunnerInfo.StdOutFile = Static.WorkingDirectory + "\\" + jt.TestCase.Index + ".validator.out";
                    Runner.RunnerInfo.HighPriorityTime = 1000;
                    Runner.RunnerInfo.MemoryLimit = 1024 * 128;
                    Runner.RunnerInfo.TimeLimit = 3000;
                    Runner.RunnerInfo.WorkingDirectory = Static.WorkingDirectory;
                    Runner.RunnerInfo.CenaCoreDirectory = Environment.CurrentDirectory + "\\Core\\CenaPlus.Core.exe";
                    System.IO.File.Copy(jt.TestCase.Input, Static.WorkingDirectory + "\\input" + jt.TestCase.Index + ".in", true);
                    System.IO.File.Copy(jt.TestCase.Output, Static.WorkingDirectory + "\\output" + jt.TestCase.Index + ".out", true);
                    Runner.RunnerInfo.Cmd = String.Format("{0} {1} {2} {3}", "spj.exe", "output" + jt.TestCase.Index + ".out", jt.TestCase.Index + ".user.out", "\\input" + jt.TestCase.Index + ".in");
                    Runner.Start();
                    if (Runner.RunnerResult.ExitCode >= 0 && Runner.RunnerResult.ExitCode <= 2)
                    {
                        jt.Status = (Entity.RecordStatus)Runner.RunnerResult.ExitCode;
                        try
                        {
                            jt.Detail = System.IO.File.ReadAllText(Static.WorkingDirectory + "\\" + jt.TestCase.Index + ".validator.out");
                        }
                        catch { }
                    }
                    else
                    {
                        jt.Status = Entity.RecordStatus.ValidatorError;
                    }
                }
            }
            Dispatcher.Invoke(new Action(() => {
                ResultListBoxItems.Add(jt);
                ResultListBoxItems.Sort((a, b) => a.TestCase.Index - b.TestCase.Index);
                ResultListBox.Items.Refresh();
            }));
        }

        private void ResultListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtDetail == null) return;
            if (ResultListBox.SelectedItem == null)
            {
                txtDetail.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtDetail.Visibility = Visibility.Visible;
                txtDetail.Text = (ResultListBox.SelectedItem as ResultListBoxItem).Detail;
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
            if (Static.SPJDirectory != null && Static.SPJDirectory != String.Empty)
            {
                System.IO.File.Copy(Static.SPJDirectory, Static.WorkingDirectory + "\\SPJ.exe", true);
            }
            else
            {
                System.IO.File.Copy(Environment.CurrentDirectory + "\\Core\\Standard.exe", Static.WorkingDirectory + "\\SPJ.exe", true);
            }
            ResultListBoxItems = null;
            GC.Collect();
            ResultListBoxItems = new List<ResultListBoxItem>();
            ResultListBoxItems.Clear();
            ResultListBox.ItemsSource = ResultListBoxItems;
            foreach (var tc in Static.TestCases)
            {
                ResultListBoxItem t = new ResultListBoxItem();
                t.TestCase = tc;
                Thread judge = new Thread(Judge);
                judge.Start(t);
            }
        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {

        }
    }
    public class ResultListBoxItem
    {
        public string Details
        {
            get
            {
                return String.Format("#{0} : {1} ms / {2} KB", TestCase.Index, Time, Memory);
            }
        }
        public TestCase TestCase = new TestCase();
        public Entity.RecordStatus Status { get; set; }
        public string StatusColor
        {
            get
            {
                // TODO: Prettify
                switch (Status)
                {
                    case Entity.RecordStatus.Accepted:
                        return "Green";
                    case Entity.RecordStatus.CompileError:
                        return "Orange";
                    case Entity.RecordStatus.Pending:
                        return "Indigo";
                    default:
                        return "Red";
                }
            }
        }
        public int Time { get; set; }
        public int Memory { get; set; }
        public string Detail { get; set; }

    }
}
