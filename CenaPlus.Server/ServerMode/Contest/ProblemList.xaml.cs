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
using System.Web.Script.Serialization;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;
using Microsoft.Win32;
using CenaPlus.Entity;

namespace CenaPlus.Server.ServerMode.Contest
{
    /// <summary>
    /// Interaction logic for ProblemList.xaml
    /// </summary>
    public partial class ProblemList : UserControl, IContent
    {
        private List<ProblemListItem> ProblemListItems = new List<ProblemListItem>();
        private int contestID;

        public ProblemList()
        {
            InitializeComponent();
            ProblemListView.ItemsSource = ProblemListItems;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            int id = App.Server.CreateProblem(contestID, "New Problem", "Insert description here.", 0,1000,256*1024*1024,null,null,null,null,null,null,Enumerable.Empty<ProgrammingLanguage>());
            var frame = NavigationHelper.FindFrame(null, this);
            if (frame != null)
            {
                frame.Source = new Uri("/ServerMode/Contest/Problem/Problem.xaml#" + id, UriKind.Relative);
            }
        }

        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            if (ProblemListView.SelectedIndex != -1)
            {
                var frame = NavigationHelper.FindFrame(null, this);
                if (frame != null)
                {
                    frame.Source = new Uri("/ServerMode/Contest/Problem/Problem.xaml#" + ProblemListView.SelectedValue, UriKind.Relative);
                }
            }
        }

