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

namespace CenaPlus.Server.ServerMode.Contest
{
    /// <summary>
    /// Interaction logic for Description.xaml
    /// </summary>
    public partial class Description : UserControl
    {
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
    }
}
