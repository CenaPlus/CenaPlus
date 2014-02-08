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
using System.IO;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using FirstFloor.ModernUI.Windows.Controls;
using CenaPlus.Entity;
namespace CenaPlus.Server.ServerMode.Contest.Problem
{
    /// <summary>
    /// Interaction logic for Content.xaml
    /// </summary>
    public partial class Content : UserControl, IContent
    {
        private int id;
        public Content()
        {
            InitializeComponent();
        }
        private void ComboBoxFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!richMain.Selection.IsEmpty)
            {
                richMain.Selection.ApplyPropertyValue(RichTextBox.FontSizeProperty, Convert.ToDouble((ComboBoxFontSize.SelectedItem as ListViewItem).Content));
            }
            richMain.Focus();
        }

        private void ComboBoxFontColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!richMain.Selection.IsEmpty)
            {
                TextRange range = new TextRange(richMain.Selection.Start, richMain.Selection.End);
                if (ComboBoxFontColor.SelectedValue.ToString() != "None")
                {
                    Brush b = (Brush)new BrushConverter().ConvertFromString(ComboBoxFontColor.SelectedValue.ToString());
                    range.ApplyPropertyValue(RichTextBox.ForegroundProperty, b);
                }
                else
                {
                    range.ClearAllProperties();
                }
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string content;
            using (MemoryStream mem = new MemoryStream())
            {
                new TextRange(richMain.Document.ContentStart, richMain.Document.ContentEnd).Save(mem, DataFormats.Rtf);
                content = Encoding.UTF8.GetString(mem.ToArray());
            }
            App.Server.UpdateProblem(id, null, content, null, null, null, null, null, null, null, null, null,null);
            ModernDialog.ShowMessage("Saved", "Error", MessageBoxButton.OK);
        }
        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            id = int.Parse(e.Fragment);
            var content = App.Server.GetProblem(id).Content;
            using (MemoryStream mem = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                new TextRange(richMain.Document.ContentStart, richMain.Document.ContentEnd).Load(mem, DataFormats.Rtf);
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


    }
}
