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

namespace CenaPlus.Client.Remote.Contest
{
    /// <summary>
    /// Interaction logic for Hack.xaml
    /// </summary>
    public partial class Hack : UserControl, IContent
    {
        private int record_id;
        private Entity.Record record;
        public Hack()
        {
            InitializeComponent();
            RichTextEditor.HighLightEdit.HighLight(txtData);
            RichTextEditor.HighLightEdit.HighLight(txtSource);
        }

        private void HackModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbbLanguage == null) return;
            if (HackModeComboBox.SelectedIndex == 0)
            {
                cbbLanguage.Visibility = Visibility.Collapsed;
            }
            else if (HackModeComboBox.SelectedIndex == 1)
            {
                cbbLanguage.Visibility = Visibility.Visible;
            }
        }

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
            record_id = int.Parse(e.Fragment);
            record = App.Server.GetRecord(record_id);
            txtSource.Document.Blocks.Clear();
            txtSource.Document.Blocks.Add(new Paragraph(new Run(record.Code)));
            var p = App.Server.GetProblem(record.ProblemID);
            var languages = Enum.GetNames(typeof(Entity.ProgrammingLanguage));
            cbbLanguage.Items.Clear();
            foreach (var l in languages)
            {
                cbbLanguage.Items.Add(l);
            }
            var forbidden = p.ForbiddenLanguages.ToList();
            foreach (var l in forbidden)
            {
                var index = cbbLanguage.Items.IndexOf(l);
                if (index >= 0)
                    cbbLanguage.Items.RemoveAt(index);
            }
            cbbLanguage.Items.Refresh();
            cbbLanguage.SelectedIndex = 0;
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

        private void btnHack_Click(object sender, RoutedEventArgs e)
        {
            Entity.ProgrammingLanguage? language;
            if(HackModeComboBox.SelectedIndex == 0) language = null;
            else language = (Entity.ProgrammingLanguage?)cbbLanguage.SelectedItem;
            App.Server.HackRecord(record_id, new TextRange(txtData.Document.ContentStart, txtData.Document.ContentEnd).Text, language);
            var cid = App.Server.GetProblemRelatedContest(record.ProblemID);
            var frame = NavigationHelper.FindFrame(null, this);
            if (frame != null)
            {
                frame.Source = new Uri("/Remote/Contest/Standings.xaml#" + cid, UriKind.Relative);
            }
        }

        private void imgDataPath_DragEnter(object sender, DragEventArgs e)
        {
            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files.Length != 1)
            {
                imgDataPath.Source = new BitmapImage(new Uri("/CenaPlus.Client;component/Resources/Box_Err.png", UriKind.Relative));
            }
            else
            {
                imgDataPath.Source = new BitmapImage(new Uri("/CenaPlus.Client;component/Resources/Box_Hover.png", UriKind.Relative));
            }
        }

        private void imgDataPath_DragLeave(object sender, DragEventArgs e)
        {
            imgDataPath.Source = new BitmapImage(new Uri("/CenaPlus.Client;component/Resources/Box.png", UriKind.Relative));
        }

        private void imgDataPath_Drop(object sender, DragEventArgs e)
        {
            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            imgDataPath.Source = new BitmapImage(new Uri("/CenaPlus.Client;component/Resources/Box.png", UriKind.Relative));
            if (files.Length != 1)
            {
                ModernDialog.ShowMessage("You must select a single file.", "Error", MessageBoxButton.OK);
                return;
            }
            else
            {
                txtData.Document.Blocks.Clear();
                txtData.Document.Blocks.Add(new Paragraph(new Run(System.IO.File.ReadAllText(files[0]))));
                var l = Entity.DetectLanguage.PathToLanguage(files[0]);
                if (l != null)
                {
                    cbbLanguage.SelectedItem = l.ToString();
                }
            }
        }
    }
}
