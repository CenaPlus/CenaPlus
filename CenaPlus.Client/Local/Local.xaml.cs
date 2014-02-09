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
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;

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
            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files.Length != 1)
            {
                ModernDialog.ShowMessage("You must select a single file.", "Error", MessageBoxButton.OK);
                return;
            }
            else
            {
                if (!Extension.Contains(System.IO.Path.GetExtension(files[0])))
                {
                    ModernDialog.ShowMessage("The file type is not supported.", "Error", MessageBoxButton.OK);
                    return;
                }
                else
                {
                    Static.SourceFileDirectory = System.IO.Path.GetDirectoryName(files[0]);
                    Static.FileName = System.IO.Path.GetFileNameWithoutExtension(files[0]);
                    Static.Extension = System.IO.Path.GetExtension(files[0]);
                    switch (Static.Extension)
                    { 
                        case ".c":
                            Static.Language = Entity.ProgrammingLanguage.C;
                            break;
                        case ".cpp":
                            Static.Language = Entity.ProgrammingLanguage.CXX;
                            break;
                        case ".java":
                            Static.Language = Entity.ProgrammingLanguage.Java;
                            break;
                        case ".pas":
                            Static.Language = Entity.ProgrammingLanguage.Pascal;
                            break;
                        case ".py":
                            Static.Language = Entity.ProgrammingLanguage.Python27;
                            break;
                        case ".rb":
                            Static.Language = Entity.ProgrammingLanguage.Ruby;
                            break;
                        default:
                            break;
                    }
                    var frame = NavigationHelper.FindFrame(null, this);
                    if (frame != null)
                    {
                        frame.Source = new Uri("/Local/Configuration.xaml", UriKind.Relative);
                    }
                }
            }
        }
    }
}
