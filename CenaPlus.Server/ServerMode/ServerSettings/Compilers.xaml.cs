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
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using FirstFloor.ModernUI.Windows.Controls;
using CenaPlus.Server.Bll;
namespace CenaPlus.Server.ServerMode.ServerSettings
{
    /// <summary>
    /// Interaction logic for Compilers.xaml
    /// </summary>
    public partial class Compilers : UserControl,IContent
    {
        public Compilers()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            App.Server.SetConfig(ConfigKey.Compiler.C, TxtC.Text);
            App.Server.SetConfig(ConfigKey.Compiler.CXX, TxtCXX.Text);
            App.Server.SetConfig(ConfigKey.Compiler.Javac, TxtJavac.Text);
            App.Server.SetConfig(ConfigKey.Compiler.Java, TxtJava.Text);
            App.Server.SetConfig(ConfigKey.Compiler.Pascal, TxtPascal.Text);
            App.Server.SetConfig(ConfigKey.Compiler.Python27, TxtPython27.Text);
            App.Server.SetConfig(ConfigKey.Compiler.Python33, TxtPython33.Text);
            App.Server.SetConfig(ConfigKey.Compiler.Ruby, TxtRuby.Text);
            ModernDialog.ShowMessage("Saved", "Message", MessageBoxButton.OK);
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
        }

        public void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            TxtC.Text = App.Server.GetConfig(ConfigKey.Compiler.C) ?? ConfigKey.Compiler.DefaultC;
            TxtCXX.Text = App.Server.GetConfig(ConfigKey.Compiler.CXX) ?? ConfigKey.Compiler.DefaultCXX;
            TxtJavac.Text = App.Server.GetConfig(ConfigKey.Compiler.Javac) ?? ConfigKey.Compiler.DefaultJavac;
            TxtJava.Text = App.Server.GetConfig(ConfigKey.Compiler.Java) ?? ConfigKey.Compiler.DefaultJava;
            TxtPascal.Text = App.Server.GetConfig(ConfigKey.Compiler.Pascal) ?? ConfigKey.Compiler.DefaultPascal;
            TxtPython27.Text = App.Server.GetConfig(ConfigKey.Compiler.Python27) ?? ConfigKey.Compiler.DefaultPython27;
            TxtPython33.Text = App.Server.GetConfig(ConfigKey.Compiler.Python33) ?? ConfigKey.Compiler.DefaultPython33;
            TxtRuby.Text = App.Server.GetConfig(ConfigKey.Compiler.Ruby) ?? ConfigKey.Compiler.DefaultRuby;
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
        }

    }
}
