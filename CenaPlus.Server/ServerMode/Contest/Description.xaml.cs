using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;
using CenaPlus.Entity;
namespace CenaPlus.Server.ServerMode.Contest
{
    /// <summary>
    /// Interaction logic for Description.xaml
    /// </summary>
    public partial class Description : UserControl, IContent
    {
        private int contestID;
        public Description()
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
            string desc;
            using (MemoryStream mem = new MemoryStream())
            {
                new TextRange(richMain.Document.ContentStart, richMain.Document.ContentEnd).Save(mem, DataFormats.Rtf);
                desc = Encoding.UTF8.GetString(mem.ToArray());
            }
            App.Server.UpdateContest(contestID, null, desc, null, null, null);
            ModernDialog.ShowMessage("Saved", "Message", MessageBoxButton.OK);
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            contestID = int.Parse(e.Fragment);
            var desc = App.Server.GetContest(contestID).Description;
            using (MemoryStream mem = new MemoryStream(Encoding.UTF8.GetBytes(desc)))
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
