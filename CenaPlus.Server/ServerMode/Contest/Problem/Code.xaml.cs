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
using System.Text.RegularExpressions;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using FirstFloor.ModernUI.Windows.Controls;
using CenaPlus.Entity;
namespace CenaPlus.Server.ServerMode.Contest.Problem
{
    /// <summary>
    /// Interaction logic for Code.xaml
    /// </summary>
    public partial class Code : UserControl, IContent
    {
        private string field;
        private int id;

        public Code()
        {
            InitializeComponent();
            RichTextEditor.HighLightEdit.HighLight(txtCode);
            cbLanguage.ItemsSource = Enum.GetNames(typeof(Entity.ProgrammingLanguage));
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var code = new TextRange(txtCode.Document.ContentStart, txtCode.Document.ContentEnd).Text;
            ProgrammingLanguage language = (ProgrammingLanguage)cbLanguage.SelectedIndex;

            switch (field)
            {
                case "Spj":
                    App.Server.UpdateProblem(id, null, null, null, null, null, null, code, null, null, language, null,null);
                    break;
                case "Std":
                    App.Server.UpdateProblem(id, null, null, null, null, null, code, null, null, language, null, null,null);
                    break;
                case "Validator":
                    App.Server.UpdateProblem(id, null, null, null, null, null, null, null, code, null, null, language,null);
                    break;
            }

            ModernDialog.ShowMessage("Saved", "Message", MessageBoxButton.OK);
        }

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
            var match = Regex.Match(e.Fragment, @"^field=(.*?)&id=(\d*)$");
            field = match.Groups[1].Value;
            id = int.Parse(match.Groups[2].Value);

            var problem = App.Server.GetProblem(id);

            string value;
            switch (field)
            {
                case "Spj":
                    value = problem.Spj == null ? "" : problem.Spj;
                    break;
                case "Std":
                    value = problem.Std == null ? "" : problem.Std;
                    break;
                case "Validator":
                    value = problem.Validator == null ? "" : problem.Validator;
                    break;
                default:
                    throw new ArgumentException("field");
            }

            ProgrammingLanguage language;
            switch (field)
            {
                case "Spj":
                    language = problem.SpjLanguage == null ? ProgrammingLanguage.C : problem.SpjLanguage.Value;
                    break;
                case "Std":
                    language = problem.StdLanguage == null ? ProgrammingLanguage.C : problem.StdLanguage.Value;
                    break;
                case "Validator":
                    language = problem.ValidatorLanguage == null ? ProgrammingLanguage.C : problem.ValidatorLanguage.Value;
                    break;
                default:
                    throw new ArgumentException("field");
            }

            txtCode.Document.Blocks.Clear();
            txtCode.Document.Blocks.Add(new Paragraph(new Run(value)));
            cbLanguage.SelectedIndex = (int)language;
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

        private void imgSourceDirectory_DragEnter(object sender, DragEventArgs e)
        {
            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files.Length != 1)
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
            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files.Length != 1)
            {
                ModernDialog.ShowMessage("You must select a single file.", "Error", MessageBoxButton.OK);
                return;
            }
            else
            {
                txtCode.Document.Blocks.Clear();
                txtCode.Document.Blocks.Add(new Paragraph(new Run(System.IO.File.ReadAllText(files[0]))));
            }
        }
    }
}