        private void ProblemListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ProblemListView.SelectedIndex != -1)
            {
                var frame = NavigationHelper.FindFrame(null, this);
                if (frame != null)
                {
                    frame.Source = new Uri("/ServerMode/Contest/Problem/Problem.xaml#" + ProblemListView.SelectedValue, UriKind.Relative);
                }
            }
        }
        
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ProblemListView.SelectedIndex != -1)
            {
                int id = (int)ProblemListView.SelectedValue;
                App.Server.DeleteProblem(id);
                for (int i = ProblemListView.SelectedIndex + 1; i < ProblemListItems.Count; i++)
                {
                    ProblemListItems[i].Index--;
                }
                ProblemListItems.RemoveAt(ProblemListView.SelectedIndex);
                ProblemListView.Items.Refresh();
            }
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            contestID = int.Parse(e.Fragment);
            var list = from id in App.Server.GetProblemList(contestID)
                       let p = App.Server.GetProblem(id)
                       select new ProblemListItem
                       {
                           ID = p.ID,
                           Title = p.Title,
                           Score = p.Score,
                           TestCasesCount = p.TestCasesCount
                       };
            ProblemListItems.Clear();
            ProblemListItems.AddRange(list);
            for (int i = 0; i < ProblemListItems.Count; i++)
            {
                ProblemListItems[i].Index = i;
            }
            ProblemListView.Items.Refresh();
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
        class ProblemListItem : Entity.Problem
        {
            public int Index { get; set; }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            if (ProblemListView.SelectedValue == null) return;
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Cena+ Problem File(*.cep)|*.cep";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true; //保存对话框是否记忆上次打开的目录 
            saveFileDialog.Title = "Export to";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName == null || saveFileDialog.FileName == String.Empty)
            {
                return;
            }
            var Problem = App.Server.GetProblem(Convert.ToInt32(ProblemListView.SelectedValue));
            var tids = App.Server.GetTestCaseList(Problem.ID);
            List<Entity.TestCase> cases = new List<TestCase>();
            foreach (var id in tids)
            {
                cases.Add(App.Server.GetTestCaseFull(id));
            }
            var Dir = Environment.CurrentDirectory + "\\Temp\\P" + Problem.ID;
            if (!System.IO.Directory.Exists(Dir))
            {
                System.IO.Directory.CreateDirectory(Dir);
            }
            System.Threading.Tasks.Task.Factory.StartNew(() => {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 2147483647;
                System.IO.File.WriteAllText(Dir + "\\Problem.json", jss.Serialize(Problem));
                System.IO.File.WriteAllText(Dir + "\\TestCases.json", jss.Serialize(cases));
                Bll.ZipHelper.Zip(Dir + "\\", saveFileDialog.FileName);
                ModernDialog.ShowMessage("Cena+ problem file has been saved successful.", "Message", MessageBoxButton.OK);
            });
        }

        private void imgSourceDirectory_DragEnter(object sender, DragEventArgs e)
        {
            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files.Any(x => System.IO.Path.GetExtension(x) != ".cep"))
            {
                imgSourceDirectory.Source = new BitmapImage(new Uri("/CenaPlus.Server;component/Resources/Box_Err.png", UriKind.Relative));
            }
            else
            {
                imgSourceDirectory.Source = new BitmapImage(new Uri("/CenaPlus.Server;component/Resources/Box_Hover.png", UriKind.Relative));
            }
        }

        private void imgSourceDirectory_DragLeave(object sender, DragEventArgs e)
        {
            imgSourceDirectory.Source = new BitmapImage(new Uri("/CenaPlus.Server;component/Resources/Box.png", UriKind.Relative));
        }

        private void imgSourceDirectory_Drop(object sender, DragEventArgs e)
        {
            imgSourceDirectory.Source = new BitmapImage(new Uri("/CenaPlus.Server;component/Resources/Box.png", UriKind.Relative));
            pbLoading.Visibility = System.Windows.Visibility.Visible;
            lbDragTip.Visibility = System.Windows.Visibility.Collapsed;
            imgSourceDirectory.IsEnabled = false;
            System.Threading.Tasks.Task.Factory.StartNew(() => {
                var files = (from f in (e.Data.GetData(DataFormats.FileDrop) as string[])
                             where System.IO.Path.GetExtension(f) == ".cep"
                             select f).ToList();
                foreach (var f in files)
                {
                    var dir = Environment.CurrentDirectory + "\\Temp\\Analysis";
                    if (System.IO.Directory.Exists(dir))
                        System.IO.Directory.Delete(dir, true);
                    System.IO.Directory.CreateDirectory(dir);
                    Bll.ZipHelper.UnZip(f, dir);
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    jss.MaxJsonLength = 2147483647;
                    var problem_json = System.IO.File.ReadAllText(dir + "\\Problem.json");
                    var testcases_json = System.IO.File.ReadAllText(dir + "\\TestCases.json");
                    var problem = jss.Deserialize<Entity.Problem>(problem_json);
                    problem.ID = App.Server.CreateProblem(contestID, problem.Title, problem.Content, problem.Score, problem.TimeLimit, problem.MemoryLimit, problem.Std, problem.Spj, problem.Validator, problem.StdLanguage, problem.SpjLanguage, problem.ValidatorLanguage, problem.ForbiddenLanguages);
                    var testcases = jss.Deserialize<List<Entity.TestCase>>(testcases_json);
                    foreach (var tc in testcases)
                    {
                        App.Server.CreateTestCase(problem.ID, tc.Input, tc.Output, tc.Type);
                    }
                    var p = App.Server.GetProblem(problem.ID);
                    ProblemListItem item = new ProblemListItem()
                    {
                        ID = p.ID,
                        Title = p.Title,
                        Score = p.Score,
                        TestCasesCount = p.TestCasesCount
                    };
                    Dispatcher.Invoke(new Action(() => {
                        ProblemListItems.Add(item);
                        ProblemListItems.Sort((x, y) => { return y.Score - x.Score; });
                        ProblemListView.Items.Refresh();
                    }));

                    try
                    {
                    }
                    catch
                    {
                        continue;
                    }
                    finally
                    {
                    }
                    Dispatcher.Invoke(new Action(() =>
                    {
                        pbLoading.Visibility = System.Windows.Visibility.Collapsed;
                        lbDragTip.Visibility = System.Windows.Visibility.Visible;
                        imgSourceDirectory.IsEnabled = true;
                    }));
                }
            });
        }
    }
}
