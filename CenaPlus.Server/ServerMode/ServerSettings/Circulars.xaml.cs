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
using System.IO;
using Microsoft.Win32;

namespace CenaPlus.Server.ServerMode.ServerSettings
{
    public partial class Circulars : UserControl
    {
        public Circulars()
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
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "rich text file|*.rtf";
            try
            {
                if ((bool)save.ShowDialog())
                {
                    SaveFile(save.FileName);
                    MessageBox.Show("保存成功");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void SaveFile(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create);

            TextRange range = new TextRange(richMain.Document.ContentStart, richMain.Document.ContentEnd);
            range.Save(fs, DataFormats.Rtf);
            fs.Close();
        }

        //http://www.csharpwin.com/csharpspace/10836r4358.shtml insert local pic
    }
}
