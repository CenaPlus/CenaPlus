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
    /// Interaction logic for Local.xaml
    /// </summary>
    public partial class Local : UserControl
    {
        public static string[] Extension = { ".c", ".cpp", ".pas", ".java", ".py", ".rb" };
        public Local()
        {
            InitializeComponent();
        }

        private void imgSourceDirectory_DragEnter(object sender, DragEventArgs e)
        {
            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files.Length != 1)
            {
                imgSourceDirectory.Source = new BitmapImage(new Uri("/CenaPlus.Client;component/Resources/Box_Err.png", UriKind.Relative));
            }
            else
            {
                if (Extension.Contains(System.IO.Path.GetExtension(files[0])))
                {
                    imgSourceDirectory.Source = new BitmapImage(new Uri("/CenaPlus.Client;component/Resources/Box_Hover.png", UriKind.Relative));
                }
                else
                {
                    imgSourceDirectory.Source = new BitmapImage(new Uri("/CenaPlus.Client;component/Resources/Box_Err.png", UriKind.Relative));
                }
            }
        }

        private void imgSourceDirectory_DragLeave(object sender, DragEventArgs e)
        {
            imgSourceDirectory.Source = new BitmapImage(new Uri("/CenaPlus.Client;component/Resources/Box.png", UriKind.Relative));
        }

        private void imgSourceDirectory_Drop(object sender, DragEventArgs e)
        {
            imgSourceDirectory.Source = new BitmapImage(new Uri("/CenaPlus.Client;component/Resources/Box.png", UriKind.Relative));
        }
    }
}
